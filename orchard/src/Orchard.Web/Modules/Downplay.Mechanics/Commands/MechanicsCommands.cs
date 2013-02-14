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
using Downplay.Mechanics.Models;

namespace Downplay.Mechanics.Commands
{
    public class MechanicsCommands : DefaultOrchardCommandHandler
    {
        private readonly IContentManager _contentManager;
        private readonly IMembershipService _membershipService;
        private readonly IMechanicsService _mechanics;

        public MechanicsCommands(
            IContentManager contentManager,
            IMembershipService membershipService,
            IMechanicsService mechanics,
            IOrchardServices services)
        {
            _contentManager = contentManager;
            _membershipService = membershipService;
            _mechanics = mechanics;
            Services= services;
        }

        public IOrchardServices Services { get; set; }

        [OrchardSwitch]
        public string ContentTypes { get; set; }

        [OrchardSwitch]
        public bool DeleteOrphans { get; set; }

        [CommandName("connectors publish")]
        [CommandHelp("connectors publish [/ContentTypes:<types>]\r\n\t" + "Attempts to publish all connectors of specified type(s). This is an upgrade option, typically if you've changed a connector to be Draftable.")]
        [OrchardSwitches("ContentTypes")]
        public void PublishConnectors()
        {

            var types = String.IsNullOrWhiteSpace(ContentTypes)?null:ContentTypes.Split(new[]{','},StringSplitOptions.RemoveEmptyEntries);
            var q = _contentManager.Query<ConnectorPart>(VersionOptions.Draft,types);
            foreach (var c in q.List())
            {
                _contentManager.Publish(c.ContentItem);
            }
        }

        [CommandName("connector orphans")]
        [CommandHelp("connector orphans [/DeleteOrphans:<true|false>]\r\n\t" + "Scans the content database for orphan connectors (connectors which link to a deleted content item).")]
        [OrchardSwitches("DeleteOrphans")]
        public void ConnectorOrphans() {

            var q = _contentManager.Query<ConnectorPart>(VersionOptions.Latest);
            var total = 0;
            var orphLeft = 0;
            var orphRight = 0;
            foreach (var c in q.List()) {
                total++;
                var del = false;
                if (c.LeftContent == null || c.LeftContent.IsDeleted()) {
                    orphLeft++;
                    del = true;
                }
                if (c.RightContent == null || c.RightContent.IsDeleted()) {
                    orphRight++;
                    del = true;
                }
                if (DeleteOrphans && del) {
                    _contentManager.Remove(c.ContentItem);
                }
            }

            Console.WriteLine("Scanned {0}, left orphans {1}, right orphans {2}",total,orphLeft,orphRight);

        }
    }

    public static class ContentExtensions {
        public static bool IsDeleted(this IContent content) {
            if (content.ContentItem.VersionRecord != null) {
                if (!content.ContentItem.VersionRecord.Published && !content.ContentItem.VersionRecord.Latest) {
                    return true;
                }
            }
            return false;
        }
    }
}