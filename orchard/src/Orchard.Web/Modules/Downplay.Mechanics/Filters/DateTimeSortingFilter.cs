using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Framework;
using Orchard.Core.Common.Models;

namespace Downplay.Mechanics.Filters {

    /// <summary>
    /// For blogs, feeds, forums etc.
    /// </summary>
    public class DateTimeSortingFilter : ISocketFilter {

        public Orchard.ContentManagement.IContentQuery Apply(Orchard.ContentManagement.IContentQuery query) {
            return query.ForPart<CommonPart>().OrderByDescending<CommonPartRecord>(c => c.CreatedUtc.Value);
        }
    }
}