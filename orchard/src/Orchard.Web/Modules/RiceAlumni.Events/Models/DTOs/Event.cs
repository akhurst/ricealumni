using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiceAlumni.Events.Models.DTOs
{
    public class Event
    {
        public EventDetails Details { get; set; }
        public Location Location { get; set; }

        public Event(EventPartRecord eventPart, LocationPartRecord locationPart)
        {
            Details = new EventDetails(eventPart);
            Location = new Location(locationPart);
        }
    }
}