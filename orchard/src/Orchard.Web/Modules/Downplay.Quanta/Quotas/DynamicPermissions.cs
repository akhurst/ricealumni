using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Security.Permissions;
using Orchard.Environment.Extensions;

namespace Downplay.Quanta.Quotas
{
    /// <summary>
    /// Default additional per-content-type permissions supplied by Enhanced Permissions module
    /// </summary>
    [OrchardFeature("Downplay.Quanta.Quotas")]
    public class DynamicPermissions : IDynamicPermissionProvider
    {
        #region Content permissions
        private static readonly Permission PublishOnlyOneOfContent = new Permission { Description = "Publish only one of {0}", Name = "PublishOnlyOneOf_{0}"}; //, ImpliedBy = new[] { } };
        #endregion

        public DynamicPermissions()
        {
        }

        public IEnumerable<Permission> GetCreatableContentPermissionTemplates()
        {
            return new[]{
                PublishOnlyOneOfContent
            };
        }


        public IEnumerable<Permission> GetWidgetPermissionTemplates()
        {
            return new[]{
                PublishOnlyOneOfContent
            };
        }
    }
}