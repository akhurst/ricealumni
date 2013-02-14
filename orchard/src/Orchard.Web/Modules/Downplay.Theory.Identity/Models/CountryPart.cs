using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Theory.Identity.Models;
using Orchard.ContentManagement;

namespace Downplay.Theory.Identity.Models
{
    public class CountryPart : ContentPart<CountryPartRecord>
    {

        public string CountryCode { get { return Record.CountryCode; } set { Record.CountryCode = value; }  }

    }
}