using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Records;
using System.ComponentModel.DataAnnotations;

namespace RiceAlumni.Events.Models
{
    public class EventPartRecord : ContentPartRecord
    {
        [Required]
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual string LinkTarget { get; set; }
        public virtual string LinkText { get; set; }
        public virtual int PciEventId { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual bool RegistrationRequired { get; set; }
        public virtual string ContactEmail { get; set; }
    }
}