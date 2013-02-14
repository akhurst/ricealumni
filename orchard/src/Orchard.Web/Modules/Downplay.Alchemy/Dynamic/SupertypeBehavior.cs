using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClaySharp;

namespace Downplay.Alchemy.Dynamic {
    public class SupertypeBehavior : ClayBehavior {

        private string _superType = "Null";
        private string _stereoType = "Null";
        private static readonly string[] _arrayMethods = new string[] { "AddRange", "Add", "Insert", "RemoveAt", /*"Contains", "IndexOf",*/ "Remove"/*, "CopyTo"*/ };

        public override object InvokeMember(Func<object> proceed, object self, string name, INamedEnumerable<object> args) {

            if (name == "Supertype") {

                if (args.Count() == 0) {
                    return _superType;
                }
                if (args.Positional.Count()==1) {
                    var param = args.Positional.First();
                    if (param is Type) {
                        _superType = ((Type)param).Name;
                    }
                    else {
                        _superType = param.ToString();
                    }
                    return self;
                }

            }

            if (name == "Stereotype") {

                if (args.Count() == 0) {
                    return _stereoType;
                }
                if (args.Positional.Count() == 1) {
                    var param = args.Positional.First();
                    if (param is Type) {
                        _stereoType = ((Type)param).Name;
                    }
                    else {
                        _stereoType = param.ToString();
                    }
                    return self;
                }
            }

            // TODO: Slow lookup, convert to a dict
            // Converts to Array supertype if we use any of the array manipulation methods
            if (_arrayMethods.Contains(name)) {
                dynamic dself = self;
                dself.Supertype("Array");
            }
            return proceed();

        }
        
    }
}
