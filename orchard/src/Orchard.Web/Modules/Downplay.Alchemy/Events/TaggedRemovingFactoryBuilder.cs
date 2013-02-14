using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Alchemy.Factories;

namespace Downplay.Alchemy.Events {
    public class TaggedRemovingFactoryBuilder<TTag, TVal> : ICompositionBuilder
        where TTag:IContextTag {

        public void Build(CompositionContext context) {
            context.Remove<TTag,TVal>();
        }
    }
}
