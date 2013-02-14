using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.Core.Title.Models;
using Orchard.Environment.Extensions;
using Downplay.Mechanics.Framework;
using Downplay.Mechanics.Models;

namespace Downplay.Mechanics.Filters {
    public class TitleSortingFilter : ISocketFilter
    {
        /// <summary>
        /// TODO: Hmm... TitlePart needs to automatically populate with existing title from RightItem, that will
        /// save some existing complexity all-round
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Orchard.ContentManagement.IContentQuery Apply(Orchard.ContentManagement.IContentQuery query)
        {
            return query.ForPart<TitlePart>().Join<TitlePartRecord>().OrderBy(r => r.Title);
        }
    }
}
