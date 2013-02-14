using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Origami.Services;

namespace Downplay.Alchemy.Services {
    public class TypedFactory {

        public TypedFactory() {
            Factories = new Dictionary<Type, Func<object>>();
            Cached = new FactoryDictionary<Type, object>();
            Cached.Loader(t => {
                if (Factories.ContainsKey(t)) {
                    var fac = Factories[t]();
                    if (_mutator != null) _mutator(t,fac);
                    return fac;
                }
                return null;
            });
        }

        protected IDictionary<Type, Func<object>> Factories { get; set; }
        protected FactoryDictionary<Type, object> Cached { get; set; }
        // TODO: Actually we could support the multiple mutators here?
        protected Action<Type,object> _mutator { get; set; }
        public void Mutate(Action<Type,object> mutator) {
            _mutator = mutator;
        }

        public void Add<T>(Func<T> factory) {
            Factories[typeof(T)] = () => factory();
            Cached.Reset(typeof(T));
        }

        public T Get<T>() {
            var result = Cached.Get(typeof(T));
            if (result == null) return default(T);
            return (T)result;
        }

        public void Remove<T>() {
            if (Factories.ContainsKey(typeof(T))) Factories.Remove(typeof(T));
            Cached.Reset(typeof(T));
        }
    }
}
