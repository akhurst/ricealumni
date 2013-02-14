using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Alchemy.Factories;

namespace Downplay.Alchemy.Events {
    public abstract class ScopedCompositionBuilder : CompositionBuilder {

        private readonly Action<ScopedCompositionBuilder> _builder;
        private bool _built = false;

        protected abstract bool IsInScope(CompositionContext context);

        public ScopedCompositionBuilder(Action<ScopedCompositionBuilder> builder) {
            _builder = builder;
        }

        public override void Build(CompositionContext context) {
            if (IsInScope(context)) {
                if (!_built) {
                    _builder(this);
                    _built = true;
                }
                base.Build(context);
            }
        }

        public void Factory<TVal>(Func<CompositionContext, TVal> factory) {
            AddBuilder(new FactoryBuilder<TVal>(factory));
        }

        /// <summary>
        /// TODO: Extensionize
        /// </summary>
        /// <typeparam name="TTag"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="factory"></param>
        public void Factory<TTag, TVal>(Func<CompositionContext, TVal> factory)
            where TTag : IContextTag {
            AddBuilder(new TaggedFactoryBuilder<TTag, TVal>(factory));
        }
        public void Mutate<TVal>(Action<TVal, CompositionContext> mutator) {
            AddBuilder(new MutationBuilder<TVal>(mutator));
        }

        public void Mutate<TTag, TVal>(Action<TVal, CompositionContext> mutator)
        where TTag : IContextTag {
            AddBuilder(new TaggedMutationBuilder<TTag, TVal>(mutator));
        }

        public void If<TTag>(Action<ScopedCompositionBuilder> action)
            where TTag : IContextTag {
            AddBuilder(new TaggedConditionalBuilder<TTag>(action));
        }
        public void If<TTag>(Func<TTag, bool> predicate, Action<ScopedCompositionBuilder> action)
            where TTag : IContextTag {
            AddBuilder(new PredicatedTaggedConditionalBuilder<TTag>(predicate, action));
        }
        public void If<TTag,TVal>(Func<TVal, bool> predicate, Action<ScopedCompositionBuilder> action)
            where TTag : IContextTag<TVal>
        {
            AddBuilder(new PredicatedTaggedConditionalBuilder<TTag,TVal>(predicate, action));
        }

        public void Chain(string ident) {
            AddBuilder(new ChainedScopeBuilder(ident));
        }

        public void Tag<TTag>()
            where TTag : IContextTag {
            AddBuilder(new AddingTagBuilder<TTag>());
        }

        /// <summary>
        /// Enter a parameter scope
        /// </summary>
        /// <typeparam name="TParam"></typeparam>
        /// <param name="paramName"></param>
        /// <param name="mix"></param>
        public void Parameter<TParam>(string paramName, Action<ParameterScopedBuilder<TParam>> mix) {
            AddBuilder(new ParameterScopedBuilder<TParam>(paramName, mix));
        }

        /// <summary>
        /// Removes a typed value
        /// </summary>
        /// <typeparam name="TVal"></typeparam>
        public void Remove<TVal>() {
            AddBuilder(new RemovingFactoryBuilder<TVal>());
        }

        /// <summary>
        /// Removes a tagged and typed value
        /// </summary>
        /// <typeparam name="TTag"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        public void Remove<TTag, TVal>() where TTag : IContextTag {
            AddBuilder(new TaggedRemovingFactoryBuilder<TTag, TVal>());
        }
    }
}
