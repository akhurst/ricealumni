using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Framework;
using Downplay.Origami.Services;
using Downplay.Mechanics.ViewModels;
using Downplay.Mechanics.Services;
using System.Web.Mvc;

namespace Downplay.Mechanics.Drivers
{
    /// <summary>
    /// Batch operations added to the end of each socket
    /// </summary>
    public class ConnectorBatchOperationsDriver : LegacyModelDriver<SocketEventContext>
    {

        protected override string Prefix
        {
            get { return "ConnectorBatchOperations"; }
        }

        protected override ModelDriverResult Display(SocketEventContext model, dynamic shapeHelper, ModelShapeContext context)
        {
            return null;
        }

        protected override ModelDriverResult Editor(SocketEventContext model, dynamic shapeHelper, ModelShapeContext context)
        {
            return Update(model, shapeHelper, null, context);
        }

        protected override ModelDriverResult Update(SocketEventContext model, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelShapeContext context)
        {
            var prefix = FullPrefix(context);
            var viewModel = new ConnectorBatchOperationViewModel()
            {
                BatchCommandList = new[]{
                    new SelectListItem(){ Selected = true, Text = "(nothing)", Value = "" },
                    new SelectListItem(){ Selected = false, Text = "Remove", Value = "Delete" },
                }
            };
            if (updater!=null) {
                if (updater.TryUpdateModel(viewModel, prefix, null, null))
                {
                    if (!String.IsNullOrWhiteSpace(viewModel.BatchCommand)) {
                        context.OnUpdated(updated => {
                            switch (viewModel.BatchCommand) {
                                case "Delete":
                                    // Delete all selected
                                    foreach (var selector in model.Query.Connectors.State<ConnectorSelector>()) {
                                        model.Query.Connectors.Remove(selector.Item1);
                                    }
                                    break;
                            }
                        });
                    }
                }
            }
            return ModelShape("Socket_Creators_BatchOperation", () => shapeHelper.EditorTemplate(TemplateName: "Socket.Creators.BatchOperation", Prefix: prefix, Model: viewModel));
        }
    }
}