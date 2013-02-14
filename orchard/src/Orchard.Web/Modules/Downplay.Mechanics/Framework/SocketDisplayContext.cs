using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData.Models;
using Downplay.Mechanics.Models;
using Downplay.Origami.Services;

namespace Downplay.Mechanics.Framework
{
    public class SocketDisplayContext : SocketEventContext
    {

        public SocketDisplayContext(IContent leftContent, ConnectorDescriptor connectorDefinition, string displayType, SocketsModel socketsContext)
            : base(leftContent, connectorDefinition, socketsContext)
        {
            Left.DisplayType = displayType;
            CacheSocket = false;
            Paradigms = new ParadigmsContext();
        }

        public string LayoutPlacement { get; set; }

        public ParadigmsContext Paradigms { get; set; }

        public bool CacheSocket { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ModelShapeContext ModelContext { get; set; }

        /// <summary>
        /// Name of the socket (i.e. the connector name)
        /// </summary>
        public string Name { get { return Connector.Name; } }

    }
}
