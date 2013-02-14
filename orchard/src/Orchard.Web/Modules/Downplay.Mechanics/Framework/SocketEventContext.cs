using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement;
using Downplay.Mechanics.Settings;
using Downplay.Mechanics.Models;
using Downplay.Mechanics.Services;
using Downplay.Origami.Services;

namespace Downplay.Mechanics.Framework
{
    public class SocketEventContext
    {
        public SocketEventContext(IContent leftContent, ConnectorDescriptor connectorDefinition,SocketsModel socketsContext)
        {
            Left = leftContent.As<SocketsPart>().Endpoint;
            Connector = connectorDefinition;
            SocketFilters = new List<ISocketFilter>();
            SocketSorters= new List<ISocketFilter>();
            RootModel = socketsContext;
            RenderSocket = true;
            SocketMetadata = new SocketMetadata() {
                SocketName = connectorDefinition.Name,
                SocketTitle = Connector.Settings.SocketDisplayName
            };
            QueryFactory = new Lazy<SocketQuery>(() => {
                var query = Left.ContentPart.Sockets[Connector.Name];
                return query;
            });
        }

        public bool RenderSocket { get; set; }

        /// <summary>
        /// This is the content item we're connecting from
        /// </summary>
        public SocketEndpoint Left { get; protected set; }
        public ConnectorDescriptor Connector { get; set; }
        public SocketMetadata SocketMetadata { get; set; }

        protected Lazy<SocketQuery> QueryFactory { get; set; }
        public SocketQuery Query { get; set; }

        /// <summary>
        /// TODO: Rename to MechanicsModel?
        /// </summary>
        public SocketsModel RootModel { get; set; }

        public List<ISocketFilter> SocketFilters { get; protected set; }
        public List<ISocketFilter> SocketSorters { get; protected set; }
        public Func<IContentQuery,IEnumerable<IContent>> SocketPager { get; protected set; }

        /// <summary>
        /// Delegate to be called in the socket driver to load the filters
        /// </summary>
        public Action Filtering { get; set; }
    }
}
