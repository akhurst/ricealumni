using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement;
using System.Web.Mvc;

namespace Downplay.Mechanics.ViewModels
{
    public class ChoiceListFinderModel
    {
        public string ConnectorName { get; set; }
        public IEnumerable<SelectListItem> AvailableRightItems { get; set; }
    }
}
