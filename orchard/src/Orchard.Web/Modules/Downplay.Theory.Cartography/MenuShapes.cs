using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.DisplayManagement.Descriptors;

namespace Downplay.Theory.Cartography {
    public class MenuShapes : IShapeTableProvider {

        public void Discover(ShapeTableBuilder builder) {
            builder.Describe("Content")
                .OnDisplaying(displaying => {
                        if (displaying.ShapeMetadata.DisplayType == "Navigation") {
                            displaying.ShapeMetadata.Alternates.Add("Content_Navigation");
                        }
                    });
        }
    }
}