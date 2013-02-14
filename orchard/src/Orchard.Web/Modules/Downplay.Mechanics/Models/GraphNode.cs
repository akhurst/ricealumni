using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement;

namespace Downplay.Mechanics.Models
{
    public class GraphNode : Graph
    {
        public ContentItem Connector { get; set; }
        public ContentItem Item { get; set; }
    }
}
