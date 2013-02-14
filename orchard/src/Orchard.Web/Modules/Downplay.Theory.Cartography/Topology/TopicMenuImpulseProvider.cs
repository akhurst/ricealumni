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

namespace Downplay.Theory.Cartography.Topology
{
    /*
     * TODO: Fix if topics are needed
    [OrchardFeature("Downplay.Theory.Cartography.Topology")]
    public class TopicMenuImpulseProvider : IImpulseProvider
    {

        private readonly IMechanicsService _mechanics;

        public TopicMenuImpulseProvider(
            IOrchardServices services,
            IMechanicsService mechanics)
        {
            Services = services;
            _mechanics = mechanics;
        }

        public IOrchardServices Services { get; set; }

        public void Describing(ImpulseDisplayContext context)
        {
        }
        
        public void Described(ImpulseDisplayContext context)
        {
            // Shorten display names of common impulses
            var isMenu = (context.DisplayType == "Menu" || context.DisplayType == "MenuSub") && (context.Content.ContentItem.ContentType=="Topic" || context.Content.ContentItem.ContentType=="SiteToTopic");
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
     * */
}