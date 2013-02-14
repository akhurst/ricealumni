using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Alchemy.Factories;

namespace Downplay.Alchemy.Events {
    public class TaggedMutationBuilder<TTag, TMut> : ICompositionBuilder
    where TTag: IContextTag {
        private Action<TMut, CompositionContext> mutator;

        public TaggedMutationBuilder(Action<TMut, CompositionContext> mutator) {
            // TODO: Complete member initialization
            this.mutator = mutator;
        }

        public void Build(CompositionContext context) {
            context.Mutate<TTag, TMut>((m)=>mutator(m,context));
        }
    }
}
