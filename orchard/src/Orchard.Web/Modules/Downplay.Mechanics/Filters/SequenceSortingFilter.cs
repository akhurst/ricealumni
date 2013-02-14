using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Mechanics.Framework;
using Downplay.Mechanics.Models;

namespace Downplay.Mechanics.Filters {
    public class SequenceSortingFilter : ISocketFilter
    {
        public Orchard.ContentManagement.IContentQuery Apply(Orchard.ContentManagement.IContentQuery query)
        {
            return query.ForPart<SequencePart>().Join<SequencePartRecord>().OrderBy(s => s.Sequence);
        }
    }
}
