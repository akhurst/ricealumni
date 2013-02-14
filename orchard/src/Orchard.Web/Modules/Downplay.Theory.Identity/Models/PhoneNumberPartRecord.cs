using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Records;

namespace Downplay.Theory.Identity.Models
{
    public class PhoneNumberPartRecord : ContentPartRecord
    {

        public virtual string Number { get; set; }
        public virtual string NumberType { get; set; }

    }
}