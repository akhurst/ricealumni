using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement;

namespace Downplay.Prototypes.Lens.ViewModels
{
    public class LensResultsViewModel
    {

        public IEnumerable<IContent> Results { get; set; }

    }
}
