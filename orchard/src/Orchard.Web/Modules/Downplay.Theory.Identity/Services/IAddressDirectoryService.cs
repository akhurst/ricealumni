using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;
using Downplay.Theory.Identity.Models;
using Orchard.Collections;
using Orchard.ContentManagement;

namespace Downplay.Theory.Identity.Services
{
    public interface IAddressDirectoryService : IDependency
    {
        IEnumerable<IContent> GetCountries();
        IContent CreateCountry(string countryName, string countryCode);
        IContent GetCountry(int countryId);
        IContent GetCountryByName(string countryName);
        IContent GetOrCreateCountryByName(string countryName);

        IEnumerable<IContent> GetTowns();
        IContent CreateTown(int countryId, string townName);
        IContent GetOrCreateTownByName(int countryId, string townName);
        IContent GetTown(int regionId);
        IEnumerable<IContent> GetTowns(int countryId);

        /*
        IContent GetRegion(int countryId, int regionId);
        IEnumerable<AddressPart> GetItemsByCountryRegion(int countryId, int regionId);
        */
        IEnumerable<AddressPart> GetItemsByPostalCodeSearch(string partialPostCode);
        IPageOfItems<IContent> GetItemsByPostalCodeSearch(string q, int pageNumber, int? pageSize);

        bool LinkAddressToTown(AddressPart part, int townId);

        TownPart GetTownForAddress(AddressPart part);
        CountryPart GetCountryForTown(IContent town);

        IContent CreateAddress(ViewModels.AddressEditViewModel viewModel, IUpdateModel updater, string prefix);
        bool PopulateAddressFromViewModel(AddressPart part, ViewModels.AddressEditViewModel viewModel, IUpdateModel updater, string prefix);
    }
}
