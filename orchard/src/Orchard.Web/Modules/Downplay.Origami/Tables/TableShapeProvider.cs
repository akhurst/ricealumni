using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.DisplayManagement.Descriptors;
using Orchard.DisplayManagement;

namespace Downplay.Origami.Tables {
    public class TableShapeProvider : IShapeTableProvider {

        public void Discover(ShapeTableBuilder builder) {
            builder.Describe("Table")
                .OnCreating(creating => {
                    creating.Behaviors.Add(new RowHoldingBehavior(()=>creating.New.Row()));
                });
            builder.Describe("Row")
                .OnCreating(creating => {
                    creating.Behaviors.Add(new CellHoldingBehavior(() => creating.New.Cell()));
                });
        }

    }
}