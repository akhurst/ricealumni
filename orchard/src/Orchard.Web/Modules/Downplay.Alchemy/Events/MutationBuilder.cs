using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Alchemy.Events {
    public class MutationBuilder<TVal> : ICompositionBuilder {
        private Action<TVal, CompositionContext> mutator;

        public MutationBuilder(Action<TVal, CompositionContext> mutator) {
            // TODO: Complete member initialization
            this.mutator = mutator;
        }

        public void Build(CompositionContext context) {

        }
    }
}
