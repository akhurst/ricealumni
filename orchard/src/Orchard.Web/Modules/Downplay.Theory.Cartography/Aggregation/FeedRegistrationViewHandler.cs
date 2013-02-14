using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Services;
using Orchard.Environment.Extensions;
using Orchard.ContentManagement;
using Orchard.Core.Feeds;
using System.Web.Routing;
using Downplay.Mechanics.Settings;
using Downplay.Mechanics.Models;
using Downplay.Mechanics.Framework;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement.Drivers;

namespace Downplay.Theory.Cartography.Aggregation
{
    [OrchardFeature("Downplay.Theory.Cartography.Aggregation")]
    public class FeedRegistrationViewHandler : SocketHandler
    {
        private readonly IFeedManager _feedManager;
        private readonly IMechanicsService _mechanics;
        public FeedRegistrationViewHandler(
            IFeedManager feedManager,
            IMechanicsService mechanics
            )
        {
            _feedManager = feedManager;
            _mechanics = mechanics;
        }

        protected override void Displaying(SocketDisplayContext context) {
            // TODO: Optionally expose a unified feed comprised of all applicable connector types.
            // TODO: We could feasibly want to register the feed on other display types
            if (context.Left.DisplayType == "Detail") {
                var settings = context.Connector.PartDefinition.Settings.GetModel<AggregationTypePartSettings>();
                if (settings != null && settings.ExposeFeed) {
                    _feedManager.Register(context.Left.ContentItem.GetTitle() + " - " + context.SocketMetadata.SocketTitle, "rss", new RouteValueDictionary { { "id", context.Left.ContentItem.Id }, { "connector", context.Connector.Name } });
                }
            }
        }

        private string ConnectorDisplayName(ConnectorDescriptor t)
        {
            return String.IsNullOrWhiteSpace(t.Settings.SocketDisplayName)
                ? (t.Definition.DisplayName + (t.Settings.AllowMany ? "s" : ""))
                : t.Settings.SocketDisplayName;
        }
    }
}