using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Alchemy.Factories;

namespace Downplay.Alchemy.Events {
    public class TaggedFactoryBuilder<TTag, TVal> : ICompositionBuilder
        where TTag:IContextTag {
        private Func<CompositionContext, TVal> factory;

        public TaggedFactoryBuilder(Func<CompositionContext, TVal> factory) {
            this.factory = factory;
        }

        public void Build(CompositionContext context) {
            context.Factory<TTag, TVal>(()=>factory(context));
        }
    }
}
