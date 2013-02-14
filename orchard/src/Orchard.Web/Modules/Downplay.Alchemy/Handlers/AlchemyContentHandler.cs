using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.Environment.Extensions;
using Orchard.ContentManagement.Handlers;
using Orchard.Core.Common.Models;

namespace Downplay.Alchemy.Handlers {
    [OrchardFeature("Downplay.Alchemy.ContentDisplay")]
    public class AlchemyContentHandler : ContentHandler {

        public AlchemyContentHandler() {
            // TODO: How can we ensure this runs after other routers, without resorting to Priority or dependencies?
            //    ...How do we control which items get the new route values?
            //    ...Do we allow Alchemy providers to manipulate the routes in a more ordered fashion?
            //    ...Maybe Alchemy shouldn't *require* the controller, at least not for content display, since we're already highjacking IContentDisplay... Then push-to-layout can work universally...
            OnGetContentItemMetadata<CommonPart>((context,part)=>{
                context.Metadata.DisplayRouteValues = new System.Web.Routing.RouteValueDictionary(
                    new {
                        area = "Downplay.Alchemy",
                        controller = "Display",
                        action = "Index",
                        id = "Content",
                        contentId = part.ContentItem.Id
                    });
            });
            
        }

    }
}
