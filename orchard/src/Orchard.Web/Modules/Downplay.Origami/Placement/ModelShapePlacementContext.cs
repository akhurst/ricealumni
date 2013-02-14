using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.DisplayManagement.Descriptors;
using Downplay.Origami.Services;

namespace Downplay.Origami.Placement
{
    public class ModelShapePlacementContext : ShapePlacementContext
    {
        public ModelShapeContext ModelContext { get; set; }
    }
}
