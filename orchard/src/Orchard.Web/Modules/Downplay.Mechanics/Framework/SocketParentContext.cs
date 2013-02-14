using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Origami.Services;

namespace Downplay.Mechanics.Framework
{
    public class SocketParentContext
    {
        public SocketParentContext(ConnectorEventContext model) {
            Connector = model;
            Socket = model.SocketContext;
            Sockets = model.SocketContext.RootModel;
            Parent = model.SocketContext.RootModel.ParentContext;
        }
        public string Prefix { get; set; }
        public SocketParentContext Parent { get; set; }
        public SocketsModel Sockets { get; set; }
        public SocketEventContext Socket { get; set; }
        public ModelShapeContext ModelContext { get; set; }
        public ConnectorEventContext Connector { get; set; }
    }
}
