using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Origami.Services;
using Downplay.Mechanics.Framework;
using Downplay.Theory.Identity.ViewModels;
using Downplay.Theory.Identity.Services;
using System.Web.Mvc;
using Downplay.Mechanics.Services;
using Orchard.Localization;
using Downplay.Mechanics.Drivers;

namespace Downplay.Theory.Identity.Drivers
{
    /// <summary>
    /// Adds an address creation shape
    /// </summary>
    public class AddressSocketDriver : LegacyModelDriver<SocketEventContext>
    {

        private readonly IAddressDirectoryService _directoryService;

        public AddressSocketDriver(IAddressDirectoryService directoryService)
        {
            _directoryService = directoryService;
            T = NullLocalizer.Instance;
        }
        public Localizer T { get; set; }
 
        protected override string Prefix
        {
            get { return "Addresses"; }
        }

        protected override ModelDriverResult Display(SocketEventContext model, dynamic shapeHelper, ModelShapeContext context)
        {
            return null;
        }

        protected override ModelDriverResult Editor(SocketEventContext model, dynamic shapeHelper, ModelShapeContext context)
        {
            return Update(model,shapeHelper,null,context);
        }

        protected override ModelDriverResult Update(SocketEventContext model, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelShapeContext context)
        {
            if (model.Connector.Name != "UserToAddress") return null;

            var prefix = FullPrefix(context);
            var viewModel = BuildCreateModel();
            if (updater != null && updater.TryUpdateModel(viewModel, prefix, null, null) && !string.IsNullOrWhiteSpace(viewModel.PostalCode))
            {
                var address = _directoryService.CreateAddress(viewModel,updater,prefix);
                if (address == null)
                {
                    updater.AddModelError(FullPrefix(context, "PostalCode"), T("Please specify a valid postcode and address"));
                }
                else
                {
                    // Create connector
                    model.Query.Connectors.Add(address.Id, "UserToAddress",null, true);
                }
            }
            return ModelShape("Socket_Creators_Address", shapeHelper.EditorTemplate(TemplateName: "Socket.Creators.Address", Prefix: prefix, Model: viewModel));
        }

        protected AddressEditViewModel BuildCreateModel()
        {
            var viewModel = new AddressEditViewModel()
            {
/*                Address1 = part.Address1,
                Address2 = part.Address2,
                Address3 = part.Address3,
                AddressType = part.AddressType,
                PostalCode = part.PostalCode*/
            };

            viewModel.Countries = _directoryService.GetCountries().Select(c => new SelectListItem() { Text = c.GetTitle(), Value = c.Id.ToString() }).ToList();
            // TODO: Seriously AJAX this
            viewModel.Towns = _directoryService.GetTowns().Select(c => new SelectListItem() { Text = c.GetTitle(), Value = c.Id.ToString() }).ToList();

            return viewModel;
        }


    }
}