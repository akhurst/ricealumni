using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;

namespace Downplay.Theory.Identity.Models
{
    public class AddressPart : ContentPart<AddressPartRecord>, ITitleAspect
    {

        public string Address1
        {
            get { return Record.Address1; }
            set { Record.Address1 = value; }
        }
        public string Address2
        {
            get { return Record.Address2; }
            set { Record.Address2 = value; }
        }
        public string Address3
        {
            get { return Record.Address3; }
            set { Record.Address3 = value; }
        }
        public string AddressType
        {
            get { return Record.AddressType; }
            set { Record.AddressType = value; }
        }

        public string PostalCode { 
            get { return Record.PostalCode; } 
            set { Record.PostalCode = value; }
        }

        public string Title
        {
            get { return ShortAddress(); }
        }

        public string ShortAddress()
        {
            return AddressTypePrefix() + Address1 + ", " + PostalCode;
        }

        private string AddressTypePrefix()
        {
            return (String.IsNullOrWhiteSpace(AddressType) ? "" : AddressType + ": ");
        }
    }
}