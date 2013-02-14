using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.Environment.Extensions;
using Downplay.Alchemy.Events;
using Orchard.ContentManagement;
using Downplay.Origami.Services;
using Orchard.DisplayManagement;
using Downplay.Alchemy.Factories;
using Orchard.DisplayManagement.Shapes;

namespace Downplay.Alchemy.Providers {
    [OrchardFeature("Downplay.Alchemy.ContentLists")]
    public class ContentListsAlchemyProvider : ICompositionProvider {

        private readonly IContentManager _contentManager;
        private readonly IOrigamiService _origami;

        public ContentListsAlchemyProvider(
            IContentManager contentManager,
            IOrigamiService origamiService,
            IShapeFactory shapeFactory
        ) {
            _contentManager = contentManager;
            _origami = origamiService;
            Shape = shapeFactory;
        }

        public dynamic Shape { get; set; }

        public void Subscribe(RootBuilder describe) {
            describe.Scope("ContentList", mix => {
                mix.Factory<RootShape,dynamic>((context) => Shape.ContentList(ListItems:((ContentListViewModel)context.Get<Model,object>()).Rendered));
                mix.Factory<ContentList,ShapeMetadata>((context) => new ShapeMetadata() {
                    DisplayType = "Summary",
                });
                mix.Factory<ContentList,ParadigmsContext>((context) => new ParadigmsContext());
                mix.Factory<ContentList,ContentListQuery>((context) => new ContentListQuery() { Query = _contentManager.Query() });
                mix.Factory<ContentList,IEnumerable<IContent>>((context) => {
                    var query = context.Get<ContentList, ContentListQuery>();
                    var pager = context.Get<ContentList,PagerData>();
                    if (pager!=null) 
                        return query.Query.Slice(pager.PageSize*(pager.Page-1),pager.PageSize);
                    return query.Query.List();
                });
                mix.Factory<ContentModelBuilder, Func<IContent, ModelShapeBuilder>>((context) => (c) => Builder(c, context));
                // TODO: Could support prefix here
                mix.Factory<ContentShapeBuilder, Func<IContent, object>>((context) => (s) => _origami.ContentShape(s, context.Get<ContentList, ShapeMetadata>().DisplayType));
                mix.Factory<Model,object>((context) => {
                    var source = context.Get<ContentList,IEnumerable<IContent>>();
                    var builder = context.Get<ContentModelBuilder, Func<IContent,ModelShapeBuilder>>();
                    return new ContentListViewModel() {
                        Rendered = source.Select(s => {
                            var root = context.Get<ContentShapeBuilder, Func<IContent, object>>()(s);
                            _origami.Build(builder(s), root);
                            return root;
                        }).ToList()
                    };
                });
                // TODO: Get page size from settings
                mix.Factory<ContentList,PagerData>((context) => {
                    var pager = new PagerData() {
                        Page = 1,
                        PageSize = 10
                    };
                    return pager;
                });
                mix.Parameter<int>("PageSize", (pageSize) => {
                    pageSize.Mutate<ContentList, PagerData>((pager, context) => { pager.PageSize = pageSize.Get(context); });
                });
                mix.Parameter<int>("Page", (page) => {
                    page.Mutate<ContentList, PagerData>((pager, context) => { pager.Page = page.Get(context); });
                });
                mix.Parameter<string>("ContentType", (ct) => {
                    ct.Mutate<ContentList, ContentListQuery>((q,context) => q.Query = q.Query.ForType(ct.Get(context)));
                });
                mix.Parameter<string>("ContentTypes", (ct) => {
                    ct.Mutate<ContentList, ContentListQuery>((q, context) => q.Query = q.Query.ForType(ct.Get(context).Split(new[]{','},StringSplitOptions.RemoveEmptyEntries)));
                });
                // Calculate total pages at end
                mix.Mutate<ContentList, PagerData>((pager, context) => {
                    var query = context.Get<ContentList, ContentListQuery>();
                    pager.TotalPages = query.Query.Count() / pager.PageSize; // TODO: Check calculation
                });

            });
        }

        public ModelShapeBuilder Builder(IContent model,CompositionContext context) {
            var paradigms = context.Get<ContentList,ParadigmsContext>();
            var builder = _origami.ContentBuilder(model).WithDisplayType(context.Get<ContentList, ShapeMetadata>().DisplayType);
            if (paradigms != null) {
                builder.WithParadigms(paradigms);
            }
            return builder;
        }
    }

}
