using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Origami.Services;
using Downplay.Mechanics.Framework;
using Downplay.Mechanics.ViewModels;
using Downplay.Mechanics.Services;
using Downplay.Mechanics.Models;

namespace Downplay.Mechanics.Drivers
{
    /// <summary>
    /// Adds a select box to each connector for batch operations
    /// </summary>
    public class ConnectorBatchSelectionDriver : LegacyModelDriver<ConnectorEventContext>
    {
        protected override string Prefix
        {
            get { return "ConnectorBatchSelection"; }
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
            var viewModel = new ConnectorBatchSelectionViewModel();
            if (updater!=null) {
                if (updater.TryUpdateModel(viewModel, prefix, null, null))
                {
                    if (viewModel.Selected)
                    {
                        model.SocketContext.Query.Connectors.Remove(model.ConnectorContent);
                        // (model.ConnectorContent as ConnectorPart).DeleteWhenLeftPublished = true;
                        // model.SocketContext.Query.Connectors.Attach(model.ConnectorContent,new ConnectorSelector());
                    }
                }
            }
            return ModelShape("Connector_Options_BatchSelect", () => shapeHelper.EditorTemplate(TemplateName: "Connector.Options.BatchSelect", Prefix: prefix, Model: viewModel));
        }
    }
}