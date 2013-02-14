using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Alchemy.Factories;

namespace Downplay.Alchemy.Events {
    public class PredicatedTaggedConditionalBuilder<T> : TaggedConditionalBuilder<T>
        where T : IContextTag {
        protected Func<T, bool> _predicate;

        public PredicatedTaggedConditionalBuilder(Func<T, bool> predicate, Action<ScopedCompositionBuilder> action)
            : base(action) {
            this._predicate = predicate;
        }
        
        protected override bool IsInScope(CompositionContext context) {
            if (base.IsInScope(context)) {
                // TODO: This causes a problem! We possibly could cause problems performing a Get during build, maybe there should be a specific
                // pool of stuff we *are* allowed to get, or we can think more deeply about the fact that we *may* need to do this kind of thing
                // based on options that won't be apparent until this stage. Or we could make sure that the routes are right in the first place,
                // which is the only case of a single use of the If predicate...
                var tag = context.Get<T>();
                return (_predicate(tag));
            }
            return false;
        }
    }

    public class PredicatedTaggedConditionalBuilder<TTag, TVal> : TaggedConditionalBuilder<TTag>
        where TTag: IContextTag<TVal>
    {
        protected Func<TVal, bool> _predicate;

        public PredicatedTaggedConditionalBuilder(Func<TVal, bool> predicate, Action<ScopedCompositionBuilder> action)
            : base(action)
        {
            this._predicate = predicate;
        }

        protected override bool IsInScope(CompositionContext context)
        {
            if (base.IsInScope(context))
            {
                // TODO: This causes a problem! We possibly could cause problems performing a Get during build, maybe there should be a specific
                // pool of stuff we *are* allowed to get, or we can think more deeply about the fact that we *may* need to do this kind of thing
                // based on options that won't be apparent until this stage. Or we could make sure that the routes are right in the first place,
                // which is the only case of a single use of the If predicate...
                var tag = context.Get<TTag,TVal>();
                return (_predicate(tag));
            }
            return false;
        }
    

    }
    
}
