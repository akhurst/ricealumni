using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Formula.Events {
    public class MetaField {
        public string Name { get; set; }

        public MetaDictionary Meta { get; set; }

        internal object Model(Origami.Services.ModelShapeContext context) {
            throw new NotImplementedException();
        }

    }
}
