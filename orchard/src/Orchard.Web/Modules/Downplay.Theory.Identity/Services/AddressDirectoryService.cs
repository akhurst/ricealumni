using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Theory.Identity.Models;
using Orchard.Data;
using Orchard.ContentManagement;
using Orchard;
using Orchard.Collections;
using System.Xml.Linq;
using Downplay.Mechanics.Services;
using Orchard.Logging;
using Orchard.Localization;
using Orchard.Core.Title.Models;

namespace Downplay.Theory.Identity.Services
{
    public class AddressDirectoryService : IAddressDirectoryService
    {
        private readonly IOrchardServices _services;
        private readonly Lazy<IMechanicsService> _mechanics;

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        public const string CountriesContentType = "Country";
        public const string TownsContentType = "Town";
        public const string AddressesContentType = "Address";
        public const string PhoneNumbersContentType = "PhoneNumber";

        public AddressDirectoryService(IOrchardServices services,Lazy<IMechanicsService> mechanics)
        {
            _services = services;
            _mechanics = mechanics;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        public IEnumerable<IContent> GetCountries()
        {
            return _services.ContentManager.Query<TitlePart, TitlePartRecord>(CountriesContentType).OrderBy(t => t.Title).List();
        }

        public IContent CreateCountry(string countryName, string countryCode)
        {
            var country = _services.ContentManager.New(CountriesContentType);
            var c = country.As<CountryPart>();
            if (c!=null) {
                c.As<TitlePart>().Title = countryName;
                c.CountryCode = countryCode;
                _services.ContentManager.Create(c);
                return c;
            }
            return null;
        }

        public IContent GetCountry(int countryId)
        {
            return _services.ContentManager.Get(countryId);
        }
        
        public IContent GetCountryByName(string countryName)
        {
            return _services.ContentManager.Query<TitlePart, TitlePartRecord>(CountriesContentType).Where(t => t.Title == countryName).Slice(0, 1).FirstOrDefault();
        }

        public IContent GetOrCreateCountryByName(string countryName)
        {
            var country = GetCountryByName(countryName);
            if (country == null)
            {
                country = CreateCountry(countryName, "xx");
            }
            return country;
        }

        public IEnumerable<IContent> GetTowns()
        {
            return _services.ContentManager.Query<TitlePart,TitlePartRecord>(TownsContentType).OrderBy(t => t.Title).List();
        }

        public IEnumerable<IContent> GetTowns(int countryId)
        {
            var connectors = _mechanics.Value.Connectors(countryId, "CountryToTown");
            var towns = _mechanics.Value.RightItemsFromConnectors(connectors.List(),TownsContentType);//.Join<TitlePartRecord>().OrderBy(t=>t.Title);
            return towns;
        }

        public IContent CreateTown(int countryId, string regionName)
        {
            var c = _services.ContentManager.New<TitlePart>(TownsContentType);
            c.Title = regionName;
            _services.ContentManager.Create(c);
            // Create connector (ignoring permissions since this will be an automatic process when someone enters a postcode; rather than anything they can manually edit
            _mechanics.Value.CreateConnector(c, countryId, "TownToCountry", true);
            return c;
        }

        public IContent GetTown(int regionId)
        {
            return _services.ContentManager.Get(regionId);
        }

        public IContent GetRegionByName(int countryId, string regionName)
        {
            var connectors = _mechanics.Value.Connectors(countryId, "CountryToTown");
            var town = _mechanics.Value.RightItemsFromConnectors(connectors.List(), TownsContentType).FirstOrDefault(t=>t.As<TitlePart>().Title==regionName);
            return town;         
        }

        public IContent GetOrCreateTownByName(int countryId, string regionName)
        {
            var region = GetRegionByName(countryId, regionName);
            if (region == null)
            {
                return CreateTown(countryId, regionName);
            }
            return region;
        }

        /*
        public RegionRecord GetRegion(int countryId, int regionId)
        {
            return _regions.Get(r=>r.CountryId==countryId && r.Id == regionId);
        }
        public IEnumerable<AddressPart> GetItemsByCountryRegion(int countryId, int regionId)
        {
            var q = _services.ContentManager.Query<AddressPart,AddressPartRecord>()
                .Where(p=>p.CountryId == countryId && p.RegionId == regionId);
            return q.List();
        }
        */


        public IEnumerable<AddressPart> GetItemsByPostalCodeSearch(string postCodePart)
        {
            var q = PostalCodeSearchQuery(postCodePart);
            return q.List();
        }

        private IContentQuery<AddressPart, AddressPartRecord> PostalCodeSearchQuery(string postCodePart)
        {
            var q = _services.ContentManager.Query<AddressPart, AddressPartRecord>()
                .Where(p => p.PostalCode.StartsWith(postCodePart));

            return q;
        }

        public IPageOfItems<IContent> GetItemsByPostalCodeSearch(string postCodePart, int page, int? pageSize)
        {
            var q = PostalCodeSearchQuery(postCodePart);
            var totalCount = q.Count();
            var paged = q.Slice((page > 0 ? page - 1 : 0) * (int)pageSize, (int)pageSize);
            return new PageOfItems<IContent>(q.List())
            {
                PageNumber = page,
                PageSize = pageSize ?? totalCount,
                TotalItemCount = totalCount
            };
        }

        public bool LinkAddressToTown(AddressPart part, int townId)
        {
            IEnumerable<IContent> result = null;
            try
            {
                result = _mechanics.Value.CreateConnector(part, townId, "AddressToTown", true);
                if (result.Count()>0) return true;
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error linking address to town");
            }
            return false;
        }

        public TownPart GetTownForAddress(AddressPart part)
        {
            return _mechanics.Value.RightItemsFromConnectors(
                _mechanics.Value.Connectors(part, "AddressToTown").Slice(0, 1)
                ).FirstOrDefault().As<TownPart>();
        }

        public CountryPart GetCountryForTown(IContent town)
        {
            return _mechanics.Value.RightItemsFromConnectors(
                _mechanics.Value.Connectors(town, "TownToCountry").Slice(0, 1)
                ).FirstOrDefault().As<CountryPart>();
        }

        public IContent CreateAddress(ViewModels.AddressEditViewModel viewModel,IUpdateModel updater, string prefix)
        {
            var address = _services.ContentManager.New(AddressesContentType);
            var part = address.As<AddressPart>();
            if (part != null)
            {
                if (PopulateAddressFromViewModel(part, viewModel, updater, prefix))
                {
                    // Publish
                    _services.ContentManager.Create(address);
                    if (!LinkAddressToTown(part, viewModel.TownId.Value))
                    {
                        updater.AddModelError(prefix + ".TownName", T("Error saving town. Enter a valid town name or select from the list."));
                        _services.ContentManager.Remove(address);
                    }
                    return address;
                }
            }
            return null;
        }

        public bool PopulateAddressFromViewModel(AddressPart part, ViewModels.AddressEditViewModel viewModel, IUpdateModel updater, string prefix)
        {
            var ok = true;

            // Copy normal fields from view model
            if (String.IsNullOrWhiteSpace(viewModel.PostalCode))
            {
                updater.AddModelError(prefix + ".PostalCode", T("Please enter a post code."));
                ok = false;
            }
            else
            {
                // Strip any spaces
                part.PostalCode = viewModel.PostalCode = viewModel.PostalCode.Replace(" ", "");
            }
 
            part.AddressType = viewModel.AddressType;
            if (String.IsNullOrWhiteSpace(viewModel.Address1))
            {
                updater.AddModelError(prefix + ".Address1", T("Please enter at least the first line of the street address."));
                ok = false;
            }
            part.Address1 = viewModel.Address1;
            part.Address2 = viewModel.Address2;
            part.Address3 = viewModel.Address3;

            // Attempt to create inputted country name
            if (!String.IsNullOrWhiteSpace(viewModel.CountryName))
            {
                try
                {
                    var country = GetOrCreateCountryByName(viewModel.CountryName);
                    if (country != null)
                    {
                        viewModel.CountryId = country.Id;
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e, "Couldn't create country");
                }
            }
            // Attempt to create inputted region name
            if (!viewModel.CountryId.HasValue)
            {
                updater.AddModelError(prefix + ".CountryName", T("Error saving country. Enter a valid country name or select from the list."));
                ok = false;
            }
            if (viewModel.CountryId.HasValue && !String.IsNullOrWhiteSpace(viewModel.TownName))
            {
                var region = GetOrCreateTownByName(viewModel.CountryId.Value, viewModel.TownName);
                if (region != null)
                {
                    viewModel.TownId = region.Id;
                }
            }
            if (!viewModel.TownId.HasValue){
                updater.AddModelError(prefix + ".TownName", T("Enter a valid town name or select from the list."));
                ok = false;
            }
            return ok;
        }
    }
}