using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Alchemy.Factories;

namespace Downplay.Alchemy.Events {
    public class TaggedScopeBuilder<T> : CompositionBuilder
        where T:IContextTag {

        private readonly Action<ICompositionBuilder> _scopeBuilder;

        public TaggedScopeBuilder(Action<ICompositionBuilder> scopeBuilder) {
            _scopeBuilder = scopeBuilder;
        }


    }
}
