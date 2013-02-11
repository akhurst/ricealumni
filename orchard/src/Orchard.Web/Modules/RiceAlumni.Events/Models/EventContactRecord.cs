using Orchard.ContentManagement.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiceAlumni.Events.Models
{
    public class EventContactRecord : ContentPartRecord
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public long Phone { get; set; }
    }
}
