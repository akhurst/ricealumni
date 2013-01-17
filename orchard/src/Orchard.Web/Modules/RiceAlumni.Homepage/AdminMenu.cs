using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace RiceAlumni.Homepage
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
			builder.Add(T("Homepage"), "1", BuildMenu);
		}

		private void BuildMenu(NavigationItemBuilder menu)
		{
			menu.Add(T("Slides"), "1.1", item =>
				item.Action("List", "Admin", new { area = "Contents", id = "HomepageSlide" }));

			menu.Add(T("New Slide"), "1.2", item =>
				item.Url("~/Admin/Widgets/AddWidget?layerId=9&widgetType=HomepageSlide&zone=Slides"));

			menu.Add(T("Links"), "1.3", item =>
				item.Action("List", "Admin", new { area = "Contents", id = "HomepageLink" }));

			menu.Add(T("New Link"), "1.4", item =>
			item.Url("~/Admin/Widgets/AddWidget?layerId=9&widgetType=HomepageLink&zone=Links"));
		}
	}
}
