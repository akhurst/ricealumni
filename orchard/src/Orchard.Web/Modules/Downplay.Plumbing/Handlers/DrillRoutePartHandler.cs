using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Plumbing.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.ContentManagement;
using Orchard;
using Downplay.Mechanics.Models;

namespace Downplay.Plumbing.Handlers
{
    public class DrillRoutePartHandler : ContentHandler
    {
        public DrillRoutePartHandler(
            )
        {

            OnGetContentItemMetadata<DrillRoutePart>((context, part) => {

                // TODO: We don't even need the DrillRoutePart; just an additional setting for ConnectorPart...
                var connector = part.As<ConnectorPart>();
                if (connector==null) return;
                // Send the item to alchemy controller 
                context.Metadata.DisplayRouteValues = new System.Web.Routing.RouteValueDictionary(
                    new {
                        area = "Downplay.Alchemy",
                        controller = "Display",
                        action = "Index",
                        // Corresponds to the DisplayProvider
                        id = "Plumbing.Drill",
                        drillId = part.ContentItem.Id
                    });
            });
        }
    }
}