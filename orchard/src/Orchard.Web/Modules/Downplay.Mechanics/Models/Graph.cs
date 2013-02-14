using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;

namespace Downplay.Mechanics.Models
{
    public class Graph
    {

        public List<GraphNode> Nodes { get; protected set; }

        public Graph()
        {
            Nodes = new List<GraphNode>();
            _keyed = new Dictionary<int, GraphNode>();
        }

        protected Dictionary<int, GraphNode> _keyed;

        public GraphNode GetNode(int id)
        {
            if (_keyed.ContainsKey(id))
            {
                return _keyed[id];
            }
            return null;
        }

        public GraphNode MakeNode(ContentItem item)
        {
            var node = new GraphNode()
            {
                Item = item
            };
            return node;
        }

    }
}