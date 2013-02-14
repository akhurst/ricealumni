using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Downplay.Mechanics {

    /// <summary>
    /// Lazy factory dictionary implementation
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class LazyDictionary<TKey,TValue> : IDictionary<TKey,TValue>
        where TValue:class
        {

        private readonly ILazyFactory<TKey, TValue> _factory;
        private readonly IDictionary<TKey, Tuple<bool, TValue>> _inner;

        public LazyDictionary(ILazyFactory<TKey, TValue> factory) {
            _factory = factory;
            _inner = new Dictionary<TKey, Tuple<bool,TValue>>();
        }

        public void Add(TKey key, TValue value) {
            _inner[key] = new Tuple<bool, TValue>(true, value);
        }

        public bool ContainsKey(TKey key) {
            return _inner.ContainsKey(key) && _inner[key].Item1;
        }

        public ICollection<TKey> Keys {
            get { return _inner.Keys; }
        }

        /// <summary>
        /// Sets a false value for the internal dictionary, so a lazy will never be generated for this key, or unset if already generated
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(TKey key) {
            // TODO: Hook it up so removing the socket changes the connector configuration?
            _inner[key] = new Tuple<bool, TValue>(false, default(TValue));
            return true;
        }

        public bool TryGetValue(TKey key, out TValue value) {

            if (!_inner.ContainsKey(key)) {
                var result = _factory.TryGetValue(key,out value);
                _inner[key] = new Tuple<bool,TValue>(result,value);
                return result;
            }
            var pair = _inner[key];
            if (pair.Item1) {
                value = pair.Item2;
                return true;
            }
            value = default(TValue);
            return false;
        }

        /// <summary>
        /// NOTE: Doesn't (can't) enumerate everything, this will only return lazy values that have already been instanced.
        /// </summary>
        public ICollection<TValue> Values {
            get { 
                return _inner.Values.Select(v=>v.Item2).ToList(); }
        }

        public TValue this[TKey key] {
            get {
                TValue output;
                if (TryGetValue(key, out output)) {
                    return output;
                }
                throw new ArgumentException("Key does not exist","key");
            }
            set {
                _inner[key] = new Tuple<bool, TValue>((value != default(TValue)), value);
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item) {
            this[item.Key] = item.Value;
        }

        /// <summary>
        /// Resets all the lazies, they will be regenerated
        /// </summary>
        public void Clear() {
            _inner.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) {
            return ContainsKey(item.Key) && this[item.Key] == item.Value;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            array.CopyTo(GetEnumerable().ToArray(), arrayIndex);
        }

        protected IEnumerable<KeyValuePair<TKey, TValue>> GetEnumerable() {
            return _inner.Select(pair => new KeyValuePair<TKey,TValue>(pair.Key, pair.Value.Item2));
        }

        public int Count {
            get { return _inner.Count; }
        }

        public bool IsReadOnly {
            get { 
                // TODO: Consider making read only
                return _inner.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) {
            if (ContainsKey(item.Key) && this[item.Key]==item.Value) {
                return Remove(item.Key);
            }
            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            return GetEnumerable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerable().GetEnumerator();
        }
    }
}