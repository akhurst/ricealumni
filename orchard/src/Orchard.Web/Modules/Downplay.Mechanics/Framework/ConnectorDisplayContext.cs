using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Downplay.Mechanics.Models;

namespace Downplay.Mechanics.Framework
{
    public class ConnectorDisplayContext : ConnectorEventContext
    {
        public bool RenderConnector { get; set; }
        public SocketDisplayContext SocketDisplayContext { get { return SocketContext as SocketDisplayContext; } }
        public ConnectorDisplayContext(IConnector item, string displayType, SocketDisplayContext context) : base(context.Connector) {
            ConnectorContent = item;
            RenderConnector = true;
            SocketContext = context;
            RightField = new Lazy<SocketEndpoint>(() => new SocketEndpoint(ConnectorContent.RightContent, displayType));
        }
        
        /// <summary>
        /// Ensure InverseConnectorContent uses the lazy field
        /// </summary>
        public override IConnector InverseConnectorContent {
            get {
                return ConnectorContent.InverseConnector;
            }
        }
    }
}