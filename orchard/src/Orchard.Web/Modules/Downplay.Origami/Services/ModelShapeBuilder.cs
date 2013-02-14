using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.DisplayManagement;

namespace Downplay.Origami.Services {
    public class ModelShapeBuilder {

        public ModelShapeContext Context { get; protected set; }

        public ModelShapeBuilder(object model, IShapeFactory factory) {
            Context = new ModelShapeContext(model,factory);
        }

        public string ContentType { get; set; }

    }
}
