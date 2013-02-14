using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.Records;

namespace Downplay.Mechanics.Models
{
    public class ConnectorPartRecord : ContentPartVersionRecord
    {
        public ConnectorPartRecord()
        {
            DeleteWhenLeftPublished = false;
        }

        public virtual int LeftContentItem_id { get; set; }
        public virtual int RightContentItem_id { get; set; }
        public virtual int? InverseConnector_id { get; set; }
        public virtual bool DeleteWhenLeftPublished { get; set; }

        public virtual int? LeftContentVersionId { get; set; }
        public virtual int? RightContentVersionId { get; set; }
        public virtual int? InverseConnectorVersionId { get; set; }
    }
}
