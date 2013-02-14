using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.UI.Navigation;
using Orchard.Localization;

namespace Downplay.Mechanics
{
    public class AdminMenu : INavigationProvider
    {
        public string MenuName
        {
            get { return "admin"; }
        }

        public Localizer T { get; set; }

        public AdminMenu() {
            T = NullLocalizer.Instance;
        }

        public void GetNavigation(NavigationBuilder builder)
        {
            // Connections menu for Graph viewing (not implemented)
         /*   builder.Add(T("Connections"), "5", menu => menu
                .Action("Connections"));
           */ 
        }
    }
}