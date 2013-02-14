using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.Environment.Extensions;
using Downplay.Alchemy.Events;
using Downplay.Alchemy.Factories;
using Orchard.DisplayManagement.Shapes;
using Orchard.ContentManagement;
using Downplay.Origami.Services;
using Orchard.DisplayManagement;
using Downplay.Mechanics.Models;
using Downplay.Mechanics.Framework;

namespace Downplay.Prototypes.Lens.Providers {
    [OrchardFeature("Downplay.Prototypes.Lens.SocketSearch")]
    public class LensSocketCompositionProvider : ICompositionProvider {

        private IOrigamiService _origami;

        public LensSocketCompositionProvider(
            IShapeFactory shapeFactory,
            IOrigamiService origami
            ) {
            Shape = shapeFactory;
            _origami = origami;
        }

        public dynamic Shape { get; set; }

        public void Subscribe(RootBuilder describe) {
            describe.Scope("Search.Ajax", compose => {
                // Following mutate should be in LensCompositionProvider?
                compose.Mutate<ContentList, ParadigmsContext>((p, context) => p.Add("Lens"));
                compose.Parameter<String>("context", param => {
                    // Context comes from a data attribute on the lens element
                    param.Mutate<ContentList, ParadigmsContext>((p,context) => {
                                switch (param.Get(context)) {
                                    case "Socket":
                                        p.Add("LensSocket");
                                        break;
                                    case "Search":
                                        p.Add("LensSearch");
                                        break;
                                }
                            });
                    });
                compose.Parameter<string>("contentTypes", param => {
                    param.Mutate<ContentListQuery, ContentListQuery>((q,context) => {
                        var types = param.Get(context).Split(',');
                        q.Query = q.Query.ForType(types);
                    });
                });
            });
            /*
            describe.Scope("Mechanics.Connector", compose => {
                compose.Chain("Content.Edit");
                compose.Chain("Ajax");
                compose.Factory<Model, object>((context) => {
                    var model = new ConnectorDisplayContext(
                });
                compose.Factory<Model, SocketsPart>((context) => context.Get<Model,IContent>().As<SocketsPart>());
                compose.Factory<RootShape, dynamic>((context) => {
                    var content = context.Get<Model,IContent>();
                    var socket = content.As<SocketsPart>();

                    return Shape.Connector_Edit(
                        ContentItem: c.SocketContext.Left.ContentItem,
                        RightContentItem: c.Right.Content,
                        ConnectorType: c.Descriptor.Name,
                        RightContentType: c.Right.ContentType,
                        LeftContentType: c.SocketContext.Left.ContentType);
                compose.Parameter<string>("socketName", leftId => {

                });
                compose.Parameter<int>("newRightId", rightId => {
                    compose.Factory<Model,IConnector>
                    var socket = context.Get<Model,SocketsPart>();

                });
            });*/
        }
    }
}
