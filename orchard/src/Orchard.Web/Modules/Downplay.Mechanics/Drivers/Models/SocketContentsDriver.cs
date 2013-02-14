using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Models;
using Downplay.Origami.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard;
using Orchard.Localization;
using Downplay.Mechanics.Services;
using Downplay.Mechanics.Framework;
using Orchard.Logging;
using Downplay.Mechanics.Drivers.Models;
using Downplay.Mechanics.ViewModels;
using ClaySharp.Implementation;
using Orchard.UI.Zones;
using Orchard.DisplayManagement.Shapes;

namespace Downplay.Mechanics.Drivers
{
    /// <summary>
    /// For each socket model, this renders appropriate connector shapes
    /// 
    /// TODO: Am somewhat in favour of ditching this driver or at least the template, perhaps handle with a ChainModelResult instead. Currently it's generating a redundant layout element
    /// that's tricky to deal with.
    /// </summary>
    public class SocketConnectorsDriver : ModelDriver<SocketDisplayContext>
    {
        private readonly Lazy<IEnumerable<IConnectorHandler>> _connectorHandlers;
        private readonly Lazy<IOrigamiService> _origami;

        public SocketConnectorsDriver(
                IOrchardServices services,
                Lazy<IOrigamiService> origami,
                Lazy<IEnumerable<IConnectorHandler>> connectorHandlers
            )
        {
            Services = services;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
            _connectorHandlers = connectorHandlers;
            _origami = origami;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }
        public IOrchardServices Services { get; set; }
 
        protected override string Prefix
        {
            get { return "Connectors"; }
        }

        private IEnumerable<dynamic> MapContentList(SocketDisplayContext model, ModelShapeContext context,
            Func<ConnectorDisplayContext, string,dynamic> rootShapeFactory,
            Action<ConnectorDisplayContext,dynamic,string> buildDisplay) {

            // HACK: Invoke filter delegate
                model.Filtering();

            // Build filters etc. starting with a delegate we'll compose
            // TODO: Need access to some filter functionality outside for Alchemy (part of Alchemy, Lens, or some other system?)
                Func<IContentQuery, IContentQuery> baseDelegate = q => q;
                baseDelegate = model.SocketFilters.Aggregate(baseDelegate, (d, f) => (q => f.Apply(d(q))));
                baseDelegate = model.SocketSorters.Aggregate(baseDelegate, (d, f) => (q => f.Apply(d(q))));
            IEnumerable<IConnector> list;
            list = (model.SocketPager==null?
                model.Query.Connectors.List(baseDelegate)
                : model.Query.Connectors.List(baseDelegate,model.SocketPager));

            var items = new List<dynamic>();
            foreach (var c in list) {
                if (c.RightContent == null) continue;

                var connectorContext = new ConnectorDisplayContext(c, String.IsNullOrWhiteSpace(model.Connector.DisplayType)?context.DisplayType:model.Connector.DisplayType,model);
                if (context.Mode == "Editor") {
                    _connectorHandlers.Value.Invoke(ch => ch.Editing(connectorContext,context), Logger);
                }
                else {
                    _connectorHandlers.Value.Invoke(ch => ch.Displaying(connectorContext, context), Logger);
                }
                if (!connectorContext.RenderConnector) continue;

                // TODO: This prefix won't work if you add multiple connectors to the same item...
                var prefix = FullPrefix(context, c.Id==0 ? ("New"+c.RightContentItemId.ToString()) : c.Id.ToString());
                var shape = rootShapeFactory.Invoke(connectorContext,prefix);
                shape.Metadata.DisplayType = connectorContext.Right.DisplayType;

                shape.Metadata.Prefix = prefix;

                if (context.Mode == "Editor") {
                    _connectorHandlers.Value.Invoke(ch => ch.Edit(connectorContext, shape, context), Logger);
                }
                else {
                    _connectorHandlers.Value.Invoke(ch => ch.Display(connectorContext, shape, context), Logger);
                }

                buildDisplay.Invoke(connectorContext, shape, prefix);

                items.Add(shape);
            };
            return items;
        }

