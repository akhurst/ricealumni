using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions;
using Orchard.Commands;
using Orchard.ContentManagement;
using Orchard.Security;
using Orchard.Settings;
using Orchard.Core.Navigation.Services;
using Downplay.Mechanics.Services;
using Orchard.Core.Common.Models;
using Orchard;

namespace Downplay.Theory.Cartography.Commands
{
    public class MenuCommands : DefaultOrchardCommandHandler
    {
        private readonly IContentManager _contentManager;
        private readonly IMembershipService _membershipService;
        private readonly ISiteService _siteService;
        private readonly IMenuService _menuService;
        private readonly IMechanicsService _mechanics;

        public MenuCommands(
            IContentManager contentManager,
            IMembershipService membershipService,
            ISiteService siteService,
            IMenuService menuService,
            IMechanicsService mechanics,
            IOrchardServices services)
        {
            _contentManager = contentManager;
            _membershipService = membershipService;
            _siteService = siteService;
            _menuService = menuService;
            _mechanics = mechanics;
            Services= services;
        }

        public IOrchardServices Services { get; set; }

        [OrchardSwitch]
        public string Zone { get; set; }

        [OrchardSwitch]
        public string Layer { get; set; }

        [CommandName("menu build")]
        [CommandHelp("menu build [/Layer:<layer>] [/Zone:<zone>]\r\n\t" + "Builds a connected menu hierarchy from existing dot-notation positions, and adds it using a ContentWidget into the chosen layer and zone")]
        [OrchardSwitches("Layer,Zone")]
        public void Build()
        {
            Context.Output.WriteLine(T("Deprecated / not supported"));

            /*
            // Build a root
            var mainRoot = _contentManager.Create("MenuRoot");
            mainRoot.As<IdentityPart>().Identifier = "MainMenu";

            if (String.IsNullOrWhiteSpace(Layer))
            {
                Layer = "Default";
            }
            if (String.IsNullOrWhiteSpace(Zone))
            {
                Zone = "Navigation";
            }

            // Clip to site
            _mechanics.CreateConnector(Services.WorkContext.CurrentSite.Id, mainRoot.Id, "SiteToMenuRoot");
            // Get menus
            var menus = _menuService.Get();

            // Create positions dict
            var dict = menus.ToDictionary(m => m.MenuPosition, m => m);

            foreach (var m in menus)
            {
                if (m.MenuPosition.Contains('.'))
                {
                    // Find parent
                    var dotPos = m.MenuPosition.LastIndexOf('.');
                    var parentPos = m.MenuPosition.Substring(0, dotPos);
                    if (dict.ContainsKey(parentPos))
                    {
                        var parentMenu = dict[parentPos];
                        if (!_mechanics.ConnectorExists(parentMenu, m, "MenuChild"))
                        {
                            _mechanics.CreateConnector(parentMenu, m, "MenuChild");
                            Context.Output.WriteLine(T("Connected menu item {0} to {1}.").Text, parentMenu.MenuText, m.MenuText);
                        }
                    }
                }
                else
                {
                    if (!_mechanics.ConnectorExists(mainRoot, m, "MenuChild"))
                    {
                        // Connect top-level item to root
                        _mechanics.CreateConnector(mainRoot, m, "MenuChild");
                        Context.Output.WriteLine(T("Connected menu item {0} to root.").Text, m.MenuText);
                    }
                }
            }*/
        }

    }
}