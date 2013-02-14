using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Quanta.Models;
using Orchard.Data;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;

namespace Downplay.Quanta.Handlers
{
    [OrchardFeature("Downplay.Quanta.Effectors")]
    public class EffectiveRolesPartHandler : ContentHandler
    {
        public EffectiveRolesPartHandler(IRepository<EffectiveRolesPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}