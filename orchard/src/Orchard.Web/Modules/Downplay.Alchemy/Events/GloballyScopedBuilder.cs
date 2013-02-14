using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Alchemy.Events {
    public class GloballyScopedBuilder : ScopedCompositionBuilder {

        public GloballyScopedBuilder(Action<ScopedCompositionBuilder> builder)
            : base(builder) {
        }
        protected override bool IsInScope(CompositionContext context) {
            return !context.IsChained;
        }
    }
}
