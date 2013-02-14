using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Origami.Services;
using Downplay.Mechanics.Models;
using Downplay.Mechanics.ViewModels;
using Orchard.Localization;
using System.Web.Mvc;
using Orchard;
using Orchard.ContentManagement;
using Downplay.Mechanics.Services;
using Orchard.ContentManagement.Drivers;
using Downplay.Mechanics.Framework;
using Orchard.ContentManagement.Records;
using Orchard.Core.Common.Models;

namespace Downplay.Mechanics.Drivers
{

    /// <summary>
    /// Provides simple UI for single-choice dropdown and multiple-choice checkboxes
    /// </summary>
    public class ChoiceListFinderDriver : LegacyModelDriver<SocketEventContext>
    {
        private readonly IMechanicsService _mechanics;

        public ChoiceListFinderDriver(
            IOrchardServices services,
            IMechanicsService mechanics
            )
        {
            _mechanics = mechanics;
            T = NullLocalizer.Instance;
            Services = services;
        }

        public Localizer T { get; set; }
        public IOrchardServices Services { get; set; }

        protected override string Prefix
        {
	        get { return "Mechanics.ChoiceListFinder"; }
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
            return Combined(
                ModelShape("Socket_Finders_SingleChoiceList",
                    () => {
                        var prefix = FullPrefix(context,"Single");
                        var viewModel = BuildSingleChoiceModel(model,prefix,updater);
                        if (updater != null)
                        {
                            updater.TryUpdateModel(viewModel, prefix, null, null);
                            UpdatedSingleChoiceModel(model, viewModel, prefix, updater);
                            // Rebuild model
                            viewModel = BuildSingleChoiceModel(model, prefix, updater);
                        }
                        return shapeHelper.EditorTemplate(TemplateName: "Socket.Finders.SingleChoiceList", Model: viewModel, Prefix:prefix);
                    }),
                ModelShape("Socket_Finders_MultipleChoiceList",
                    () => {
                        var prefix = FullPrefix(context, "Multiple");
                        var viewModel = BuildMultipleChoiceModel(model, prefix, updater);
                        if (updater != null)
                        {
                            updater.TryUpdateModel(viewModel, prefix, null, null);
                            UpdatedMultipleChoiceModel(model, viewModel, prefix, updater);
                            // Rebuild model
                            viewModel = BuildMultipleChoiceModel(model, prefix, updater);
                        }
                        return shapeHelper.EditorTemplate(TemplateName: "Socket.Finders.MultipleChoiceList", Model: viewModel, Prefix: prefix);
                    }
                    ));
        }

        private void UpdatedMultipleChoiceModel(SocketEventContext model, MultipleChoiceListFinderModel viewModel, string prefix, IUpdateModel updater)
        {
            // Any unchecked can be removed
            model.Query.Connectors.Remove(c => !viewModel.RightItemIds.Contains(c.RightContentItemId));
            var currentConnectors = model.Query.Connectors.List();
            // Add all ids (unless duplicates are prevented)
            foreach (var id in viewModel.RightItemIds)
            {
                if (model.Connector.Settings.AllowDuplicates
                    || !currentConnectors.Any(c => c.RightContentItemId == id))
                {
                    // Add new connector
                    model.Query.Connectors.Add(id);
                }
            }
            // TODO: This logic needs to be in a validation step instead
            if (!model.Query.Connectors.Any() && !model.Connector.Settings.AllowNone)
            {
                updater.AddModelError(prefix + ".RightItemIds", T("You must select items for {0}",model.SocketMetadata.SocketTitle));
            }
            model.Query.Connectors.Flush(_mechanics);
        }

        private void UpdatedSingleChoiceModel(SocketEventContext model, SingleChoiceListFinderModel viewModel, string prefix, IUpdateModel updater)
        {
            if (viewModel.RightItemId.HasValue) {
                // Remove existing ones if only allowed one end point
                // TODO: Sure we *might* want to show the dropdown instead but really, why?
                // Don't add if it's already there
                // TODO: (Perf) possibly not efficient, but it only happens on edits
                if (model.Connector.Settings.AllowDuplicates || !model.Query.Connectors.List().Any(c => c.RightContentItemId == viewModel.RightItemId.Value)) {
                    // Add new connector
                    var addedId = viewModel.RightItemId.Value;
                    model.Query.Connectors.Add(addedId);
                }
                if (!model.Connector.Settings.AllowMany) {
                    model.Query.Connectors.Remove(c => c.RightContentItemId != viewModel.RightItemId.Value);
                }
                
            }
            else {
                if (!model.Connector.Settings.AllowMany) {
                    // Removing all connectors
                    model.Query.Connectors.Remove(c => true);
                }
            }
            // TODO: Logic shouldn't be here. Might be a problem if connector creation later fails due to permissions.
            if (!model.Connector.Settings.AllowNone && !model.Query.Connectors.Any())
            {
                updater.AddModelError(prefix + ".RightItemId", T("You must select an item for {0}",model.SocketMetadata.SocketTitle));
            }
        }

        private SingleChoiceListFinderModel BuildSingleChoiceModel(SocketEventContext model, string prefix, IUpdateModel updater)
        {
            var viewModel = new SingleChoiceListFinderModel();
            var item = model.Query.Connectors.List().FirstOrDefault();
            var ids = new int[]{};
            if (item != null && item.RightContent!=null) {
                viewModel.RightItemId = item.RightContent.ContentItem.Id;
                ids = new[] { item.RightContent.ContentItem.Id };
            }
            PopulateChoiceListModel(model,viewModel,prefix,updater,ids);

            // Add "none" option for the drop-down
            if (model.Connector.Settings.AllowNone)
            {
                viewModel.AvailableRightItems = new[] { new SelectListItem() { Text = T("(none)").Text, Value = "", Selected = !viewModel.RightItemId.HasValue } }
                   .Concat(viewModel.AvailableRightItems) ;
            }
            
            return viewModel;
        }

        private MultipleChoiceListFinderModel BuildMultipleChoiceModel(SocketEventContext model, string prefix, IUpdateModel updater)
        {
            var viewModel = new MultipleChoiceListFinderModel();
            PopulateChoiceListModel(model, viewModel, prefix, updater, null);
            return viewModel;
        }
        private void PopulateChoiceListModel(SocketEventContext model, ChoiceListFinderModel viewModel, string prefix, IUpdateModel updater, int[] selectedIds)
        {
            viewModel.ConnectorName = model.Connector.Name;

            var rightAllowed = model.Connector.Settings.ListAllowedContentRight();
            // Get all available items
            var rightItems = Services.ContentManager.Query<CommonPart,CommonPartRecord>(rightAllowed.ToArray()).ForVersion(VersionOptions.Published);
/*            foreach (var c in rightAllowed) {
                rightItems = rightItems.WithQueryHintsFor(c);
            } */
            if (selectedIds==null && !model.Connector.Settings.AllowDuplicates) {
                selectedIds = model.Query.Connectors.List().Select(c=>c.RightContentItemId).ToArray();
            }

            viewModel.AvailableRightItems = rightItems.List().Select(item =>
                new SelectListItem()
                {
                    Text = item.GetTitle() + " ("+item.TypeDefinition.DisplayName+")",
                    Value = item.ContentItem.Id.ToString(),
                    Selected = selectedIds!=null && selectedIds.Contains(item.Id)
                }).OrderBy(c=>c.Text).ToList();
        }

    }
}