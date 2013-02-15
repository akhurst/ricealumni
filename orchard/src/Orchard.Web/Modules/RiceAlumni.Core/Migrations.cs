using Orchard.Data.Migration;
using Orchard.Roles.Services;

namespace RiceAlumni.Core
{
	public class Migrations : DataMigrationImpl
	{
	    private IRoleService roleService;

	    public Migrations(IRoleService roleService)
        {
            this.roleService = roleService;
        }

		public int Create()
		{
            roleService.CreateRole("Staff");
            roleService.CreatePermissionForRole("Staff", "ViewContent");
            roleService.CreatePermissionForRole("Staff", "AccessAdminPanel");
            roleService.CreatePermissionForRole("Staff", "AccessFrontEnd");
            roleService.CreatePermissionForRole("Staff", "GrantOwnPermission");
            roleService.CreatePermissionForRole("Staff", "View_Page");
            roleService.CreatePermissionForRole("Staff", "ViewOwn_Page");
            roleService.CreatePermissionForRole("Staff", "View_ProjectionPage");
            roleService.CreatePermissionForRole("Staff", "ViewOwn_ProjectionPage");
            roleService.CreatePermissionForRole("Staff", "View_LongPartnerBanner");
            roleService.CreatePermissionForRole("Staff", "ViewOwn_LongPartnerBanner");
            roleService.CreatePermissionForRole("Staff", "View_SquarePartnerBanner");
            roleService.CreatePermissionForRole("Staff", "ViewOwn_SquarePartnerBanner");
            roleService.CreatePermissionForRole("Staff", "View_StaffGroup");
            roleService.CreatePermissionForRole("Staff", "ViewOwn_StaffGroup");
            roleService.CreatePermissionForRole("Staff", "EditOwn_StaffProfile");
            roleService.CreatePermissionForRole("Staff", "View_StaffProfile");
            roleService.CreatePermissionForRole("Staff", "ViewOwn_StaffProfile");

			return 1;
		}
	}
}