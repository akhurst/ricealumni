using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;
using Orchard.Events;

namespace Downplay.Origami.Placement
{
    public interface IPlacementAlterationProvider : IEventHandler
    {
        void Alter(Orchard.DisplayManagement.Descriptors.PlacementInfo placement, string property, string value);
    }
}
