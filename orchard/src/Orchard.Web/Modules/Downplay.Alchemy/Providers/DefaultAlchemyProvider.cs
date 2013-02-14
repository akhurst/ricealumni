using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Alchemy.Events;
using Downplay.Alchemy.Services;
using Orchard.DisplayManagement.Shapes;
using Downplay.Origami.Services;
using Downplay.Alchemy.Factories;
using System.Web.Mvc;
using Orchard.Mvc;
using Downplay.Origami;

namespace Downplay.Alchemy.Providers {
    /// <summary>
    /// Default provider for some factories
    /// </summary>
    public class DefaultAlchemyProvider : ICompositionProvider {
        private readonly IOrigamiService _origami;

        public DefaultAlchemyProvider(
                        IOrigamiService origamiService
            ) {
            _origami = origamiService;
        }

        public void Subscribe(RootBuilder describe) {
            describe.Global(global => {
                global.Factory<RootShapeBuilder, dynamic>(context => {
                    var model = context.Get<Model, object>();
                    var root = context.Get<RootShape, dynamic>();
                    if (model != null) {
                        var builder = context.Get<ModelBuilder, ModelShapeBuilder>();
                        _origami.Build(builder, root);
                    }
                    return root;
                });
                global.Factory<ActionResult>((context) => {
                    dynamic root = context.Get<RootShapeBuilder, object>();
                    var controller = context.Single<ControllerBase>();
                    dynamic display = context.Get<DisplayShape, object>();
                    return context.Get<ActionResultBuilder, Func<ControllerBase, object, ActionResult>>()(controller, display);
                });

                global.Factory<ActionResultBuilder, Func<ControllerBase, object, ActionResult>>((context) => (c, o) => {
                    return new ShapeResult(c, o);
                });

                global.Factory<ShapeMetadataFactory, ShapeMetadata>((context) => new ShapeMetadata() {
                    DisplayType = "Detail",
                });
                global.Factory<DisplayShape, dynamic>((context) => context.Get<RootShape, dynamic>());
                global.Factory<ModelBuilder, ModelShapeBuilder>((context) => _origami.Builder(context.Get<Model, object>()).WithDisplayType(context.Get<ShapeMetadataFactory, ShapeMetadata>().DisplayType).WithMode("Display"));
            });
            describe.Scope("Ajax", ajax => {
                ajax.Factory<ActionResultBuilder, Func<ControllerBase, object, ActionResult>>((context2) => (c, o) => {
                    return new ShapePartialResult(c, o);
                });
            });
        }
    }
}
