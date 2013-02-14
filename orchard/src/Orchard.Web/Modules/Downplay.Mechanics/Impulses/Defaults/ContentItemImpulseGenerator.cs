using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Impulses.Services;
using Orchard.Core.Contents;
using Orchard.Environment.Extensions;
using Orchard.ContentManagement;
using Downplay.Mechanics.Models;
using Orchard.Localization;

namespace Downplay.Mechanics.Impulses.Defaults
{
    /// <summary>
    /// Generates standard impulses for content items
    /// TODO: We need a proper way to switch them off e.g. placement
    /// </summary>
    [OrchardFeature("Downplay.Mechanics.Impulses")]
    public class ContentItemImpulseGenerator : IImpulseProvider
    {
        public Localizer T { get; set; }

        public ContentItemImpulseGenerator() {
            T = NullLocalizer.Instance;
        }

        public void Describing(ImpulseDescribeContext context) {
            context.Impulse("EditItem", T("Edit"), T("Edit this item")).ForPermissions(Orchard.Core.Contents.Permissions.EditContent).OnDisplaying((displaying) => {
                
                var metadata = displaying.Content.ContentItem.ContentManager.GetItemMetadata(displaying.Content);
                var routeValues = metadata.EditorRouteValues;

                // Return URL for detail view (i.e. front end)
                /*
                if (displaying.DisplayType == "Detail") {
                    var IncludeReturnUrl = true;
                }
                if (IncludeReturnUrl)
                {
                    metadata.EditorRouteValues["returnUrl"] = html.ViewContext.HttpContext.Request.RawUrl;
                }
                 */
                displaying.HrefRoute = routeValues;
            });

            context.Impulse("DeleteConnector", T("X"), T("Remove this relationship"))
                .ForPermissions(Orchard.Core.Contents.Permissions.DeleteContent)
                .ForPart<ConnectorPart>(null,(a,c)=> {
                    c.LeftContent.Sockets[c.ContentItem.ContentType].Connectors.Remove(c);
                });

        }

        public void Described(ImpulseDescribeContext context) {
        }
    }
}