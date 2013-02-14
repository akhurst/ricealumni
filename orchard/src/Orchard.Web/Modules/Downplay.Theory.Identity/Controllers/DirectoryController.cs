using System.Web.Mvc;
using Orchard.Localization;
using Orchard;
using Downplay.Theory.Identity.Services;
using Orchard.Themes;
using Orchard.Mvc;
using System.Linq;
using Orchard.DisplayManagement;
using Orchard.UI.Navigation;
using Orchard.Logging;
using Orchard.Settings;
using Orchard.Collections;
using Orchard.ContentManagement;
using System;
using Downplay.Theory.Identity.ViewModels;
namespace Downplay.Theory.Identity.Controllers {
    public class DirectoryController : Controller {
        private readonly IOrchardServices Services;

        private readonly IAddressDirectoryService _addressDirectoryService;
        private readonly ISiteService _siteService;

        public DirectoryController(
            IOrchardServices services,
            IAddressDirectoryService addressDirectoryService,
            ISiteService siteService,
            IShapeFactory shapeFactory)
        {
            Services = services;
            _addressDirectoryService = addressDirectoryService;
            _siteService = siteService;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
            Shape = shapeFactory;
        }

        public Localizer T { get; set; }
        public ILogger Logger { get; set; }
        dynamic Shape { get; set; }
        /*
        [Themed]
        [HttpGet]
        public ActionResult Directory(int? countryId=null, int? regionId=null)
        {
            if (countryId.HasValue) {
                var country = _addressDirectoryService.GetCountry(countryId.Value);
                if (country == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Country = country;
                // Handle regions
                if (regionId.HasValue)
                {
                    var region = _addressDirectoryService.GetTown(regionId.Value);
                    if (region == null)
                    {
                        return HttpNotFound();
                    }

                    // Shaping bits based on BlogPostController.ListByArchive
                    var items = _addressDirectoryService.GetItemsByCountryRegion(countryId.Value, regionId.Value)
                        .Select(b => Services.ContentManager.BuildDisplay(b, "Summary"));
                    var list = Shape.List();
                    list.AddRange(items);

                    dynamic viewModel = Shape.ViewModel()
                        .ContentItems(list)
                        .Country(country)
                        .Region(region);

                    return View("Items", (object)viewModel);
                }
                return View("Regions",_addressDirectoryService.GetTowns(countryId.Value));
            }
            return View("Countries",_addressDirectoryService.GetCountries());
        }
*/
        [Themed]
        [HttpGet]
        public ActionResult PostalCodeSearch(PagerParameters pagerParameters, string q = "")
        {
            // Strip spaces
            q = q.Replace(" ", "");
            Pager pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);
            IPageOfItems<IContent> searchHits = new PageOfItems<IContent>(new IContent[] { });
            try
            {
                searchHits = _addressDirectoryService.GetItemsByPostalCodeSearch(q, pager.Page, pager.PageSize);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Invalid postal code search query: " + q);
            }

            var list = Shape.List();
            foreach (var contentItem in searchHits.Select(searchHit => searchHit.ContentItem))
            {
                // ignore search results which content item has been removed or unpublished
                if (contentItem == null)
                {
                    searchHits.TotalItemCount--;
                    continue;
                }
                list.Add(Services.ContentManager.BuildDisplay(contentItem, "Summary"));
            }

            var pagerShape = Shape.Pager(pager).TotalItemCount(searchHits.TotalItemCount);

            var searchViewModel = new PostalCodeSearchViewModel
            {
                Query = q,
                TotalItemCount = searchHits.TotalItemCount,
                StartPosition = (pager.Page - 1) * pager.PageSize + 1,
                EndPosition = pager.Page * pager.PageSize > searchHits.TotalItemCount ? searchHits.TotalItemCount : pager.Page * pager.PageSize,
                ContentItems = list,
                Pager = pagerShape
            };

            //todo: deal with page requests beyond result count

            return View((object)searchViewModel);
        }

    }
}
