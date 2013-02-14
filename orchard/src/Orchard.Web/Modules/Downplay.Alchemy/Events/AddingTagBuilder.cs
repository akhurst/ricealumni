using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Alchemy.Factories;

namespace Downplay.Alchemy.Events {

    public class AddingTagBuilder<T> : ICompositionBuilder
    where T:IContextTag {
        public void Build(CompositionContext context) {
            context.Tag<T>();
        }
    }
}
