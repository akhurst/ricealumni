using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Formula.Events {
    public class MetaDictionary {

        private Dictionary<Type, object> Dictionary = new Dictionary<Type, object>();

        public void With<T>(Action<T> alteration = null)
            where T : IMeta, new() {
            var meta = GetOrAdd<T>(() => new T());
            if (alteration != null)
                alteration(meta);
        }

        protected T GetOrAdd<T>(Func<T> factory) {
            if (!HasMeta<T>()) {
                AddMeta(factory());
            }
            return GetMeta<T>();
        }

        protected void AddMeta<T>(T meta) {
            Dictionary[typeof(T)] = meta;
        }
        protected bool HasMeta<T>() {
            return Dictionary.ContainsKey(typeof(T));
        }
        protected T GetMeta<T>() {
            return (T)(Dictionary[typeof(T)]);
        }

    }
}
