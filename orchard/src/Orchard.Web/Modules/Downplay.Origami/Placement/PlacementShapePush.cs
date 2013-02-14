using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.DisplayManagement.Descriptors.ShapePlacementStrategy;

namespace Downplay.Origami.Placement
{
    /*
    public class ParadigmsShapePushAlterationBuilder : IPlacementAlterationBuilder {

        public void Alter(Orchard.DisplayManagement.Descriptors.PlacementInfo placement, string property, string value) {
            if (property == "push") {
                var mp = placement as ModelPlacementInfo;
                if (mp != null) {

                    mp.AddMutator((placementInfo, parentShape, shape, metadata, context) => {
                        placementInfo.ParentShape
                    });
                }
            }
        }
    }*/

    public class PlacementShapePush : PlacementNode
    {
        public string ShapeType { get; set; }
        public string Location { get; set; }
    }
}