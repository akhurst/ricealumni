using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Alchemy.Events;
using Orchard.ContentManagement;
using Downplay.Alchemy.Factories;
using Downplay.Origami.Services;
using Orchard.Environment.Extensions;
using Downplay.Alchemy.Services;
using Orchard.DisplayManagement.Shapes;
using Orchard.DisplayManagement;
using System.Web.Mvc;

namespace Downplay.Alchemy.Providers {
    [OrchardFeature("Downplay.Alchemy.ContentDisplay")]
    public class ContentDisplayAlchemyProvider : ICompositionProvider {

        private readonly IContentManager _contentManager;
        private readonly IOrigamiService _origami;

        public ContentDisplayAlchemyProvider(
            IContentManager contentManager,
            IOrigamiService origamiService
        ) {
            _contentManager = contentManager;
            _origami = origamiService;
        }

        public void Subscribe(RootBuilder describe) {
            describe.Scope("Content", mix => {
                mix.Factory<RootShape, dynamic>((context) => _origami.ContentShape(context.Get<Model, object>() as IContent, context.Get<ShapeMetadataFactory, ShapeMetadata>().DisplayType));
                mix.Factory<ModelBuilder, ModelShapeBuilder>((context) => _origami.ContentBuilder(context.Get<Model, object>() as IContent).WithDisplayType(context.Get<ShapeMetadataFactory, ShapeMetadata>().DisplayType));
                mix.Parameter<int>("contentId", (mix2) => {
                    mix2.Factory<Model, object>((ctx) => _contentManager.Get(mix2.Get(ctx), VersionOptions.Published));
                });
            });
        }
    }
}