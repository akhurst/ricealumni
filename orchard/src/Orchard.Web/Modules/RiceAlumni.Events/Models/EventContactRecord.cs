using Orchard.ContentManagement.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiceAlumni.Events.Models
{
    public class EventContactRecord : ContentPartRecord
    {
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual long Phone { get; set; }
    }
}
