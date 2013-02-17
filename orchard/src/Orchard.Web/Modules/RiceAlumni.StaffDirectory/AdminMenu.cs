using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Navigation;

namespace RiceAlumni.StaffDirectory
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
			builder.Add(T("Staff Directory"), "1", BuildMenu);
		}

		private void BuildMenu(NavigationItemBuilder menu)
		{
			menu.Add(T("Staff Profiles"), "1.1", item =>
				item.Action("List", "Admin", new { area = "Contents", id = "StaffProfile" }));

			menu.Add(T("Staff Groups"), "1.2", item =>
				item.Action("List", "Admin", new { area = "Contents", id = "StaffGroup" }));

            if (authorizer.Authorize(Orchard.Core.Contents.Permissions.EditContent))
                menu.Add(T("New Staff Profile"), "1.3", item =>
			        item.Action("Create", "Admin", new { area = "Contents", id = "StaffProfile" }));
		}
	}
}