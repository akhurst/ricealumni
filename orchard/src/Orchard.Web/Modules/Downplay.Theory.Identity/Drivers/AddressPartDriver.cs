using System.Linq;
using Orchard;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement;
using Orchard.UI.Resources;
using System.Web.Mvc;
using System;
using Downplay.Theory.Identity.Models;
using Downplay.Theory.Identity.Services;
using Downplay.Theory.Identity.ViewModels;
using Downplay.Origami.Services;
using Orchard.Localization;
using Downplay.Mechanics.Services;
using Orchard.ContentManagement.Aspects;

namespace Downplay.Theory.Identity.Drivers {
    public class AddressPartDriver : ContentPartDriver<AddressPart> {
        private readonly IAddressDirectoryService _directoryService;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IOrigamiService _origami;

        public AddressPartDriver(IAddressDirectoryService directoryService, IWorkContextAccessor workContextAccessor,IOrigamiService origami)
        {
            _workContextAccessor = workContextAccessor;
            _directoryService = directoryService;
            _origami = origami;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get;set;} 
        
        protected override DriverResult Display(AddressPart part, string displayType, dynamic shapeHelper) {
            /*
            // Populate country and region name
            // TODO: URLs for both
            var country = _directoryService.GetCountry(part.CountryId);
            if (country != null)
            {
                part.CountryName = country.Title;
            }
            var region = _directoryService.GetTown(part.RegionId);
            if (region != null)
            {
                part.RegionName = region.Title;
            }
            */
            return ContentShape("Parts_Address",
                () => shapeHelper.Parts_Address(
                    TemplateName: "Parts.Address",
                    ContentPart: part, 
                    Prefix: Prefix));
        }

        //GET
        protected override DriverResult Editor(AddressPart part, dynamic shapeHelper) {
            var viewModel = BuildViewModel(part);



           return ContentShape("Parts_Address_Edit",
                    () => shapeHelper.EditorTemplate(
                        TemplateName: "Parts.Address.Edit",
                        Model: viewModel,
                        Prefix: Prefix));
        }

        //POST
        protected override DriverResult Editor(
            AddressPart part, IUpdateModel updater, dynamic shapeHelper) {

            var viewModel = BuildViewModel(part);

            if (updater.TryUpdateModel(viewModel, Prefix, null, null))
            {
                if (_directoryService.PopulateAddressFromViewModel(part, viewModel, updater, Prefix))
                {
                    if (!_directoryService.LinkAddressToTown(part, viewModel.TownId.Value))
                    {
                        updater.AddModelError(Prefix + ".TownName", T("Error saving town. Enter a valid town name or select from the list."));
                    }
                }
            }
            else
            {
                // TODO: Notify (and other errors, validation, etc.
            }
            return Editor(part, shapeHelper);
        }

        protected AddressEditViewModel BuildViewModel(AddressPart part)
        {
            var viewModel = new AddressEditViewModel()
            {
                Address1 = part.Address1,
                Address2 = part.Address2,
                Address3 = part.Address3,
                AddressType = part.AddressType,
                PostalCode = part.PostalCode
            };
            viewModel.Countries = _directoryService.GetCountries().Select(c => new SelectListItem() { Text = c.GetTitle(), Value = c.Id.ToString() }).ToList();
            // TODO: Seriously AJAX this
            viewModel.Towns = _directoryService.GetTowns().Select(c => new SelectListItem() { Text = c.GetTitle(), Value = c.Id.ToString() }).ToList();
           var town = _directoryService.GetTownForAddress(part);
            if (town!=null) {
                viewModel.TownId = town.Id;
                viewModel.TownName = town.As<ITitleAspect>().Title;
                    var country =_directoryService.GetCountryForTown(town);
                    if (country != null)
                    {
                        viewModel.CountryId = country.Id;
                        viewModel.CountryName = country.As<ITitleAspect>().Title;
                    }
            }
            return viewModel;
        }

    }
}