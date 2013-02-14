using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Downplay.Theory.Identity.Models;
using Orchard.Data;
using Orchard.Core.Common.Models;
using Orchard;
using Orchard.ContentManagement.Aspects;

namespace Downplay.Theory.Identity.Handlers
{
    public class TownPartHandler : ContentHandler
    {

        public TownPartHandler(IRepository<TownPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }

    }
}