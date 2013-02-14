using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Records;

namespace Downplay.Theory.Identity.Models
{
    public class CountryPartRecord : ContentPartRecord
    {

        public virtual string CountryCode { get; set; }

    }
}