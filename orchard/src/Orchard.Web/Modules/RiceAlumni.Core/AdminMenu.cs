using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Navigation;

namespace RiceAlumni.Core
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
			builder.Add(T("Pages"), "1", BuildMenu);
		}

		private void BuildMenu(NavigationItemBuilder menu)
		{

			menu.Add(T("Manage Pages"), "1.1", item =>
                item.Action("List", "Admin", new { area = "Contents", id = "Page" }));

            if(authorizer.Authorize(Orchard.Core.Contents.Permissions.EditContent))
    			menu.Add(T("New Page"), "1.2", item =>
                    item.Action("Create", "Admin", new { area = "Contents", id = "Page" }));
        }
	}
}
