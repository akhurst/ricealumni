using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.Security.Permissions;
using Orchard;

namespace Downplay.Quanta
{
    public interface IDynamicPermissionProvider : IDependency
    {
        /// <summary>
        /// Permissions for creatable content items
        /// </summary>
        /// <returns></returns>
        IEnumerable<Permission> GetCreatableContentPermissionTemplates();

        /// <summary>
        /// Permissions for access to specific kinds of widget
        /// </summary>
        /// <returns></returns>
        IEnumerable<Permission> GetWidgetPermissionTemplates();
    }
}
