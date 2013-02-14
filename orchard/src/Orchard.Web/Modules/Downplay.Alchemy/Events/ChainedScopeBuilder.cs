using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Alchemy.Events {
    public class ChainedScopeBuilder : ICompositionBuilder {
        protected string ident;

        public ChainedScopeBuilder(string ident) {
            this.ident = ident;
        }
        public void Build(CompositionContext context) {
            var scope = new CompositionContext(ident, context) { IsChained = true };
            // TODO: Again this looks very wrong!
            scope.Builder.Build(scope);
        }

    }
}
