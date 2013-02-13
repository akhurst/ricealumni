using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using RiceAlumni.Events.Models;

namespace RiceAlumni.Event.Handlers
{
    public class EventHandler : ContentHandler
    {
        public EventHandler(IRepository<EventPartRecord> eventPartRepository)
        {
            Filters.Add(StorageFilter.For(eventPartRepository));
        }
    }
}