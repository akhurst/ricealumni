using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Roles.Services;

namespace RiceAlumni.Core.Extensions
{
    public static class RiceRoleExtensions 
    {
        public static void CreateCoreRolesAndPermissions(this IRoleService roleService)
        {
            var author = roleService.GetRoleByName("Author");
            var moderator = roleService.GetRoleByName("Moderator");
            roleService.DeleteRole(author.Id);
            roleService.DeleteRole(moderator.Id);
            roleService.CreateRole("Staff");
            roleService.CreatePermissionForRole("Staff", "ViewContent");
            roleService.CreatePermissionForRole("Staff", "AccessAdminPanel");
            roleService.CreatePermissionForRole("Staff", "AccessFrontEnd");
            roleService.CreatePermissionForRole("Staff", "GrantOwnPermission");
            roleService.CreatePermissionForRole("Staff", "ViewContent");

            roleService.CreatePermissionForRole("Contributor", "ManageMedia");
            roleService.CreatePermissionForRole("Contributor", "EditContent");
            roleService.CreatePermissionForRole("Contributor", "ManageWidgets");
            roleService.CreatePermissionForRole("Contributor", "GrantPermission");
            roleService.CreatePermissionForRole("Contributor", "ManageFeatures");
            roleService.CreatePermissionForRole("Contributor", "AccessFrontEnd");
            roleService.CreatePermissionForRole("Contributor", "AccessAdminPanel");

            var editor = roleService.GetRoleByName("Editor");
            var editorPermissions = roleService.GetPermissionsForRoleByName("Editor").Where(s=>s!="ApplyTheme" && s!="SetHomePage").ToList();
            editorPermissions.Add("ManageMainMenu");
            editorPermissions.Add("GrantPermission");
            editorPermissions.Add("ManageWidgets");
            roleService.UpdateRole(editor.Id, editor.Name, editorPermissions);


        }

        public static void CreateStaffProfileRolesAndPermissions(this IRoleService roleService)
        {
            roleService.CreatePermissionForRole("Staff", "View_StaffGroup");
            roleService.CreatePermissionForRole("Staff", "ViewOwn_StaffGroup");
            roleService.CreatePermissionForRole("Staff", "EditOwn_StaffProfile");
            roleService.CreatePermissionForRole("Staff", "View_StaffProfile");
            roleService.CreatePermissionForRole("Staff", "ViewOwn_StaffProfile");
        }

        public static void CreateHomepageRolesAndPermissions(this IRoleService roleService)
        {
            roleService.CreatePermissionForRole("Staff", "View_LongPartnerBanner");
            roleService.CreatePermissionForRole("Staff", "ViewOwn_LongPartnerBanner");
            roleService.CreatePermissionForRole("Staff", "View_SquarePartnerBanner");
            roleService.CreatePermissionForRole("Staff", "ViewOwn_SquarePartnerBanner");
        }
    }
}