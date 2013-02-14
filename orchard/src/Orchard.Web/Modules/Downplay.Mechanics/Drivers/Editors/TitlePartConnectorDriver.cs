using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Framework;
using Downplay.Origami.Services;
using Orchard.Environment.Extensions;
using Orchard.ContentManagement;
using Orchard.Core.Title.Models;
namespace Downplay.Mechanics.Drivers
{
    /// <summary>
    /// Surfaces the title when editing a connector (very useful for non-obvious reasons like altering menu captions)
    /// </summary>
    public class TitlePartConnectorDriver : LegacyModelDriver<ConnectorEventContext>
    {
        protected override string Prefix
        {
            get { return "TitlePart"; }
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
            var part = model.ConnectorContent.As<TitlePart>();
            if (part!=null) {
                if (updater!=null && updater.TryUpdateModel(part, prefix, null, null))
                {
                    // TODO:
                }
                return ModelShape("Connector_Editors_Title", () => shapeHelper.EditorTemplate(TemplateName: "Connector.Editors.Title", Model: part, Prefix: prefix));
            }
            return null;
        }
    }
}