using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Paperclips;
using Orchard;
using Downplay.Mechanics.Services;
using Downplay.Mechanics.Impulses.Services;
using Downplay.Mechanics.Impulses.Defaults;
using Orchard.Environment.Extensions;
namespace Downplay.Theory.Cartography.Impulses
{
    public class ClipSiteMenu : IConnectorImpulseGenerator
    {
        private readonly IWorkContextAccessor _workContextAccessor;
        public ClipSiteMenu(
            IWorkContextAccessor workContextAccessor) 
        {
            _workContextAccessor = workContextAccessor;
        }

        public string ImpulsePrefix
        {
            get { return "Downplay.Theory.Cartography"; }
        }

        public string ImpulseName
        {
            get { return "ClipSiteMenu"; }
        }
        public string ConnectorType(ImpulseContext context)
        {
            return "MenuRootToSite";
        }

        public IEnumerable<Orchard.ContentManagement.IContent> RightContent(ImpulseContext context)
        {
            // Connect to site
            return new[]{
                _workContextAccessor.GetContext().CurrentSite
            };
        }

        public void ConnectorAlteration(ImpulseContext context, Orchard.ContentManagement.IContent connector)
        {
        }

    }
}