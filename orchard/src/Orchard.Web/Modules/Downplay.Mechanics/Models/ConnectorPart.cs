using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using Orchard.Core.Common.Utilities;
using Downplay.Mechanics.Framework;

namespace Downplay.Mechanics.Models
{
    /// <summary>
    /// This part is attached to Content Types that define relationships. 
    /// </summary>
    public class ConnectorPart : ContentPart<ConnectorPartRecord>, IConnector
    {

        private readonly LazyField<SocketsPart> _left = new LazyField<SocketsPart>();
        private readonly LazyField<SocketsPart> _right = new LazyField<SocketsPart>();
        private readonly LazyField<IConnector> _inverse = new LazyField<IConnector>();

        public LazyField<SocketsPart> LeftContentField { get { return _left; } }
        public LazyField<SocketsPart> RightContentField { get { return _right; } }
        public LazyField<IConnector> InverseConnectorField { get { return _inverse; } }
        
        public int LeftContentItemId
        {
            get
            {
                return (Record.LeftContentItem_id);
            }
            set
            {
                Record.LeftContentItem_id = value;
            }
        }

        public int RightContentItemId
        {
            get { return Record.RightContentItem_id; }
            set { Record.RightContentItem_id = value; }
        }

        public Framework.SocketParentContext ParentContext { get; set; }

        public int? InverseConnectorId
        {
            get { return Record.InverseConnector_id; }
            set { Record.InverseConnector_id = value; }
        }

        public SocketsPart LeftContent {
            get { return LeftContentField.Value; }
            set { LeftContentField.Value = value; }
        }

        public SocketsPart RightContent { get { return RightContentField.Value; } set { RightContentField.Value = value; } }
        public IConnector InverseConnector { get { return InverseConnectorField.Value; } set { InverseConnectorField.Value = value; } }
        
        /// <summary>
        /// Flags that a connector is *due* to get removed, when the left item is published. This is so things don't start disappearing prematurely.
        /// </summary>
        public bool DeleteWhenLeftPublished
        {
            get { return Record.DeleteWhenLeftPublished; }
            set { Record.DeleteWhenLeftPublished = value; }
        }

        public int? LeftContentVersionId { get { return Record.LeftContentVersionId; } set { Record.LeftContentVersionId = value;  } }
        public int? RightContentVersionId { get { return Record.RightContentVersionId; } set { Record.RightContentVersionId = value; } }

        public bool IgnorePermissions { get; set; }
    }
}