using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Framework;
using Downplay.Origami.Services;
using Downplay.Quanta.Models;
using Downplay.Quanta.ViewModels;
using Orchard.ContentManagement;
using Orchard.UI.Notify;
using Orchard.Roles.Services;
using Orchard.Localization;
using System.Web.Mvc;

namespace Downplay.Quanta.Drivers
{
    public class EffectiveRolesConnectorDriver : LegacyModelDriver<ConnectorEventContext>
    {
        protected override string Prefix
        {
            get { return "EffectiveRoles"; }
        }

        private readonly IRoleService _roleService;
        private readonly INotifier _notifier;
        public Localizer T { get; set; }

        public EffectiveRolesConnectorDriver(INotifier notifier, IRoleService roleService)
        {
            _notifier = notifier;
            _roleService = roleService;
            T = NullLocalizer.Instance;
        }

        protected override ModelDriverResult Display(ConnectorEventContext model, dynamic shapeHelper, ModelShapeContext context)
        {
            // TODO: For display purposes we might want to group roles and display "Moderators: username, username; Admins: username, username"
            // Actually that could be done more easily with different connectors for each role rather than crazy grouping scenarios...
            return new ModelDriverResult();
        }

        protected override ModelDriverResult Editor(ConnectorEventContext model, dynamic shapeHelper, ModelShapeContext context)
        {
            var part = model.ConnectorContent.As<EffectiveRolesPart>();
            if (part == null) return new ModelDriverResult();
            var viewModel = BuildViewModel(part);
            return ModelShape("Connector_Editors_EffectiveRoles",
                () => shapeHelper.EditorTemplate(TemplateName: "Connector.Editors.EffectiveRoles", Model: viewModel, Prefix: FullPrefix(context)));
        }

        protected override ModelDriverResult Update(ConnectorEventContext model, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelShapeContext context)
        {
            var part = model.ConnectorContent.As<EffectiveRolesPart>();
            if (part == null) return new ModelDriverResult();
            var viewModel = BuildViewModel(part);
            var prefix = FullPrefix(context);
            if (updater.TryUpdateModel(viewModel, prefix, null, null))
            {
                // TODO: Check role exists and user is allowed permission
                part.EffectiveRoles = String.Join(" ", viewModel.SelectRoles.ToArray());
            }
            else
            {
                _notifier.Error(T("Error updating Effective Roles."));
            }
            return ModelShape("Connector_Editors_EffectiveRoles",
                () => shapeHelper.EditorTemplate(TemplateName: "Connector.Editors.EffectiveRoles", Model: viewModel, Prefix: prefix));
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

    }
}