        protected override ModelDriverResult Build(SocketDisplayContext model, dynamic shapeHelper, ModelShapeContext context) {
            var results = new List<ModelDriverResult>();
            // TODO: Pagers
            return Combined(
                ModelShape(
                    "Sockets_Contents_Connectors",()=>{
                        var items = MapContentList(model,context,
                            (c, prefix) => {
                                return shapeHelper.Connector(ConnectorType:c.Descriptor.Name,ContentItem:c.ConnectorContent.ContentItem);
                            },
                            (c,shape,prefix)=>{
                                var builder = _origami.Value.BuildDisplayShape(c, prefix, c.Right.DisplayType, context.Stereotype, model.Connector.Name, context).WithParadigms(model.Paradigms);
                                _origami.Value.Build(builder,shape);
                            });
                        var root = shapeHelper.Sockets_Contents(Contents:items);
                        return root;
                    }),
                ModelShape(
                    "Sockets_Contents_Right",()=>{
                        var items = MapContentList(model,context,
                            (c, prefix) => {
                                // Build content shape
                                // TODO: Might want to use different display types / paradigms for content shape and connector?
                                return _origami.Value.ContentShape(c.Right.Content, c.Right.DisplayType,prefix:prefix);
                            },
                            (c,shape,prefix)=>{
                                var builder = _origami.Value.BuildDisplayShape(c.Right.Content, prefix, c.Right.DisplayType,
                                    context.Stereotype, c.Right.ContentType, context)
                                    // TODO: Need mode and updater in here to work properly for editors
                                    .WithParadigms(model.Paradigms).WithParadigms(new[]{"Nested"});
                                _origami.Value.Build(builder,shape);
                            });

                        var root = shapeHelper.Sockets_Contents(Contents: items);
                        return root;
                    }),
                ModelShape(
                    "Sockets_Contents_Flat",()=>{
                        var items = MapContentList(model,context,
                            (c,prefix)=>{
                                // Build content shape
                                var shape = _origami.Value.ContentShape(c.ConnectorContent.ContentItem, c.Right.DisplayType, prefix: prefix);
                                shape.ContentConnector = c.ConnectorContent.ContentItem;
                                shape.ContentRight = c.Right.Content.ContentItem;
                                shape.ContentLeft = c.SocketContext.Left.ContentItem;
                                return shape;
                            },
                            (c,shape,prefix)=>{
                                // TODO: Need mode and updater in here to work properly for editors
                                var builder = _origami.Value.BuildDisplayShape(c.ConnectorContent,
                                        prefix, c.Right.DisplayType,
                                        context.Stereotype, model.Connector.Name, context)
                                    .WithParadigms(model.Paradigms)
                                    .WithParadigms(new[] { "Nested" });
                                _origami.Value.Build(builder,shape);
                                var builder2 = _origami.Value.BuildDisplayShape(c.Right.Content, prefix, c.Right.DisplayType,
                                        context.Stereotype, c.Right.ContentType, context)
                                    .WithParadigms(model.Paradigms)
                                    .WithParadigms(new[] { "Nested" });
                                _origami.Value.Build(builder2,shape);
                            });

                        var root = shapeHelper.Sockets_Contents(Contents: items);
                        return root;
                    }),
                ModelShape(
                    "Sockets_Contents_Edit",()=>{
                        var items = MapContentList(model, context,
                            (c,prefix) => {
                                return shapeHelper.Connector_Edit(ContentItem: c.SocketContext.Left.ContentItem, RightContentItem: c.Right.Content, ConnectorType: c.Descriptor.Name, RightContentType: c.Right.ContentType, LeftContentType: c.SocketContext.Left.ContentType);
                            },
                            (c, shape, prefix) => {
                                var builder = _origami.Value.BuildEditorShape(c, context.Updater, prefix, c.Right.DisplayType, context.Stereotype, model.Connector.Name, context).WithMode("Editor").WithParadigms(model.Paradigms);
                                _origami.Value.Build(builder, shape);
                            });

                        var root = shapeHelper.Sockets_Contents(Contents: items);
                        return root;
                    }),
                 ModelShape(
                    "Sockets_Contents_Links",()=>{
                        var items = MapContentList(model,context,
                            (c,prefix)=>{
                                return shapeHelper.Link();
                            },
                            (c,shape,prefix)=>{
                                // TODO: I *think* it's best to only populate the shape's properties here; so they can definitely be adjusted in content events first, and
                                // it prevents processing if the connector isn't displayed. On the other hand, maybe it'd be better to let connector events alter the
                                // shape directly instead ..
                                var metadata = c.Right.Metadata;
                                if (metadata.DisplayRouteValues != null) {
                                    shape.Text = metadata.DisplayText;
                                    shape.Values = metadata.DisplayRouteValues;
                                };
                            });
                        var root = shapeHelper.SeparatorList(Items:items);
                        return root;
                    })
               );
        }

        protected override void Update(SocketDisplayContext model, dynamic shapeHelper, IUpdateModel updater, ModelShapeContext context) {
        }


    }
}