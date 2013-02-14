using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Impulses;
using Orchard.ContentManagement;
using Downplay.Mechanics.Services;
using Orchard;
using Downplay.Mechanics.Impulses.Services;
using Orchard.Environment.Extensions;
using Orchard.Security;

namespace Downplay.Theory.Cartography.Impulses
{
    /// <summary>
    /// If topology is enabled, we don't need both handlers; this will do the same thing
    /// </summary>
    /*
     * TODO: fix and reinstate if we need the old menu
    [OrchardSuppressDependency("Downplay.Theory.Cartography.Topology.TopicMenuImpulseProvider")]
    public class SiteMenuImpulseProvider : IImpulseProvider
    {

        private readonly IMechanicsService _mechanics;

        public SiteMenuImpulseProvider(
            IOrchardServices services,
            IMechanicsService mechanics)
        {
            Services = services;
            _mechanics = mechanics;
        }

        public IOrchardServices Services { get; set; }

        public void Describing(ImpulseDisplayContext context)
        {
            /*
            var connectorSettings = _mechanics.ConnectorSettings("MenuRootToSite");
            if (connectorSettings == null) return;
            var currentName = context.Content.ContentItem.TypeDefinition.Name;
            if (connectorSettings.ListAllowedContentLeft().Any(name => name == currentName))
            {
                var left = context.Content.Id;
                var right = Services.WorkContext.CurrentSite.Id;
                var connectors = _mechanics.Connectors(left, right, "MenuRootToSite").List();
                /* TODO: Generate impulses again when we've sorted things out a bit
                // We're only checking AdminPanel permission - could define specific permissions. But connector creation will be authorized by mechanics service anyway based on content type permissions.
                if (connectors.Count() > 0)
                {
                    context.Impulses.Add(new Impulse("UnclipSiteMenu", "Downplay.Theory.Cartography", isMenu?"X":"Remove from menu", new[]{StandardPermissions.AccessAdminPanel}));
                }
                else
                {
                    context.Impulses.Add(new Impulse("ClipSiteMenu", "Downplay.Theory.Cartography", "Add to menu", new[]{StandardPermissions.AccessAdminPanel}));
                }*/
            // }
    /*
        }
        
        public void Described(ImpulseDisplayContext context)
        {
            // Shorten display names of common impulses
            // TODO: Impulses need to supply icons
            var isMenu = (context.DisplayType == "Menu" || context.DisplayType == "MenuSub");
            if (isMenu)
            {
                // Amend existing impulses that we know of
                foreach (var i in context.Impulses)
                {
                    if (i.Name == "Edit")
                    {
                        i.DisplayName = i.DisplayName == "Edit" ? "E" : "C";
                    }
                }
            }
        }

        public void Actuating(ImpulseContext context)
        {
        }

        public void Actuated(ImpulseContext context)
        {
        }
    }
*/
}