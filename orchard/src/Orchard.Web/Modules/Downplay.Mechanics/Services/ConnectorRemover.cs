using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;

namespace Downplay.Mechanics.Services
{
    public class ConnectorRemover
    {
        public ConnectorRemover(IContent connector)
        {
            Connector = connector;
        }

        public Orchard.ContentManagement.IContent Connector { get; set; }
    }
}