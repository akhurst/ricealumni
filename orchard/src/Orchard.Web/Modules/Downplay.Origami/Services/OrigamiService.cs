using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

using Orchard;
using Orchard.Themes;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.DisplayManagement.Descriptors;
using Orchard.FileSystems.VirtualPath;
using Orchard.UI.Zones;
using Orchard.Logging;

using ClaySharp.Implementation;
using Autofac;
using Downplay.Origami.Placement;
using Downplay.Origami.Shapes;

namespace Downplay.Origami.Services
{
    public class OrigamiService : IOrigamiService
    {
        private readonly IComponentContext _context;

        private readonly dynamic Shape;
        private readonly IShapeTableManager _shapeTableManager;
        private readonly Lazy<IThemeManager> _themeService;
        private readonly RequestContext _requestContext;
        private readonly IShapeFactory _shapeFactory;
        private readonly IVirtualPathProvider _virtualPathProvider;

        public OrigamiService(
            IComponentContext context,
            IShapeFactory shapeFactory,
            Lazy<IThemeManager> themeService,
            IShapeTableManager shapeTableManager,
            RequestContext requestContext,
            IVirtualPathProvider virtualPathProvider
            )
        {
            _context = context;
            _virtualPathProvider = virtualPathProvider;
            Shape = _shapeFactory = shapeFactory;
            _shapeTableManager = shapeTableManager;
            _themeService = themeService;
            _requestContext = requestContext;
            Logger = NullLogger.Instance;
        }
        public ILogger Logger { get; set; }
        private IEnumerable<IModelDriver> _drivers;
        public IEnumerable<IModelDriver> Drivers
        {
            get
            {
                if (_drivers == null)
                    _drivers = _context.Resolve<IEnumerable<IModelDriver>>();
                return _drivers;
            }
        }

        public ModelShapeBuilder Builder(object model) {
            return new ModelShapeBuilder(model,_shapeFactory);
        }

        public void Build(ModelShapeBuilder builder, dynamic root) {
            var context = builder.Context;
            context.Shape = root;

            BindPlacement(context, context.DisplayType, context.Stereotype, builder.ContentType);

            foreach (var driver in Drivers) {
                var result = driver.Run(context);
                // Null check for driver result
                if (result != null) {
                    result.Apply(context);
                }
            }

            // Chain sub results?
            // They are applied to the same base object (as opposed to rendering an entirely new shape with its own zones)
            foreach (var chain in context.ChainedResults) {
                var newRoot = chain.Root ?? root;

                Build(
                    Builder(chain.Model)
                        // TODO: Could be nice to chain from displays to editors and back; assuming updater is available 
                        // To do that we could pass a whole new Builder into context instead of this hack?
                        .WithMode(context.Mode)
                        .WithUpdater(context.Updater,chain.Prefix)
                        .WithDisplayType(chain.DisplayType??context.DisplayType)
                        .WithStereotype(context.Stereotype)
                        .WithContentType(builder.ContentType)
                        .WithParent(context)
                        .WithParadigms(context.Paradigms)
                        ,newRoot);

                // Fire an event so parent shape can perform work after the update
                chain.OnCompleted(context);
            }

            // Invoke Updated event now all drivers have been executed
            if (context.Updater != null) {
                context.InvokeUpdated();
            }

            // Done
        }

        private void BindPlacement(ModelShapeContext context, string displayType, string stereotype, string contentType)
        {

            context.FindPlacement = (partShapeType, differentiator, defaultLocation) =>
            {
                var theme = _themeService.Value.GetRequestTheme(_requestContext);
                var shapeTable = _shapeTableManager.GetShapeTable(theme.Id);
                var request = _requestContext.HttpContext.Request;

                ShapeDescriptor descriptor;
                if (shapeTable.Descriptors.TryGetValue(partShapeType, out descriptor))
                {
                    var placementContext = new ModelShapePlacementContext
                    {
                        ModelContext = context,
                        Stereotype = stereotype,
                        DisplayType = displayType,
                        Differentiator = differentiator,
                        ContentType = String.IsNullOrWhiteSpace(contentType)?context.Model.GetType().Name:contentType,
                        // Get the current app-relative path, i.e. ~/my-blog/foo
                        // TODO: This is for Url placement to work. It'd be far better if we could just inject any old strings into a properties dictionary,
                        // or even a dictionary of LazyFields, so work like this doesn't have to happen. Not sure how long ToAppRelative takes but it's
                        // getting called for every single placement op and might never get used...
                        Path = VirtualPathUtility.AppendTrailingSlash(_virtualPathProvider.ToAppRelative(request.Path)) 
                    };

                    var placement = descriptor.Placement(placementContext);
                    if (placement != null)
                    {
                        placement.Source = placementContext.Source;
                        return placement;
                    }
                }

                return new PlacementInfo
                {
                    Location = defaultLocation,
                    Source = String.Empty
                };
            };
        }

