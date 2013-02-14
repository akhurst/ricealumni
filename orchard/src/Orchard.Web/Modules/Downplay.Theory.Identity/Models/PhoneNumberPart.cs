using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;

namespace Downplay.Theory.Identity.Models
{
    public class PhoneNumberPart : ContentPart<PhoneNumberPartRecord>
    {

        public string Number { get { return Record.Number; } set { Record.Number = value; } }

    }
}