using Orchard.Data.Migration;
using Orchard.Roles.Services;
using RiceAlumni.MainSite.Extensions;

namespace RiceAlumni.MainSite
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
		    roleService.CreateCoreRolesAndPermissions();
			return 1;
		}
	}
}