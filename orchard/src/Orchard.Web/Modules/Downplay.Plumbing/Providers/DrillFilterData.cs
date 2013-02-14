using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Plumbing.Providers
{
    public class DrillFilterData
    {
        public int? Id { get; set; }
        public string DrillType { get; set; }
        public string DisplayType { get; set; }
        public IEnumerable<DrillFilterData> ChildFilters { get; set; }

    }
}
