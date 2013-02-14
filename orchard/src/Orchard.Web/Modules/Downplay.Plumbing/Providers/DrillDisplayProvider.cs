using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement;
using Downplay.Alchemy.Events;
using Downplay.Alchemy.Factories;
using Downplay.Mechanics.Models;
using Downplay.Plumbing.Models;
using Downplay.Plumbing.Factories;
using Downplay.Origami.Services;
using Downplay.Alchemy.Providers;
using Orchard.DisplayManagement.Shapes;

namespace Downplay.Plumbing.Providers {
    public class DrillDisplayProvider : ICompositionProvider {
        private readonly IContentManager _contentManager;
        private readonly IOrigamiService _origami;
        public DrillDisplayProvider(
            IContentManager contentManager,
            IOrigamiService origami) {
            _contentManager = contentManager;
            _origami = origami;
        }

        public void Subscribe(RootBuilder describe) {
            describe.Scope("Plumbing.Drill", drill => {
                drill.Chain("Content");
                drill
                .Parameter<int>("drillId", param => {
                    param.Factory<DrillContent>((context) => {
                        var c = _contentManager.Get<ConnectorPart>(param.Get(context));
                        if (c == null) return null;
                        return new DrillContent() {
                            Connector = c,
                            Left = c.LeftContent,
                            Right = c.RightContent
                        };
                    });

                    param.Factory<Model, object>((context) => {
                        return context.Get<DrillContent>().Left;
                    });
                    param.Factory<DrillContent, DrillFilterData>((context) => {
                        var content = context.Get<DrillContent>();
                        if (content == null) return null;
                        var drillPart = content.Connector.As<DrillRoutePart>();
                        if (drillPart == null) return null;
                        return new DrillFilterData() {
                            Id = content.Connector.Id,
                            // TODO: DisplayType is kinda redundant here; need to handle in Placement?
                            DisplayType = "Detail",
                            DrillType = content.Connector.ContentItem.TypeDefinition.Name
                        };
                    });
                    param.Factory<DrillContent, ModelShapeBuilder>(msb => {
                        return _origami.Builder(msb.Get<DrillContent>().Right).WithDisplayType(msb.Get<ShapeMetadataFactory, ShapeMetadata>().DisplayType).WithMode("Display").WithParadigms(new[]{"DrillDetail"});});

/*                    param.Mutate<ModelBuilder, ModelShapeBuilder>((msb, context) => {
                        // Push drill data into model builder context
                        msb.Context.CustomContext[typeof(DrillFilterData)] = context.Get<DrillContent, DrillFilterData>();
                        msb.WithParadigms(new[] { "DrillDetail" });
                    });*/
                    param.Mutate<RootShapeBuilder, dynamic>((root, context) => {
                        var model = context.Get<DrillContent>().Right;
                        if (model != null) {
                            var builder = context.Get<DrillContent, ModelShapeBuilder>();
                            _origami.Build(builder, root);
                        }
                    });
                });
            });
        }

    }
}
