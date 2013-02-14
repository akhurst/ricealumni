using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Downplay.Origami.Services
{

    /// <summary>
    /// Helps us with recursive rendering. Allows Origami to take over and perform a complete child model rendering process.
    /// </summary>
    public class ChildShapeResult : ModelShapeResultBase
    {
        private string _shapeType;
        private Func<ModelChainContext,dynamic> _shape;
        private object _model;
        private string _prefix;

        public ChildShapeResult(string shapeType, Func<ModelChainContext,dynamic> shape, object model,string prefix) : base(shapeType, prefix)
        {
            _shapeType = shapeType;
            _shape = shape;
            _model = model;
            _prefix = prefix;
        }

        protected override dynamic BuildShape(ModelShapeContext context) {
            var chain = new ModelChainContext() {
                Model = _model,
                ShapeType = _shapeType,
                Prefix = _prefix
            };
            chain.Root = _shape(chain);
            context.ChainedResults.Add(chain);
            return chain.Root;
        }
    }
}