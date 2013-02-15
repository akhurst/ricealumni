using Orchard.Localization;
using Orchard.UI.Navigation;

namespace RiceAlumni.Core
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
			builder.Add(T("Pages"), "1", BuildMenu);
		}

		private void BuildMenu(NavigationItemBuilder menu)
		{
			menu.Add(T("Manage Pages"), "1.1", item =>
                item.Action("List", "Admin", new { area = "Contents", id = "Page" }));

			menu.Add(T("New Page"), "1.2", item =>
                item.Action("Create", "Admin", new { area = "Contents", id = "Page" }));
        }
	}
}
