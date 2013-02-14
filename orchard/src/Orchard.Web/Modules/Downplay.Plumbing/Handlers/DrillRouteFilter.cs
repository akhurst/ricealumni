using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Framework;
using Downplay.Plumbing.Models;
using Orchard.Environment.Extensions;
using Downplay.Mechanics.Models;
using Orchard.Core.Common.Models;

namespace Downplay.Plumbing.Handlers {
    public class DrillRouteFilter : ISocketFilter
    {
        public DrillRouteFilter(int id)
        {
            _id = id;
        }

        public Orchard.ContentManagement.IContentQuery Apply(Orchard.ContentManagement.IContentQuery query)
        {
            // Filters the child items on drill id. 
            return query.ForPart<ConnectorPart>().Where<ConnectorPartRecord>(c => c.Id == _id);
        }

        private readonly int _id;
    }
}