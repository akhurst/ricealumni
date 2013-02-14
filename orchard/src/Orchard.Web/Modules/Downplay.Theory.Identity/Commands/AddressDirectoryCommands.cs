using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Commands;
using Orchard.ContentManagement;
using Orchard.Security;
using Downplay.Theory.Identity.Services;
using Orchard.Core.Navigation.Services;
using Downplay.Theory.Identity.Models;
using Orchard.Core.Title.Models;

namespace Downplay.Theory.Identity.Commands
{
    public class AddressDirectoryCommands : DefaultOrchardCommandHandler
    {

        private readonly IContentManager _contentManager;
        private readonly IMembershipService _membershipService;
        private readonly IAddressDirectoryService _directoryService;
        private readonly IMenuService _menuService;

        public AddressDirectoryCommands(
            IContentManager contentManager,
            IMembershipService membershipService,
            IAddressDirectoryService blogService,
            IMenuService menuService) {
            _contentManager = contentManager;
            _membershipService = membershipService;
            _directoryService = blogService;
            _menuService = menuService;
        }

        [CommandName("directory create country")]
        [CommandHelp("directory create country <country-name> <country-code> \r\n\t" + "Create a new Country record")]
        public string CreateCountry(string countryName, string countryCode)
        {
            var country = _directoryService.CreateCountry(countryName, countryCode);
            return "Created country ID "+country.Id;
        }

        [CommandName("directory list countries")]
        [CommandHelp("directory list countries  \r\n\t" + "List all Country records")]
        public void ListCountries()
        {
            var countries = _directoryService.GetCountries();
            foreach (var c in countries)
            {
                Context.Output.WriteLine(c.As<TitlePart>().Title); // + " ("+c.CountryCode+")");
            }
            Context.Output.WriteLine(countries.Count() + " countries");
        }

        [CommandName("directory list regions")]
        [CommandHelp("directory list regions <country-name>")]
        public void ListRegions(string countryName="")
        {
            IEnumerable<IContent> regions;
            if (String.IsNullOrWhiteSpace(countryName))
            {
                Context.Output.WriteLine("Listing all regions");
                regions = _directoryService.GetTowns();
            }
            else {
                var country = _directoryService.GetCountryByName(countryName);
                Context.Output.WriteLine("Listing regions in " + country.As<TitlePart>().Title);
                regions = _directoryService.GetTowns(country.Id);
            }
            foreach (var r in regions)
            {
                Context.Output.WriteLine(r.As<TitlePart>().Title);
            }
            Context.Output.WriteLine(regions.Count() + " regions");
        }
        [CommandName("directory create region")]
        [CommandHelp("directory create region <country-name> <region-name> \r\n\t" + "Create a new Region record in the specified Country")]
        public string CreateRegion(string countryName, string regionName)
        {
            var country = _directoryService.GetCountryByName(countryName);
            if (country == null)
            {
                return "Country not found";
            }

            var region = _directoryService.CreateTown(country.Id, regionName);
            return "Created region in country " + country.As<TitlePart>().Title;
        }
    }
}