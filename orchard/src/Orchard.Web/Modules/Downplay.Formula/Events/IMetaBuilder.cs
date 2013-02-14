using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Formula.Events {
    public interface IMetaBuilder {
        void Build(FormDescriptor meta);
    }
}
