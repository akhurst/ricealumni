using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Security.Permissions;
using Orchard.Environment.Extensions.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Settings;
using Orchard.ContentManagement.MetaData.Models;

namespace Downplay.Quanta
{
    /// <summary>
    /// This extends the concept of a typical DynamicPermissions file by allowing anyone to add their own forms of permission to all content
    /// </summary>
    public class DynamicPermissionsProvider : IPermissionProvider
    {

        private IContentDefinitionManager _contentDefinitionManager;
        private IEnumerable<IDynamicPermissionProvider> _dynamicProviders;

        public DynamicPermissionsProvider(IEnumerable<IDynamicPermissionProvider> dynamicProviders, IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _dynamicProviders = dynamicProviders;
        }

        public virtual Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions()
        {
            // manage rights only for Creatable types
            var creatableTypes = _contentDefinitionManager.ListTypeDefinitions()
                .Where(ctd => ctd.Settings.GetModel<ContentTypeSettings>().Creatable);

            foreach (var provider in _dynamicProviders)
            {
                foreach (var template in provider.GetCreatableContentPermissionTemplates())
                {
                    foreach (var type in creatableTypes)
                    {
                        yield return CreateDynamicPermission(template, type);
                    }
                }
            }

            var widgetTypes = _contentDefinitionManager.ListTypeDefinitions()
                .Where(ctd => ctd.Settings["Stereotype"] == "Widget");
            foreach (var provider in _dynamicProviders)
            {
                foreach (var template in provider.GetWidgetPermissionTemplates())
                {
                    foreach (var type in creatableTypes)
                    {
                        yield return CreateDynamicPermission(template, type);
                    }
                }
            }

        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return Enumerable.Empty<PermissionStereotype>();
        }

        /// <summary>
        /// Generates a permission dynamically for a content type
        /// </summary>
        public static Permission CreateDynamicPermission(Permission template, ContentTypeDefinition typeDefinition)
        {
            return new Permission
            {
                Name = String.Format(template.Name, typeDefinition.Name),
                Description = String.Format(template.Description, typeDefinition.DisplayName),
                Category = typeDefinition.DisplayName,
                ImpliedBy = (template.ImpliedBy ?? new Permission[0]).Select(t => CreateDynamicPermission(t, typeDefinition))
            };
        }


    }
}