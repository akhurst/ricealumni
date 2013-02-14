using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Downplay.Mechanics.ViewModels
{
    public class ComboChoiceListFinderModel : MultipleChoiceListFinderModel
    {
        public string ComboText { get; set; }
        public string ComboAddNew { get; set; }
    }
}