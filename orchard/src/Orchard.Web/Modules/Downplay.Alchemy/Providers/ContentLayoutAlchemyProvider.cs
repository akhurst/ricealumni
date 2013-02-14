using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Alchemy.Events;
using Downplay.Alchemy.Factories;
using Orchard;
using Orchard.Environment.Extensions;
using System.Web.Mvc;
using Orchard.ContentManagement;

namespace Downplay.Alchemy.Providers {
    /// <summary>
    /// This causes content to be rendered across the Layout shape zones instead of generating a Content shape to render across
    /// the zones. You can still push title and body to the Content zone, but it means you can push any parts to any other zones
    /// e.g. sidebar, before/after content, etc.
    /// </summary>
    [OrchardFeature("Downplay.Alchemy.LayoutDisplay")]
    public class ContentLayoutAlchemyProvider : ICompositionProvider {
        private readonly IWorkContextAccessor _workContextAccessor;

        public ContentLayoutAlchemyProvider(
            IWorkContextAccessor workContextAccessor
            ) {
            _workContextAccessor = workContextAccessor;
        }

        public void Subscribe(RootBuilder describe) {
            describe.Scope("Content",content => {
                // Root all items in layout
                content.Factory<RootShape, dynamic>((context) =>
                {
                    var layout = _workContextAccessor.GetContext().Layout;
                    var item = context.Get<Model, object>() as IContent;
                    // Save the item there so we can use correct CSS class in layout
                    layout.ContentItem = item;
                    layout.Title = item.ContentItem.ContentManager.GetItemMetadata(item.ContentItem).DisplayText;
                    return layout;
                }
                );
                // Prevent displaying anything in Content zone on layout
                content.Factory<ActionResultBuilder, Func<ControllerBase, object, ActionResult>>((context) => (c, o) => {
                    // Return Null (empty) view
                    return new ViewResult() {
                        MasterName = null,
                        ViewName = "Null",
                        ViewData = c.ViewData,
                        TempData = c.TempData
                    };
                });
            });
        }
    }
}
