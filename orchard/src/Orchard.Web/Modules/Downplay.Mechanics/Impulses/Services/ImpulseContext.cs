using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions;
using Downplay.Mechanics.Services;

namespace Downplay.Mechanics.Impulses.Services
{
    [OrchardFeature("Downplay.Mechanics.Impulses")]
    public class ImpulseContext
    {
        public ImpulseContext() {
            ConnectorBuilders = new List<IConnectorBuilder>();
        }

        public ImpulseDescriptor Impulse { get; set; }
        public string ReturnUrl { get; set; }

        public Orchard.ContentManagement.IContent SourceContent { get; set; }
        public List<IConnectorBuilder> ConnectorBuilders { get; set; }
        public bool Actuated { get; set; }

        public string ImpulseName { get; set; }
    }
}