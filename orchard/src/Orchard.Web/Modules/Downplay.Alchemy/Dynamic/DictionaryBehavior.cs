using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClaySharp;

namespace Downplay.Alchemy.Dynamic {
    public class DictionaryBehavior : ClayBehavior {

        public override object InvokeMember(Func<object> proceed, object self, string name, INamedEnumerable<object> args) {
            switch (name) {
                case "ContainsKey":
                    if (args.Positional.Count() == 1 && args.Positional.First().GetType() == typeof(String)) {
                        return ((dynamic)self)[args.Positional.First()] != null;
                    }
                    break;
            }
            return proceed();
        }

    }
}
