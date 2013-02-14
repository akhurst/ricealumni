using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Alchemy.Factories {
    public class PagerData : IContextTag<PagerData> {

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

    }
}
