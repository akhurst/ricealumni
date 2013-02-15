using Orchard.Data.Migration;
using Orchard.Roles.Services;
using RiceAlumni.Core.Extensions;

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
		    roleService.CreateCoreRolesAndPermissions();
			return 1;
		}
	}
}