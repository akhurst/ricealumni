using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;
using Orchard.Environment.Extensions;
using Orchard.Events;

namespace Downplay.Mechanics.Impulses.Services
{
    public interface IImpulseProvider : IEventHandler
    {
        void Describing(ImpulseDescribeContext context);
        void Described(ImpulseDescribeContext context);
    }
}
