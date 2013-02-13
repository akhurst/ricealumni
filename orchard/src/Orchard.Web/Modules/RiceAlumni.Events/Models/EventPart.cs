using System;
using Orchard.ContentManagement;

namespace RiceAlumni.Events.Models
{
    public class EventPart : ContentPart<EventPartRecord>
    {
        public virtual DateTime? StartDate
        {
            get { return Record.StartDate; }
            set { Record.StartDate = value; }
        }

        public virtual DateTime? EndDate
        {
            get { return Record.EndDate; }
            set { Record.EndDate = value; }
        }

        public virtual string LinkTarget
        {
            get { return Record.LinkTarget; }
            set { Record.LinkTarget = value; }
        }

        public virtual string LinkText
        {
            get { return Record.LinkText; }
            set { Record.LinkText = value; }
        }

        public virtual int PciEventId
        {
            get { return Record.PciEventId; }
            set { Record.PciEventId = value; }
        }

        public virtual string Title
        {
            get { return Record.Title; }
            set { Record.Title = value; }
        }

        public virtual string Description
        {
            get { return Record.Description; }
            set { Record.Description = value; }
        }

        public virtual LocationPartRecord Location
        {
            get { return Record.Location; }
            set { Record.Location = value; }
        }

        public virtual string ContactEmail
        {
            get { return Record.ContactEmail; }
            set { Record.ContactEmail = value; }
        }

        public virtual bool RegistrationRequired
        {
            get { return Record.RegistrationRequired; }
            set { Record.RegistrationRequired = value; }
        }
    }
}