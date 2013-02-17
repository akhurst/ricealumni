﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Navigation;

namespace RiceAlumni.Homepage
{
	public class AdminMenu : INavigationProvider
	{
		public Localizer T { get; set; }
        private IAuthorizer authorizer;
        public AdminMenu(IAuthorizer authorizer)
        {
            this.authorizer = authorizer;
        }

		public string MenuName
		{
			get { return "admin"; }
		}

		public void GetNavigation(NavigationBuilder builder)
		{
			builder.Add(T("Homepage"), "1", BuildMenu);
		}

		private void BuildMenu(NavigationItemBuilder menu)
		{
			menu.Add(T("Manage Content"), "1.1", item =>
				item.Url("~/Admin/Widgets?layerId=9"));

            if (authorizer.Authorize(Orchard.Widgets.Permissions.ManageWidgets))
                menu.Add(T("New Slide"), "1.2", item =>
				item.Url("~/Admin/Widgets/AddWidget?layerId=9&widgetType=HomepageSlide&zone=Slides&returnUrl=%2FAdmin%2FWidgets%3FlayerId%3D9"));

            if (authorizer.Authorize(Orchard.Widgets.Permissions.ManageWidgets))
                menu.Add(T("New Link"), "1.3", item =>
				item.Url("~/Admin/Widgets/AddWidget?layerId=9&widgetType=HomepageLink&zone=Links&returnUrl=%2FAdmin%2FWidgets%3FlayerId%3D9"));
		}
	}
}
