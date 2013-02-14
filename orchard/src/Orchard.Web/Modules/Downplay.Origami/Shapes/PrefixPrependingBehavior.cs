using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClaySharp;
using Orchard.DisplayManagement;

namespace Downplay.Origami.Shapes {
    public class PrefixPrependingBehavior : ClayBehavior {

        private readonly string _prefix;

        public PrefixPrependingBehavior(string prefix) {

            _prefix = prefix;

        }

        public override object InvokeMember(Func<object> proceed, object self, string name, INamedEnumerable<object> args) {

            // Intercept Add() and inject the prefix into the incoming shape
            if (name == "Add" && (args.Positional.Count() == 1 || args.Positional.Count() == 2)
                && (typeof(IShape).IsInstanceOfType(args.Positional.First()))) {
                dynamic added = args.Positional.First();
                if (added.Prefix != null) {
                    added.Prefix = _prefix + "." + ((string)added.Prefix);
                }
            }
            return proceed();
        }

    }
}