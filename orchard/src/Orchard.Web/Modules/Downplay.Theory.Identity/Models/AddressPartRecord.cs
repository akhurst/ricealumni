using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Records;

namespace Downplay.Theory.Identity.Models
{
    public class AddressPartRecord : ContentPartRecord
    {

        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string Address3 { get; set; }
        public virtual string AddressType { get; set; }
        public virtual string PostalCode { get; set; }

    }
}