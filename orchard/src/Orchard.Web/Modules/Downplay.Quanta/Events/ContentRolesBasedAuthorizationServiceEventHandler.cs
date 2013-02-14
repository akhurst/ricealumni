using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Security;
using Orchard.ContentManagement;
using Downplay.Quanta.Models;
using Orchard.Security.Permissions;
using Orchard.Roles.Services;
using Orchard.Environment.Extensions;
using Downplay.Mechanics.Services;
using Downplay.Mechanics.Models;

namespace Downplay.Quanta.Events
{
    [OrchardFeature("Downplay.Quanta.Effectors")]
    public class ContentRolesBasedAuthorizationServiceEventHandler : IAuthorizationServiceEventHandler
    {
        private readonly IMechanicsService _relationsService;
        private readonly IRoleService _roleService;
        public ContentRolesBasedAuthorizationServiceEventHandler(IMechanicsService relationsService, IRoleService roleService)
        {
            _relationsService = relationsService;
            _roleService = roleService;
        }

        public void Checking(CheckAccessContext context)
        {
            // TODO: We could actually implement *denied* permissions. Maybe in Adjust(...)
            if (context.Granted) return;

            // TODO: Do we need to move to Adjusted so we can check per-type permissions? I general they're irrelevant since here we check against a specific content *item*.

            // TODO: Check this works with versioning. Will the authoritative item still get security check?
            // NOTE: I had to repeat a lot of code from RolesBasedAuthorizationService. Not brilliantly neat but I couldn't see any other way.

            // TODO: There's a lot of querying going on here, especially when groups are involved. Need indexing on left/right Ids *definitely* as well as other
            // optimisation / caching methods.

            // Check we have user and content
            // TODO: Could have EffectiveRolesPart on group itself; this would mean all users get that role (kind of dangerous tho!)
            if (context.User==null) return;
            if (context.Content == null) return;

            var userItem = context.User.ContentItem.As<SocketsPart>();
            if (userItem == null) return;
            var contentSockets = context.Content.As<SocketsPart>();
            if (contentSockets == null) return;
             
            // Find relationships between content and the User type
            var perms = contentSockets.Sockets["ContentToEffectingUser"].Connectors.List();
            if (perms.Count() == 0) return;
            
            // Find roles effected by this relationship
            IEnumerable<String> rolesToExamine = perms.SelectMany(p=>p.As<EffectiveRolesPart>().ListEffectiveRoles());

            // Find groups from content
            var groups = contentSockets.Sockets["GroupContentToGroup"].Connectors.List();
            var userGroups = userItem.Sockets["UserToGroupMembership"].Connectors.List();
            // Find any links to user
            foreach (var g in groups)
            {
                var userInGroup = userGroups.Where(u=>u.RightContentItemId == g.RightContentItemId);
                if (userInGroup.Any())
                {
                    // Firstly, any group roles the user has are applied
                    rolesToExamine = rolesToExamine.Union(userInGroup.SelectMany(ug=>ug.As<EffectiveRolesPart>().ListEffectiveRoles()));
                    // And apply any roles the whole group has
                    rolesToExamine = rolesToExamine.Union(g.InverseConnector.As<EffectiveRolesPart>().ListEffectiveRoles());
                }
            }

            // Determine which set of permissions would satisfy the access check
            var grantingNames = PermissionNames(context.Permission, Enumerable.Empty<string>()).Distinct().ToArray();

            foreach (var role in rolesToExamine)
            {
                foreach (var permissionName in _roleService.GetPermissionsForRoleByName(role))
                {
                    string possessedName = permissionName;
                    if (grantingNames.Any(grantingName => String.Equals(possessedName, grantingName, StringComparison.OrdinalIgnoreCase)))
                    {
                        context.Granted = true;
                    }

                    if (context.Granted)
                        break;
                }
                // End as soon as it's granted
                if (context.Granted)
                    break;
            }

        }

        /// <summary>
        /// Copied in full from RolesBasedAuthorizationService
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="stack"></param>
        /// <returns></returns>
        private static IEnumerable<string> PermissionNames(Permission permission, IEnumerable<string> stack)
        {
            // the given name is tested
            yield return permission.Name;

            // iterate implied permissions to grant, it present
            if (permission.ImpliedBy != null && permission.ImpliedBy.Any())
            {
                foreach (var impliedBy in permission.ImpliedBy)
                {
                    // avoid potential recursion
                    if (stack.Contains(impliedBy.Name))
                        continue;

                    // otherwise accumulate the implied permission names recursively
                    foreach (var impliedName in PermissionNames(impliedBy, stack.Concat(new[] { permission.Name })))
                    {
                        yield return impliedName;
                    }
                }
            }

            yield return StandardPermissions.SiteOwner.Name;
        }

        public void Adjust(CheckAccessContext context)
        {
            // NOTE: I'd like to do it here and feed a list of Roles back into the CheckAccessContext. But the only way I could do that would be to stuff them into the User object and I'm
            // worried that could affect checks elsewhere if the User object is reused.
        }

        public void Complete(CheckAccessContext context)
        {
        }
    }
}