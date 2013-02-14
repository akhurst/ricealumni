using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.Events;
using Orchard.ContentManagement;
using System.Web.Routing;
using Orchard.Environment.Extensions;

namespace Downplay.Alchemy.Providers {
    public interface IRoutePatternProvider : IEventHandler {
        void Describe(dynamic context);
        void Routed(IContent content, String path, ICollection<Tuple<string, RouteValueDictionary>> aliases);
    }
    // TODO: Reinstate RoutePatternProvider as part of Science?
    [OrchardFeature("Downplay.Alchemy.ContentTypeRoutes")]
    public class ContentListPatternProvider : IRoutePatternProvider {

        public void Describe(dynamic context) {
            context.Pattern("ContentTypeList", "content-type(s)", "{Content.ContentType.PluralSlug}");
        }

        public void Routed(IContent content, string path, ICollection<Tuple<string, RouteValueDictionary>> aliases) {
            aliases.Add(Tuple.Create("ContentTypeList", new RouteValueDictionary(new { 
                area = "Downplay.Alchemy",
                controller = "Display",
                action = "Index",
                id="ContentList",
                ContentType = content.ContentItem.ContentType
            })));
        }
    }
}
