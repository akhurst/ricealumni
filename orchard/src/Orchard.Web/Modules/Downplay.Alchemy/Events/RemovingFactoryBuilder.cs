using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Alchemy.Events {
    public class RemovingFactoryBuilder<T> : ICompositionBuilder {
        public void Build(CompositionContext context) {
            context.Remove<T>();
        }
    }
}
