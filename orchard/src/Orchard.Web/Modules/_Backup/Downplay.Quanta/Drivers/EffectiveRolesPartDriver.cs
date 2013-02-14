using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Quanta.Models;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Orchard.UI.Notify;
using Orchard.Localization;
using Orchard.ContentManagement;
using Orchard.Roles.Services;
using Downplay.Quanta.ViewModels;
using System.Web.Mvc;

namespace Downplay.Quanta.Drivers
{
    [OrchardFeature("Downplay.Quanta.Effectors")]
    public class EffectiveRolesPartDriver : ContentPartDriver<EffectiveRolesPart>
    {

        protected override string Prefix
        {
            get
            {
                return "Downplay.Quanta.Effectors.EffectiveRoles";
            }
        }
        private readonly IRoleService _roleService;
        private readonly INotifier _notifier;
        private const string TemplateName = "Parts.EffectiveRoles";
        public Localizer T { get; set; }

        public EffectiveRolesPartDriver(INotifier notifier, IRoleService roleService)
        {
            _notifier = notifier;
            _roleService = roleService;
            T = NullLocalizer.Instance;
        }
        protected override DriverResult Display(EffectiveRolesPart part, string displayType, dynamic shapeHelper)
        {
            var hasRoles = part.ListEffectiveRoles();
            return ContentShape("Parts_EffectiveRoles",
                () => shapeHelper.Parts_EffectiveRoles(ContentItem: part.ContentItem,Roles:hasRoles));
        }

        protected override DriverResult Editor(EffectiveRolesPart part, dynamic shapeHelper)
        {
            var model = BuildViewModel(part);
            return EditorShape(model, shapeHelper);
        }
        private DriverResult EditorShape(EffectiveRolesEditModel model, dynamic shapeHelper)
        {
            return ContentShape("Parts_EffectiveRoles_Edit",
            () => shapeHelper.EditorTemplate(TemplateName: TemplateName + ".Edit", Model: model, Prefix: Prefix));
        }
        private EffectiveRolesEditModel BuildViewModel(EffectiveRolesPart part)
        {
            var hasRoles = part.ListEffectiveRoles();
            var allRoles = _roleService.GetRoles().OrderBy(r => r.Name).Select(r => new SelectListItem() { Selected = hasRoles.Contains(r.Name), Text = r.Name, Value = r.Name });
            var model = new EffectiveRolesEditModel()
            {
                AllRoles = allRoles
            };
            return model;
        }

        protected override DriverResult Editor(EffectiveRolesPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var model = BuildViewModel(part);
            if (updater.TryUpdateModel(model, Prefix, null, null))
            {
                // TODO: Check role exists and user is allowed permission
                part.EffectiveRoles = String.Join(" ", model.SelectRoles.ToArray());
            }
            else
            {
                _notifier.Error(T("Error updating Effective Roles."));
            }
            return EditorShape(model, shapeHelper);
        }
    }
}