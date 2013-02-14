using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Origami.Services {
    public class ParadigmsContext {
        protected IEnumerable<String> Inherited { get; set; }
        protected IDictionary<String,bool> Keyed { get; set; }

        public ParadigmsContext() {
            Keyed = new Dictionary<string,bool>();
        }
        public ParadigmsContext(ParadigmsContext inheritFrom) {
            Inherited = inheritFrom.Keyed.Keys;
            Keyed = Inherited.ToDictionary(k=>k,k=>true);
        }

        public bool Has(string paradigm) {
            return Keyed.ContainsKey(paradigm);
        }

        public void Add(string paradigm) {
            Keyed[paradigm] = true;
        }
        public void Add(IEnumerable<string> paradigms) {
            foreach (var p in paradigms)
                Add(p);
        }
        public void Remove(string paradigm) {
            if (Has(paradigm)) {
                Keyed.Remove(paradigm);
            }
        }

        public IEnumerable<String> All() {
            return Keyed.Keys;
        }
    }
}
