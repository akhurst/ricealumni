using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Security.Permissions;

namespace Downplay.Theory.Identity
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageCountries = new Permission { Description = "Manage address directory countries", Name = "ManageCountries" };
        public static readonly Permission ManageRegions = new Permission { Description = "Manage address directory regions", Name = "ManageRegions" };

        public global::Orchard.Environment.Extensions.Models.Feature Feature
        {
            get;
            set; 
        }

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]{
                ManageCountries,
                ManageRegions
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {ManageCountries,ManageRegions}
                },
                new PermissionStereotype {
                    Name = "Editor",
                    Permissions = new[] {ManageCountries,ManageRegions}
                },
                new PermissionStereotype {
                    Name = "Moderator",
                    Permissions = new[] {ManageCountries,ManageRegions}
                },
                new PermissionStereotype {
                    Name = "Author",
                    Permissions = new Permission[0] 
                },
                new PermissionStereotype {
                    Name = "Contributor",
                    Permissions = new Permission[0] 
                }
            };
        }

    }
}