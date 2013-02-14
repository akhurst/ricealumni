using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClaySharp;

namespace Downplay.Alchemy.Dynamic {
    public class StuffOnDemandBehavior : ClayBehavior {
        private readonly object _parent;
        private readonly object _parentName;

        public StuffOnDemandBehavior(object self, string parentName) {
            this._parent = self;
            this._parentName = parentName;
        }

        /// <summary>
        /// As soon as a member gets set, we can actually create the stuff
        /// </summary>
        /// <param name="proceed"></param>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object SetMember(Func<object> proceed, object self, string name, object value) {
            // Create the parent stuff
            Stuff stuff = ((dynamic)_parent)[_parentName] = new Stuff();
            // Set the member on it
            return ((dynamic)stuff)[name] = value;
        }

        public override object InvokeMember(Func<object> proceed, object self, string name, INamedEnumerable<object> args) {
            Stuff stuff = ((dynamic)_parent)[_parentName] = new Stuff();
            return (stuff as IClayBehaviorProvider).Behavior.InvokeMember(proceed, self, name, args);
        }

        public override object SetIndex(Func<object> proceed, object self, IEnumerable<object> keys, object value) {
            if (keys.Count() == 1) {
                // Create the parent stuff
                Stuff stuff = ((dynamic)_parent)[_parentName] = new Stuff();
                // Set the member on it
                return ((dynamic)stuff)[keys.First().ToString()] = value;
            }
            return proceed();
        }

    }
}
