using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Mechanics.Models;
using Orchard.ContentManagement;

namespace Downplay.Mechanics.Framework {
    public class SocketQuery {

        public SocketEndpoint Left { get; set; } 

        protected Lazy<ConnectorCollection> ConnectorsFactory { get; set; }
        public ConnectorCollection Connectors { get { return ConnectorsFactory.Value; } }
        public void ConnectorsLoader(Func<ConnectorCollection> factory) {
            ConnectorsFactory = new Lazy<ConnectorCollection>(factory);
        }
        protected Lazy<ConnectorDescriptor> DescriptorFactory { get; set; }
        public ConnectorDescriptor Descriptor { get { return DescriptorFactory.Value; } }
        public void DescriptorLoader(Func<ConnectorDescriptor> factory) {
            DescriptorFactory = new Lazy<ConnectorDescriptor>(factory);
        }

        protected Func<IContentQuery<ConnectorPart>> ConnectorQueryFactory { get; set; }
        /// <summary>
        /// This will generate a new, unfiltered, unsorted query each time; giving you currently stored database connectors
        /// which can then be queried further. Be aware that any call to this will cause database reads (potentially a lot)
        /// when you enumerate it so be careful.
        /// </summary>
        public IContentQuery<ConnectorPart> ConnectorQuery { get { return ConnectorQueryFactory(); } }
        public void ConnectorQueryLoader(Func<IContentQuery<ConnectorPart>> factory) {
            ConnectorQueryFactory = factory;
        }

        protected Lazy<IEnumerable<ConnectorPart>> ConnectorItemsFactory { get; set; }
        public IEnumerable<ConnectorPart> ConnectorItems { get { return ConnectorItemsFactory.Value; } }
        public void ConnectorItemsLoader(Func<IEnumerable<ConnectorPart>> factory) {
            ConnectorItemsFactory = new Lazy<IEnumerable<ConnectorPart>>(factory);
        }

        protected Lazy<IEnumerable<SocketsPart>> RightContentFactory { get; set; }
        /// <summary>
        /// TODO: It'd be useful to have a method for "Single"
        /// </summary>
        public IEnumerable<SocketsPart> RightContent { get { return RightContentFactory.Value; } }
        public void RightContentLoader(Func<IEnumerable<SocketsPart>> factory) {
            RightContentFactory = new Lazy<IEnumerable<SocketsPart>>(factory);
        }

        public int TotalCount { get { return Connectors.Count(); } }
    }
}
