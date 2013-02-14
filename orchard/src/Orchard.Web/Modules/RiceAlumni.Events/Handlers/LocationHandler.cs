using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using RiceAlumni.Events.Models;

namespace RiceAlumni.Events.Handlers
{
    public class LocationHandler : ContentHandler
    {
        public LocationHandler(IRepository<LocationPartRecord> locationPartRepository)
        {
            Filters.Add(StorageFilter.For(locationPartRepository));
        }
    }
}