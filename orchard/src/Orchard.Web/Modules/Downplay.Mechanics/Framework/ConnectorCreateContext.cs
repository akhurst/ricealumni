using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Downplay.Mechanics.Models;

namespace Downplay.Mechanics.Framework
{
    public class ConnectorCreateContext : ConnectorEventContext
    {

        public ConnectorCreateContext(IContent left, IContent right, ConnectorDescriptor connectorDefinition) : base(connectorDefinition)
        {
            LeftContent = left;
            RightField = new Lazy<SocketEndpoint>(()=>new SocketEndpoint(right.As<SocketsPart>()));
            Cancel = false;
        }

        public bool Cancel { get; set; }

        public IContent LeftContent { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public IContentQuery<ConnectorPart> SiblingConnectors { get; set; }

    }
}