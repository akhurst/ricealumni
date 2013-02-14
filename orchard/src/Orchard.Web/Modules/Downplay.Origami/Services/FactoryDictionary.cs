using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Origami.Services {
    public class FactoryDictionary<TKey, TValue> {

        protected IDictionary<TKey, TValue> Inner;
        protected Func<TKey, TValue> Factory;

        public FactoryDictionary() {
            Inner = new Dictionary<TKey, TValue>();
        }

        public TValue Get(TKey key) {
            if (!Inner.ContainsKey(key)) {
                Inner[key] = Factory(key);
            }
            return Inner[key];
        }

        /// <summary>
        /// Set the factory for the dictionary
        /// </summary>
        /// <param name="factory"></param>
        public void Loader(Func<TKey, TValue> factory) {
            Factory = factory;
        }

        public void Reset(TKey type) {
            if (Inner.ContainsKey(type))
                Inner.Remove(type);
        }

        /// <summary>
        /// This will only tell you if an object has *already been generated yet*; it won't tell you if the factory is capable of generating it.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Has(TKey key) {
            return Inner.ContainsKey(key);
        }
    }
}
