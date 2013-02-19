using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiceAlumni.Events.Models.DTOs
{
    public class EventDetails
    {
        public EventDetails() { }
        public EventDetails(EventPart eventPart)
            : this(eventPart.Record) { }

        public EventDetails(EventPartRecord eventPartRecord)
            : this()
        {
            StartDate = eventPartRecord.StartDate;
            EndDate = eventPartRecord.EndDate;
            LinkTarget = eventPartRecord.LinkTarget;
            LinkText = eventPartRecord.LinkText;
            PciEventId = eventPartRecord.PciEventId;
            Title = eventPartRecord.Title;
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
        public string ContactEmail { get; set; }
        public bool RegistrationRequired { get; set; }
        public Location Location { get; set; }
    }
}