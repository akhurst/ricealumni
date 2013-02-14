using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.UI.Navigation;
using Orchard.Localization;

namespace Downplay.Theory.Identity
{
    /*
    public class AdminMenu : INavigationProvider
    {
        public Localizer T { get; set; }
        public string MenuName
        {
            get { return "admin"; }
        }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T("Address Directory"), "3",
                menu => menu
                    .Add(T("Manage Countries"), "1.0", item => item.Action("Countries", "Admin", new { area = "Downplay.Theory.Identity" }).Permission(Permissions.ManageCountries))
                    .Add(T("Manage Regions"), "2.0", item => item.Action("Regions", "Admin", new { area = "Downplay.Theory.Identity" }).Permission(Permissions.ManageRegions)));
        }

    }
     * */
}