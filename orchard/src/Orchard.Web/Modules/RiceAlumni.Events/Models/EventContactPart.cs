using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiceAlumni.Events.Models
{
    public class EventContactPart : ContentPart<EventContactRecord>
    {
        public virtual string Name
        {
            get { return Record.Name; }
            set { Record.Name = value; }
        }

        public virtual long Phone
        {
            get { return Record.Phone; }
            set { Record.Phone = value; }
        }

        public virtual string Email
        {
            get { return Record.Email; }
            set { Record.Email = value; }
        }
    }
}