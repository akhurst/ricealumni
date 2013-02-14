using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.Handlers;
using Orchard;
using Downplay.Origami.Drivers;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Security;
using Orchard.Core.Contents;
using Downplay.Mechanics.Models;

namespace Downplay.Mechanics.Impulses.Handlers {

    /// <summary>
    /// Decorates Content shapes with an Id for Javascript purposes
    /// TODO: Very useful and perhaps shouldn't be specifically tied to Impulses ... maybe part of the Delta module which Impulses will depend on?
    /// </summary>
    public class ImpulsesContentHandler : ContentHandler {

        public ImpulsesContentHandler(
            IOrchardServices orchardServices
            ) {
                Services = orchardServices;
        }

        public IOrchardServices Services { get; set; }

        protected override void BuildDisplayShape(BuildDisplayContext context) {

            // TODO: Adding these attributes onto *all* items could be seen as a bad thing.
            // But realistically there are tons of situations where it'll be useful. So review this and decide (maybe only if admin?)

            if (context.Shape.ContentItem != null) {
                ContentItem content = context.Shape.ContentItem;
                if (content!=null) {
                    context.Shape.Attributes["data-content-item-id"]=content.Id.ToString();
                    if (content.VersionRecord != null) {
                        context.Shape.Attributes["data-content-item-version-id"] = content.VersionRecord.Id.ToString();
                    }
                    if (Services.Authorizer.Authorize(Permissions.EditContent, content)) {
                        context.Shape.Attributes["data-content-editable"] = "true";
                    }
                    context.Shape.Attributes["data-content-type"] = content.ContentType;
                    var connector = content.As<ConnectorPart>();
                    if (connector != null)
                    {
                        context.Shape.Attributes["data-right-content-type"] = connector.RightContent.ContentItem.ContentType;
                    }
                }
            }
        }

    }
}
