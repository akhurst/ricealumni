using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Mechanics.Services;
using Downplay.Mechanics.Models;
using Downplay.Mechanics.Framework;
using Orchard.ContentManagement;

namespace Downplay.Mechanics.Services
{
    public class ConnectorCreator : IConnectorBuilder
    {
        private Action<IConnector> transform;
                public IContent left { get; set; }
        public IContent right { get; set; }
        public int RightContentItemId { get; set; }
        public string ConnectorType { get; set; }
        public bool IgnorePermissions { get; set; }

        public ConnectorCreator(int rightContentItemId, string connectorType)
        {
            RightContentItemId = rightContentItemId;
            ConnectorType = connectorType;
        }

        public ConnectorCreator(int rightContentItemId, string connectorType, bool ignorePermissions=false) : this(rightContentItemId,connectorType)
        {
            IgnorePermissions = ignorePermissions;
        }

        public ConnectorCreator(IContent left, int addedId, string connectorType, Action<IConnector> transform, bool ignorePermissions = false):this(addedId,connectorType,ignorePermissions) {
            this.transform = transform;
            this.left = left;
        }

        public ConnectorCreator(IContent left, IContent right, string connectorType, Action<IConnector> transform, bool ignorePermissions) : this(left,0,connectorType, transform, ignorePermissions) {
            this.right = right;
        }

        public IEnumerable<IConnector> Create(IMechanicsService _mechanics) {
            IEnumerable<IConnector> conn = null;
            if (right != null) {
                if (right.ContentItem.VersionRecord==null) {
                    right.ContentItem.ContentManager.Create(right);
                }
                conn = _mechanics.CreateConnector(left, right, ConnectorType, IgnorePermissions);
            }
            else {
                conn = _mechanics.CreateConnector(left, RightContentItemId, ConnectorType, IgnorePermissions);
            }
            if (transform != null) {
                foreach (var c in conn) { 
                    transform(c);
                }
            }
            return conn;
        }

    }
}
