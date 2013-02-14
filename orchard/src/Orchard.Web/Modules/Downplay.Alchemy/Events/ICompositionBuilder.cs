using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Alchemy.Events {
    public interface ICompositionBuilder {
        void Build(CompositionContext context);
    }
}
