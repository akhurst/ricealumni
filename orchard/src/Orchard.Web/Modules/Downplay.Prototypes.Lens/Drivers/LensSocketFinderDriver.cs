using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Mechanics.Framework;
using Downplay.Origami.Services;
using Orchard;
using Orchard.Localization;
using Downplay.Prototypes.Lens.ViewModels;
using Downplay.Mechanics;
using Orchard.ContentManagement;
using Downplay.Mechanics.Services;
using Orchard.Environment.Extensions;

namespace Downplay.Prototypes.Lens.Drivers {
    [OrchardFeature("Downplay.Prototypes.Lens.SocketSearch")]
    public class LensSocketFinderDriver : ModelDriver<SocketEventContext,LensFinderViewModel>
    {
        public LensSocketFinderDriver(
            IOrchardServices services
            )
        {
            T = NullLocalizer.Instance;
            Services = services;
        }

        public Localizer T { get; set; }
        public IOrchardServices Services { get; set; }

        protected override string Prefix
        {
	        get { return "LensSocketFinderDriver"; }
        }

        protected override ModelDriverResult Build(SocketEventContext model, LensFinderViewModel viewModel, dynamic shapeHelper, ModelShapeContext context) {
            var prefix = FullPrefix(context);
            return ModelShape("Socket_Finders_LensSearch",
                () => {
                    return shapeHelper.EditorTemplate(TemplateName: "Socket.Finders.LensSearch", Model: viewModel, Prefix: prefix);
                });
        }

        protected override void Update(SocketEventContext model, LensFinderViewModel viewModel, dynamic shapeHelper, IUpdateModel updater, ModelShapeContext context) {
            var prefix = FullPrefix(context);
            if (updater != null) {
                updater.TryUpdateModel(viewModel, prefix, null, null);
                UpdatedLensViewModel(model, viewModel, prefix, updater);
            }
        }

        private void UpdatedLensViewModel(SocketEventContext model, LensFinderViewModel viewModel, string prefix, IUpdateModel updater)
        {
            // TODO: This is a pretty slow way of doing things if we have a lot of connectors.
            var currentConnectors = model.Query.ConnectorQuery.List().Select(c=>c.Id);
            if (!String.IsNullOrWhiteSpace(viewModel.SelectedItemIds))
            {
                var ids = viewModel.SelectedItemIds.Split(',').ParseInt();
                // Don't add if it's already there
                foreach (var id in ids)
                {
                    // Ensure unique if duplicates not allowed
                    // TODO: Not checking Single/Multiple - just need it checking when connectors are added
                    if (model.Connector.Settings.AllowDuplicates
                        || !currentConnectors.Contains(id))
                    {
                        // Add new connector
                        var addedId = id;
                        model.Query.Connectors.Add(id);
                    }
                }
            }
            else {
                // TODO: Logic shouldn't be here
                if (currentConnectors.Count()==0 && !model.Connector.Settings.AllowNone)
                {
                    updater.AddModelError(prefix + ".RightItemIds", T("You must select items for " + model.SocketMetadata.SocketTitle));
                }
            }
        }

        private void PopulateLensFinderModel(SocketEventContext model, LensFinderViewModel viewModel, string prefix, IUpdateModel updater) {
            var rightAllowed = model.Connector.Settings.ListAllowedContentRight();
            viewModel.ContentTypes = rightAllowed;
            viewModel.SocketName = model.Connector.Name;
        }

        protected override LensFinderViewModel ViewModel(SocketEventContext model, ModelShapeContext context) {
            var viewModel = new LensFinderViewModel();
            PopulateLensFinderModel(model, viewModel, context.Prefix, context.Updater);
            return viewModel;
        }

    }
}
