using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Alchemy.Events {
    /// <summary>
    /// This is the root builder. Whenever we add builders to this it inserts a Globally-scoped builder (which is how I support Chain scopes where we *don't* want to
    /// chain global scopes because they're already there).
    /// </summary>
    public class RootBuilder : CompositionBuilder {

        /// <summary>
        /// Enter a global scope
        /// </summary>
        /// <param name="mix"></param>
        public void Global(Action<ScopedCompositionBuilder> mix) {
            AddBuilder(new GloballyScopedBuilder(mix));
        }
    }
}
