using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace Downplay.Mechanics.Impulses.Services
{
    public interface IImpulseService : IDependency
    {
        ImpulseDescriptor CheckForImpulse(string prefix, IContent content, object data = null);
        ImpulseActuationResult ActuateImpulse(ImpulseContext context);
        IEnumerable<IImpulse> ListImpulses(IContent item, string displayType, object data = null);
        IEnumerable<dynamic> BuildImpulseShapes(IContent content, string displayType, object data = null);
    }
}