        #region Content Display implementation


        public ModelShapeBuilder ContentBuilder(IContent content) {
            var contentTypeDefinition = content.ContentItem.TypeDefinition;
            string stereotype;
            if (!contentTypeDefinition.Settings.TryGetValue("Stereotype", out stereotype))
                stereotype = "Content";
            var builder = Builder(content)
                .WithContentType(content.ContentItem.ContentType)
                .WithStereotype(stereotype);
            string paradigms;
            if (contentTypeDefinition.Settings.TryGetValue("Paradigms", out paradigms)) {
                var paradigmList = paradigms.Split(new[]{','}, StringSplitOptions.RemoveEmptyEntries);
                builder.WithParadigms(paradigmList);
            }
            return builder;
        }
        // TODO: Following 3 functions are possibly redundant
        public dynamic BuildContentEditor(IContent content, string displayType, string groupId, IUpdateModel updater = null, ModelShapeContext parentContext = null) {
            return BuildContentShape("Editor", content, displayType, groupId, null, updater, parentContext);
        }
        public dynamic BuildContentDisplay(IContent content, string displayType, string groupId, IUpdateModel updater = null, ModelShapeContext parentContext = null) {
            return BuildContentShape("Display", content, displayType, groupId, null, null, parentContext);
        }
        public dynamic BuildContentShape(string mode, IContent content, string displayType, string groupId, dynamic itemShape = null, IUpdateModel updater = null, ModelShapeContext parentContext = null)
        {
            // TODO: This is all very well. But we *still* haven't found a way to sanitize Prefix for child editors :(  ... perhaps with a Clay behaviour on shapeFactory?

            var actualDisplayType = string.IsNullOrWhiteSpace(displayType) ? "Detail" : displayType;

            var builder = ContentBuilder(content)
                .WithMode(mode)
                .WithDisplayType(actualDisplayType)
                ;
            if (itemShape == null) {
                var actualShapeType = builder.Context.Stereotype;
                // Slight Editor hack
                if (mode == "Editor") {
                    actualShapeType = builder.Context.Stereotype + "_Edit";
                }
                itemShape = CreateItemShape(actualShapeType);
                itemShape.ContentItem = content.ContentItem;
                itemShape.Metadata.DisplayType = actualDisplayType;
            }

            if (parentContext != null) {
                builder.WithParent(parentContext)
                    .WithParadigms(parentContext.Paradigms);
            }
            if (updater != null) {
                builder.WithUpdater(updater,parentContext==null?"Content":parentContext.Prefix+".Content");
            }
            // Origami build (ContentHandlerModelDriver will delegate to ContentPartDrivers and everything else...)
            Build(builder,itemShape);
            return itemShape;
        }

        private dynamic CreateItemShape(string actualShapeType, string prefix = null)
        {
            var zoneHoldingBehavior = (prefix==null)?
                new ZoneHoldingBehavior(() => _shapeFactory.Create("ContentZone", Arguments.Empty()),null)
                : new ZoneHoldingBehavior(() => _shapeFactory.Create("ContentZone", Arguments.Empty(),new[]{new PrefixPrependingBehavior(prefix) }),null);
            return _shapeFactory.Create(actualShapeType, Arguments.Empty(), new[] { zoneHoldingBehavior });
        }

        #endregion

        public dynamic ContentShape(IContent content, string displayType, bool editor=false,string prefix=null) {
             string stereotype;
            if (!content.ContentItem.TypeDefinition.Settings.TryGetValue("Stereotype", out stereotype))
                stereotype = "Content";
            var shape = ContentShape(stereotype, displayType, editor,prefix);
            shape.ContentItem = content.ContentItem;
            return shape;
        }
        public dynamic ContentShape(String stereotype, string displayType, bool editor = false, string prefix=null) {
            var actualDisplayType = string.IsNullOrWhiteSpace(displayType) ? "Detail" : displayType;
            var actualShapeType = stereotype;
            if (editor) actualShapeType += "_Edit";
            var itemShape = CreateItemShape(actualShapeType);
            itemShape.Metadata.DisplayType = actualDisplayType;
            return itemShape;
        }
    }
}