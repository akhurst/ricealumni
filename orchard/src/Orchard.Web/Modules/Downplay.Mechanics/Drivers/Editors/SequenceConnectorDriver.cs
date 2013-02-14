using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Origami.Services;
using Downplay.Mechanics.Framework;
using Downplay.Mechanics.Models;
using Orchard.ContentManagement;

namespace Downplay.Mechanics.Drivers
{
    public class SequenceConnectorDriver : LegacyModelDriver<ConnectorEventContext>
    {
        protected override string Prefix
        {
            get { return "Sequence"; }
        }

        protected override ModelDriverResult Display(ConnectorEventContext model, dynamic shapeHelper, ModelShapeContext context)
        {
            return null;
        }

        protected override ModelDriverResult Editor(ConnectorEventContext model, dynamic shapeHelper, ModelShapeContext context)
        {
            return Update(model, shapeHelper, null, context);
        }

        protected override ModelDriverResult Update(ConnectorEventContext model, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelShapeContext context)
        {
            var prefix = FullPrefix(context);
            var part = model.ConnectorContent.As<SequencePart>();
            if (part != null)
            {
                if (updater != null && updater.TryUpdateModel(part, prefix, null, null))
                {
                    // TODO: Adjust sequence of other items?
                }
                return ModelShape("Connector_Editors_Sequence", () => shapeHelper.EditorTemplate(TemplateName: "Connector.Editors.Sequence", Model: part, Prefix: prefix));
            }
            return null;
        }
    }
}