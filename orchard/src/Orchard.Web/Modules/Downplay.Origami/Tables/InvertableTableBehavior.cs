using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClaySharp;

namespace Downplay.Origami.Tables {
    public class InvertableTableBehavior : ClayBehavior {

        public InvertableTableBehavior() {
            Inverted = false;
        }

        public override object InvokeMember(Func<object> proceed, object self, string name, INamedEnumerable<object> args) {
            if (name == "Invert") {
                if (args.Count() == 0) {
                    Inverted = true;
                }
                else {
                    Inverted = (bool)(args.First());
                }
                return null;
            }
            return proceed();
        }

        public bool Inverted { get; set; }
    }
}