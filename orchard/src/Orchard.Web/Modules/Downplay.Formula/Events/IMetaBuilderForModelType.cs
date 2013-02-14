using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Formula.Events {
    public interface IMetaBuilderForModelType : IMetaBuilder {
        Type ModelType { get; set; }
    }
}
