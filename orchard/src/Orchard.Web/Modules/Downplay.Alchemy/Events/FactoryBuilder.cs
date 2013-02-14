using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Alchemy.Events {
    public class FactoryBuilder<TOutput> : ICompositionBuilder {
        private Func<CompositionContext, TOutput> factory;

        public FactoryBuilder(Func<CompositionContext, TOutput> factory) {
            this.factory = factory;
        }

        public void Build(CompositionContext context) {
            context.Factory<TOutput>(()=>factory(context));
        }
    }
}
