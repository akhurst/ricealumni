using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Downplay.Mechanics.Models;

namespace Downplay.Mechanics.Framework {
    public class ConnectorEventContext {

        public IConnector ConnectorContent { get; set; }

        public SocketEventContext SocketContext { get; set; }
        public ConnectorDescriptor Descriptor { get; protected set; }
        public ConnectorEventContext(ConnectorDescriptor descriptor) {
            this.Descriptor = descriptor;
        }

        protected Lazy<SocketEndpoint> RightField;
        public SocketEndpoint Right { get { return RightField.Value; } }

        public virtual IConnector InverseConnectorContent { get; set; }
    }
}
