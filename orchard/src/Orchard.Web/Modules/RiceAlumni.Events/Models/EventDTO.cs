using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiceAlumni.Events.Models
{
    public class EventDTO
    {
        public EventDTO() { }
        public EventDTO(EventPart eventPart)
            : this()
        {
            StartDate = eventPart.StartDate;
            EndDate = eventPart.EndDate;
            LinkTarget = eventPart.LinkTarget;
            LinkText = eventPart.LinkText;
            PciEventId = eventPart.PciEventId;
            Title = eventPart.Title;
            Description = eventPart.Description;
            Location = new LocationDTO(eventPart.Location);
            ContactEmail = eventPart.ContactEmail;
            RegistrationRequired = eventPart.RegistrationRequired;
        }

        public EventDTO(EventPartRecord eventPartRecord)
            : this()
        {
            StartDate = eventPartRecord.StartDate;
            EndDate = eventPartRecord.EndDate;
            LinkTarget = eventPartRecord.LinkTarget;
            LinkText = eventPartRecord.LinkText;
            PciEventId = eventPartRecord.PciEventId;
            Title = eventPartRecord.Title;
            Description = eventPartRecord.Description;
            Location = new LocationDTO(eventPartRecord.Location);
            ContactEmail = eventPartRecord.ContactEmail;
            RegistrationRequired = eventPartRecord.RegistrationRequired;
        }


        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string LinkTarget { get; set; }
        public string LinkText { get; set; }
        public int PciEventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public LocationDTO Location { get; set; }
        public string ContactEmail { get; set; }
        public bool RegistrationRequired { get; set; }

    }
}