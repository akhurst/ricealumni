using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Alchemy.Events {
    public class IdentifierScopedBuilder : ScopedCompositionBuilder {

        protected string _ident;

        public IdentifierScopedBuilder(string ident, Action<ScopedCompositionBuilder> mix) : base(mix) {
            _ident = ident;
        }
        
        protected override bool IsInScope(CompositionContext context) {
            return (context.Identifier == _ident);
        }
    }
}
