using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData.Models;
using Downplay.Mechanics.Framework;

namespace Downplay.Mechanics.Models
{
    /// <summary>
    /// This part is attachable to normal content items. It allows it to accept connectors.
    /// </summary>
    public class SocketsPart : ContentPart<SocketsPartRecord>
    {
        /// <summary>
        /// All groups of connector connected to this item
        /// </summary>
        public SocketFactory Sockets { get; set; }

        public SocketEndpoint Endpoint { get; set; }
    }
}