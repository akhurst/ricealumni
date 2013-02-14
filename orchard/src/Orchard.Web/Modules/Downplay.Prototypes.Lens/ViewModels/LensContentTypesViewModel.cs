using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Prototypes.Lens.ViewModels
{
    public class LensContentTypesViewModel
    {

        /// <summary>
        /// Available content types for this search
        /// TODO: Need to be able to limit this, e.g. for Mechanics or Media searches
        /// </summary>
        public IEnumerable<string> ContentTypes { get; set; }

    }
}
