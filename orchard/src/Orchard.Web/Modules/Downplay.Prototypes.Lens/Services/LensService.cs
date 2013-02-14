using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.Search.Services;
using Downplay.Prototypes.Lens.ViewModels;
using Downplay.Origami.Services;
using Orchard.ContentManagement;

namespace Downplay.Prototypes.Lens.Services
{
    public class LensService : ILensService
    {
        private readonly ISearchService _searchService;
        private readonly IOrigamiService _origami;

        public LensService(
            ISearchService searchService,
            IOrigamiService origami
            )
        {
            _searchService = searchService;
            _origami = origami;
        }


        public IEnumerable<IContent> PerformSearch(LensViewModel model)
        {

//             _searchService.Query<dynamic>(model.Query, 0, 10, true, new[] { }, (hit) => { return null; });
            return Enumerable.Empty<IContent>();
        }


    }
}
