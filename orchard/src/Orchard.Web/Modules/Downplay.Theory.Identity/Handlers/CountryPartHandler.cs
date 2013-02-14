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
    public class CountryPartHandler : ContentHandler
    {

        public CountryPartHandler(IRepository<CountryPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }

    }
}