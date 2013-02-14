using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Mechanics.Impulses.Services;
using Orchard.ContentManagement;

namespace Downplay.Mechanics.Impulses.Defaults
{
    public interface IConnectorImpulseGenerator
    {
        string ImpulsePrefix { get; }
        string ImpulseName { get; }
        string ConnectorType(ImpulseContext context);
        IEnumerable<IContent> RightContent(ImpulseContext context);
        void ConnectorAlteration(ImpulseContext context, IContent connector);
    }
}
