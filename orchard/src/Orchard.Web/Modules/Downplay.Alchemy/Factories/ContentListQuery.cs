using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement;

namespace Downplay.Alchemy.Factories {
    public class ContentListQuery : IContextTag<ContentListQuery> {

        public IContentQuery<ContentItem> Query { get; set; }

    }
}
