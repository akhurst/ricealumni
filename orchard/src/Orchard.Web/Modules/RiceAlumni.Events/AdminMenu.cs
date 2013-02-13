using Orchard.Localization;
using Orchard.UI.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiceAlumni.Events
{
    public class AdminMenu : INavigationProvider
    {
        public Localizer T { get; set; }

        public string MenuName
        {
            get { return "admin"; }
        }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T("Event"), "1", BuildEventsMenu);
        }

        private void BuildEventsMenu(NavigationItemBuilder menu)
        {
            menu.Add(T("Manage Events"), "1.2", item =>
                item.Url("~/Admin/Contents/List/Event?Options.OrderBy=Created"));

            menu.Add(T("New Event"), "1.2", item =>
                item.Url("~/Admin/Contents/Create/Event"));
        }
    }
}