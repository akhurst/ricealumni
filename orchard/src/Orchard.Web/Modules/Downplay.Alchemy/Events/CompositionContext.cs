using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Alchemy.Factories;
using System.Collections.Concurrent;
using Downplay.Origami.Services;
using System.Collections;
using System.Web.Mvc;
using Downplay.Alchemy.Services;

namespace Downplay.Alchemy.Events {
    public class CompositionContext {

        public CompositionContext(){
                //foreach (var mut in Mutators.Get(t)) mut(val);
            Singletons = new Dictionary<Type, object>();
            Factories = new TypedFactory();
            Factories.Mutate((t, o) => {
                foreach (var mut in Mutators.Get(t)) mut(o);
            });
            TaggedFactories = new FactoryDictionary<Type, TypedFactory>();
            TaggedFactories.Loader((t)=>{
                var fac = new TypedFactory();
                fac.Mutate((t2, o) => {
                    foreach (var mut in TaggedMutators.Get(t).Get(t2)) mut(o);
                });
                return fac;
            });
            Mutators = new FactoryDictionary<Type, IList<Action<object>>>();
            Mutators.Loader((t) => new List<Action<object>>());
            TaggedMutators = new FactoryDictionary<Type, FactoryDictionary<Type, IList<Action<object>>>>();
            TaggedMutators.Loader((t) => {
                var muts = new FactoryDictionary<Type, IList<Action<object>>>();
                muts.Loader((t2) => new List<Action<object>>());
                return muts;
            });
        }

        public CompositionContext(string ident, CompositionContext context) {
            // Make a new context substituting in a different Identifier
            Identifier = ident;
            // TODO: Wrap all the following up into a single object for quick transposition here (doesn't happen too often so not a huge
            // problem)
            Singletons = context.Singletons;
            Factories = context.Factories;
            TaggedFactories = context.TaggedFactories;
            Mutators = context.Mutators;
            TaggedMutators = context.TaggedMutators;
            Values = context.Values;
            Builder = context.Builder;
        }

        public string Identifier { get; set; }

        protected IDictionary<Type, object> Singletons { get; set; }
        protected TypedFactory Factories { get; set; }
        protected FactoryDictionary<Type, TypedFactory> TaggedFactories { get; set; }
        protected FactoryDictionary<Type, IList<Action<object>>> Mutators { get; set; }
        protected FactoryDictionary<Type, FactoryDictionary<Type, IList<Action<object>>>> TaggedMutators { get; set; }

        public System.Web.Mvc.IValueProvider Values { get; set; }
        public ICompositionBuilder Builder { get; set; }
        
        public void Factory<TFac>(Func<TFac> factory) {
            Factories.Add(factory);
        }
        /// <summary>
        /// Factory scoped to a tagging context type
        /// </summary>
        /// <typeparam name="TMeta"></typeparam>
        /// <typeparam name="TFac"></typeparam>
        /// <param name="factory"></param>
        public void Factory<TTag, TFac>(Func<TFac> factory) where TTag : IContextTag {
            TaggedFactories.Get(typeof(TTag)).Add(factory);

        }
        public void Mutate<TTag, TMut>(Action<TMut> mutator) where TTag : IContextTag {
            // TODO: Following contains a suspicious cast, need to make all these functions behave better
            TaggedMutators.Get(typeof(TTag)).Get(typeof(TMut)).Add(o => mutator((TMut)o));
        }
        
        public TFac Get<TTag, TFac>() where TTag : IContextTag {
            return TaggedFactories.Get(typeof(TTag)).Get<TFac>();
        }

        public void Remove<TFac>() {
            Factories.Remove<TFac>();
            // TODO: Remove Mutators or doesn't it matter?
        }
        public void Remove<TTag, TFac>() where TTag : IContextTag {
            TaggedFactories.Get(typeof(TTag)).Remove<TFac>();
        }

        public TFac Get<TFac>() {
            return Factories.Get<TFac>();
//            var tfac = TaggedFactories.Keys.FirstOrDefault(t => typeof(IContextTag<TVal>).IsAssignableFrom(t));
        }

        
        public void Single<T>(T add) {
            Singletons[typeof(T)] = add;
        }

        public T Single<T>() {
            if (Singletons.ContainsKey(typeof(T))) {
                return (T)Singletons[typeof(T)];
            }
            return default(T);
        }

        public bool Has<TTag>() 
            where TTag : IContextTag {
                return TaggedFactories.Has(typeof(TTag));
        }

        public void Tag<TTag>()
            where TTag : IContextTag {
            // Simply ensure the tagged factory is generated
            TaggedFactories.Get(typeof(TTag));
        }

        public bool HasParam<TParam>(string name) {
            var value = Values.GetValue(name);
            return (value != null && value.AttemptedValue != null);
        }

        public TParam Param<TParam>(string name) {
            var value = Values.GetValue(name);
            if (value != null && value.AttemptedValue != null) {
                var convert = value.ConvertTo(typeof(TParam));
                if (convert != null) {
                    return (TParam)convert;
                }
            }
            return default(TParam);
        }

        public bool IsChained { get; set; }

        internal CompositionResult Build() {
            throw new NotImplementedException();
        }
    }

}
