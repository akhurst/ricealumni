using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Downplay.Theory.Identity.ViewModels
{
    public class AddressEditViewModel
    {

        public string PostalCode { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string AddressType { get; set; }

        public string CountryName { get; set; }

        public int? CountryId { get; set; }

        public string TownName { get; set; }

        public int? TownId { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Countries { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Towns { get; set; }
    }
}