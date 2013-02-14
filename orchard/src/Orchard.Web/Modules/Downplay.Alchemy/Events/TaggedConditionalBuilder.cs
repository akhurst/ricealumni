using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Alchemy.Factories;

namespace Downplay.Alchemy.Events {
    public class TaggedConditionalBuilder<TTag> : ScopedCompositionBuilder
        where TTag:IContextTag {

        public TaggedConditionalBuilder(Action<ScopedCompositionBuilder> action)
            : base(action) {
        }
        protected override bool IsInScope(CompositionContext context) {
            return context.Has<TTag>();
        }
    }
}
