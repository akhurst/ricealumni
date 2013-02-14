using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;

namespace Downplay.Origami.Placement
{
    public interface IPlacementPredicateProvider : IDependency
    {
        Func<Orchard.DisplayManagement.Descriptors.ShapePlacementContext, bool> Predicate(KeyValuePair<string, string> term, Func<Orchard.DisplayManagement.Descriptors.ShapePlacementContext, bool> predicate);
    }
}
