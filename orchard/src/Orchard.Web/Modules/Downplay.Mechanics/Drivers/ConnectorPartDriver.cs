using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Drivers;
using Downplay.Mechanics.Models;
using Orchard.ContentManagement;
using Orchard;
using Orchard.DisplayManagement.Shapes;
using Downplay.Origami.Services;
using Downplay.Mechanics.Services;

namespace Downplay.Mechanics.Drivers
{
    /// <summary>
    /// Driver used for displaying and editing an individual Connector node
    /// </summary>
    public class ConnectorPartDriver : ContentPartDriver<ConnectorPart>
    {
        private readonly IOrigamiService _origami;
        private readonly Lazy<IMechanicsService> _mechanics;
        private readonly Lazy<IContentManager> _contentManager;
        public ConnectorPartDriver(
            IOrigamiService origami,
            Lazy<IMechanicsService> mechanics,
            Lazy<IContentManager> contentManager)
        {
            _origami = origami;
            _contentManager = contentManager;
            _mechanics = mechanics;
        }

        protected override string Prefix
        {
            get
            {
                return "Connector";
            }
        }

        protected override DriverResult Display(ConnectorPart part, string displayType, dynamic shapeHelper)
        {
            // TODO: Right now this has become irrelevant but it could be useful in some circumstances...
            return ContentShape("Parts_Connector",()=>{
                var rightItem = part.RightContent;
                if (rightItem == null)
                {
                    // TODO: Notify this error? It will be logged as-is
                    throw new Exception("Connection with invalid right item");
                }

                return _origami.Build(
                    _origami.ContentBuilder(rightItem).WithDisplayType(displayType),
                    _origami.ContentShape(rightItem,displayType));
            });
        }

        protected override DriverResult Editor(ConnectorPart part, dynamic shapeHelper)
        {
            return Editor(part, (IUpdateModel)null, shapeHelper);
        }

        protected override DriverResult Editor(ConnectorPart part, IUpdateModel updater, dynamic shapeHelper)
        {

            // TODO: Validate items are relatable
            if (updater!=null) {
                updater.TryUpdateModel(part, Prefix, null, null);
                // TODO: Could have generally more validation here but user shouldn't be manually editing the connectors
            }
            // Editor
            return ContentShape("Parts_Connector_Edit",
                 () => shapeHelper.EditorTemplate(
                     TemplateName: "Parts.Connector.Edit",
                     Model: part,
                     Prefix: Prefix));

        }
        protected override void Exporting(ConnectorPart part, Orchard.ContentManagement.Handlers.ExportContentContext context) {

            if (part.LeftContent != null) {
                var ownerIdentity = _contentManager.Value.GetItemMetadata(part.LeftContent).Identity;
                context.Element(part.PartDefinition.Name).SetAttributeValue("LeftContent", ownerIdentity.ToString());
            }
            if (part.RightContent != null) {
                var ownerIdentity = _contentManager.Value.GetItemMetadata(part.RightContent).Identity;
                context.Element(part.PartDefinition.Name).SetAttributeValue("RightContent", ownerIdentity.ToString());
            }
            if (part.InverseConnector != null) {
                var ownerIdentity = _contentManager.Value.GetItemMetadata(part.InverseConnector).Identity;
                context.Element(part.PartDefinition.Name).SetAttributeValue("InverseConnector", ownerIdentity.ToString());
            }

        }
        protected override void Importing(ConnectorPart part, Orchard.ContentManagement.Handlers.ImportContentContext context) {

            // TODO: Test this!
            var leftContent = context.Attribute(part.PartDefinition.Name, "LeftContent");
            if (leftContent != null) {
                part.LeftContent = context.GetItemFromSession(leftContent).As<SocketsPart>();
            }
            var rightContent = context.Attribute(part.PartDefinition.Name, "RightContent");
            if (rightContent != null) {
                part.RightContent = context.GetItemFromSession(rightContent).As<SocketsPart>();
            }
            var inverseConnector = context.Attribute(part.PartDefinition.Name, "InverseConnector");
            if (inverseConnector != null) {
                part.InverseConnector = context.GetItemFromSession(inverseConnector).As<ConnectorPart>();
            }

        }
    }
}