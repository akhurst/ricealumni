using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.DisplayManagement.Descriptors;
using Orchard.DisplayManagement.Shapes;
using Downplay.Origami.Services;

namespace Downplay.Origami.Placement
{
    public class ModelPlacementInfo : PlacementInfo
    {
        public ModelPlacementInfo()
            : base()
        {
            Mutators = Enumerable.Empty<Action<ModelPlacementInfo, dynamic, dynamic, ShapeMetadata, ModelShapeContext>>();
        }

        /// <summary>
        /// List of delegate(placementInfo,parentShape,shape,metadata,context)
        /// </summary>
        public IEnumerable<Action<ModelPlacementInfo,dynamic,dynamic,ShapeMetadata,ModelShapeContext>> Mutators { get; set; }

        /// <summary>
        /// Add a delegate(placementInfo,parentShape,shape,metadata,context)
        /// </summary>
        public void AddMutator(Action<ModelPlacementInfo, dynamic, dynamic, ShapeMetadata, ModelShapeContext> mutator) {
            Mutators = Mutators.Concat(new[] { mutator });
        }

        public string Destination { get; set; }
    }
}
