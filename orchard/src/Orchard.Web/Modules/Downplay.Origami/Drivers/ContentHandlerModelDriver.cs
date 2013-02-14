using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Origami.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Themes;
using Orchard.Logging;
using System.Web.Routing;
using Orchard.DisplayManagement.Descriptors;
using Orchard.FileSystems.VirtualPath;
using Downplay.Origami.Placement;

namespace Downplay.Origami.Drivers {

    /// <summary>
    /// Hooks all normal content handlers into an Origami build of IContent
    /// </summary>
    public class ContentHandlerModelDriver : ModelDriver<IContent> {

        private readonly IShapeTableManager _shapeTableManager;
        private readonly Lazy<IEnumerable<IContentHandler>> _handlers;
        private readonly Lazy<IThemeManager> _themeService;
        private readonly RequestContext _requestContext;
        private readonly IVirtualPathProvider _virtualPathProvider;
        private readonly Lazy<IShapeTableLocator> _shapeTableLocator;
        private readonly IWorkContextAccessor _workContextAccessor;
        public ILogger Logger { get; set; }
 
        public ContentHandlerModelDriver (
            IShapeTableManager shapeTableManager,
            Lazy<IEnumerable<IContentHandler>> handlers,
            Lazy<IThemeManager> themeService,
            RequestContext requestContext,
            IVirtualPathProvider virtualPathProvider,
            Lazy<IShapeTableLocator> shapeTableLocator,
            IWorkContextAccessor workContextAccessor
            ){
            _shapeTableManager = shapeTableManager;
            _handlers = handlers;
            _themeService = themeService;
            _requestContext = requestContext;
            _virtualPathProvider = virtualPathProvider;
            _shapeTableLocator = shapeTableLocator;
            _workContextAccessor = workContextAccessor;
            Logger = NullLogger.Instance;
        }

        protected override ModelDriverResult Build(IContent model, dynamic shapeHelper, ModelShapeContext context) {
            if (context.Mode == "Editor") {
                if (context.Updater != null) {

                    var updater = context.Updater;
                    // If there's a parent prefix we can inject it now
                    if (!String.IsNullOrWhiteSpace(context.Prefix)) {
                        updater = new PrefixedUpdateModel(context.Prefix,context.Updater);
                    }
                    var workContext = _workContextAccessor.GetContext(_requestContext.HttpContext);
                    var theme = workContext.CurrentTheme;
                    var shapeTable = _shapeTableLocator.Value.Lookup(theme.Id);
                    var context2 = new UpdateContentEditorContext(context.Shape, model, context.DisplayType, updater, context.GroupId, shapeHelper, shapeTable, context);
                    BindPlacement(context2, context.DisplayType, context.Stereotype, context);
                    _handlers.Value.Invoke(handler => handler.UpdateEditor(context2), Logger);
                }
                else {
                    var context2 = new BuildContentEditorContext(context.Shape, model, context.DisplayType, context.GroupId, shapeHelper, context);
                    BindPlacement(context2, context.DisplayType, context.Stereotype, context);
                    _handlers.Value.Invoke(handler => handler.BuildEditor(context2), Logger);
                }
            }
            else {
                var context2 = new BuildContentDisplayContext(context.Shape, model, context.DisplayType, context.GroupId, shapeHelper, context);
                BindPlacement(context2, context.DisplayType, context.Stereotype, context);
                _handlers.Value.Invoke(handler => handler.BuildDisplay(context2), Logger);
            }
            return null;
        }

        protected override void Update(IContent model, dynamic dynamic, IUpdateModel iUpdateModel, ModelShapeContext context) {
        }

        protected override string Prefix {
            get { return "Content"; }
        }

        private void BindPlacement(BuildShapeContext context, string displayType, string stereotype, ModelShapeContext modelContext)
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
                        ModelContext = modelContext,                        
                        ContentType = context.ContentItem.ContentType,
                        Stereotype = stereotype,
                        DisplayType = displayType,
                        Differentiator = differentiator,
                        Path = VirtualPathUtility.AppendTrailingSlash(_virtualPathProvider.ToAppRelative(request.Path)) // get the current app-relative path, i.e. ~/my-blog/foo
                    };

                    var placement = descriptor.Placement(placementContext);
                    if (placement != null)
                    {
                        placement.Source = placementContext.Source;
                        return placement;
                    }
                }

                // Default
                return new PlacementInfo
                {
                    Location = defaultLocation,
                    Source = String.Empty
                };
            };
        }


    }
}