using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Core.Feeds;
using Downplay.Mechanics;
using Downplay.Mechanics.Services;
using Orchard;
using Orchard.Environment.Extensions;
using System.Xml.Linq;
using Orchard.Settings;
using Orchard.Mvc.Html;
using Orchard.Mvc.Extensions;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Security;
using Orchard.Core.Settings.Metadata;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Downplay.Mechanics.Models;
using Orchard.Core.Common.Models;

namespace Downplay.Theory.Cartography.Aggregation
{
    /// <summary>
    /// TODO: This could move into a standard mechanics module instead of Theory, it's a bit obscure
    /// </summary>
    [OrchardFeature("Downplay.Theory.Cartography.Aggregation")]
    public class MechanicsFeedQueryProvider : IFeedQueryProvider, IFeedQuery
    {
        public IOrchardServices Services { get;set; }
        private readonly RequestContext _requestContext;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        public MechanicsFeedQueryProvider(
            IOrchardServices services,
            RequestContext requestContext,
            IContentDefinitionManager contentDefinitionManager
            )
        {
            Services = services;
            _requestContext = requestContext;
            _contentDefinitionManager = contentDefinitionManager;
        }

        public FeedQueryMatch Match(Orchard.Core.Feeds.Models.FeedContext context)
        {
            var idValue = context.ValueProvider.GetValue("id");
            if (idValue == null)
                return null;
            var connectorValue = context.ValueProvider.GetValue("connector");
            if (connectorValue == null)
                return null;

            var typeDef = _contentDefinitionManager.GetTypeDefinition((String)connectorValue.ConvertTo(typeof(String)));
            if (typeDef == null)
                return null;

            var connectorDef = typeDef.Parts.FirstOrDefault(p => p.PartDefinition.Name == "ConnectorPart");
            if (connectorDef == null)
                return null;

            var settings = connectorDef.Settings.GetModel<AggregationTypePartSettings>();
            if (!settings.ExposeFeed) {
                return null;
            }
            // TODO: Could additionally check a user permission specifically for feeds, and of course view permissions for both parent and child items

            return new FeedQueryMatch {FeedQuery= this, Priority = 10 };
        }

        public void Execute(Orchard.Core.Feeds.Models.FeedContext context)
        {
            var idValue = context.ValueProvider.GetValue("id");
            var connectorValue = context.ValueProvider.GetValue("connector");

            var limitValue = context.ValueProvider.GetValue("limit");
            var limit = 20;
            if (limitValue != null)
            {
                limit = (int)limitValue.ConvertTo(typeof(int));
            }

            var id = (int)idValue.ConvertTo(typeof(int));
            var connectorName = (string)connectorValue.ConvertTo(typeof(string));
            
            var item = Services.ContentManager.Get(id);

            var socket = item.As<SocketsPart>();

            var connectors = socket.Sockets[connectorName].Connectors.List(
                q => q.ForPart<CommonPart>().OrderBy<CommonPartRecord>(c => c.CreatedUtc),
                q=>q.ForPart<ConnectorPart>().Slice(0,limit));
            var site = Services.WorkContext.CurrentSite;
            var url = new UrlHelper(_requestContext);
            if (context.Format == "rss")
            {
                var link = new XElement("link");
                context.Response.Element.SetElementValue("title", GetTitle(item.GetTitle(), site));
                context.Response.Element.Add(link);
                context.Response.Element.SetElementValue("description", item.GetBody());
                context.Response.Contextualize(requestContext => link.Add(url.AbsoluteAction(()=>url.ItemDisplayUrl(item))));
            }
            else
            {
                context.Builder.AddProperty(context, null, "title", GetTitle(item.GetTitle(), site));
                context.Builder.AddProperty(context, null, "description", item.GetBody());
                context.Response.Contextualize(requestContext => context.Builder.AddProperty(context, null, "link", url.AbsoluteAction(() => url.ItemDisplayUrl(item))));
            }

            foreach (var child in connectors)
            {
                context.Builder.AddItem(context, child.RightContent.ContentItem);
            }

        }
        public static string GetTitle(string tagName, ISite site)
        {
            return site.SiteName + " - " + tagName;
        }


    }
}