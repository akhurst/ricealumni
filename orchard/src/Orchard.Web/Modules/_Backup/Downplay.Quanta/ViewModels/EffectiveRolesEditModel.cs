using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Downplay.Quanta.ViewModels
{
    public class EffectiveRolesEditModel
    {
        public IEnumerable<string> SelectRoles { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> AllRoles { get; set; }
    }
}