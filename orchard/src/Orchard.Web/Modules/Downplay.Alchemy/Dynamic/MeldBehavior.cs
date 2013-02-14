using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClaySharp;
using System.Linq.Expressions;
using Microsoft.CSharp.RuntimeBinder;
using System.Collections;

namespace Downplay.Alchemy.Dynamic {
    public class MeldBehavior : ClayBehavior {

        public override object InvokeMember(Func<object> proceed, object self, string name, INamedEnumerable<object> args) {

            if (name == "Meld") {
                // Perform meld
                foreach (var o in args.Positional) {
                    Meld(self, o);
                }
                return null;
            }

            return proceed();
        }

        private void Meld(object self, object o) {
            if (o == null) return; // throw new Exception("Cannot meld null object");
            // If it's Clay, special handling required
            if (o is IClayBehaviorProvider) {
                var dict = new Dictionary<string,object>();
                ((IClayBehaviorProvider)o).Behavior.GetMembers(() => null, o, dict);
                dynamic stuff = self;
                foreach (var kv in dict) {
                    stuff[kv.Key] = kv.Value;
                }
            }
            else if (typeof(IDictionary).IsAssignableFrom(o.GetType())) {
                var dict = ((IDictionary)o);
                dynamic dself = self;
                foreach (var key in dict.Keys) {
                    dself[key] = dict[key];
                }
            }
            else {
                // Reflection meld (copied from Clay Factory which is used for property assignment on construct e.g. Display.SomeShape(new { foo=bar })
                var assigner = GetAssigner(o.GetType());
                if (assigner != null)
                    assigner.Invoke(self, o);
            }
        }

        // Copied from ClayFactoryBehavior
        private static Action<dynamic, object> GetAssigner(Type sourceType) {
            lock (_assignerCache) {
                Action<dynamic, object> assigner;
                if (_assignerCache.TryGetValue(sourceType, out assigner))
                    return assigner;


                // given "sourceType T" with public properties, e.g. X and Y, generate the following lambda
                //
                // (dynamic target, object source) => { 
                //    target.X = ((T)source).X; 
                //    target.Y = ((T)source).Y;
                //  }

                var targetParameter = Expression.Parameter(typeof(object), "target");
                var sourceParameter = Expression.Parameter(typeof(object), "source");

                // for each propertyInfo, e.g. X
                // produce dynamic call site, (target).X = ((T)source).X
                var assignments = sourceType.GetProperties().Select(
                    property => Expression.Dynamic(
                        Binder.SetMember(
                            CSharpBinderFlags.None,
                            property.Name,
                            typeof(void),
                            new[] {
                                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                            }),
                        typeof(void),
                        targetParameter,
                        Expression.Property(
                            Expression.Convert(sourceParameter, sourceType),
                            property)));

                // Fix test "CanInitializeWithEmptyAnonymousType"
                if (assignments.Count() == 0) return null;

                var lambda = Expression.Lambda<Action<dynamic, object>>(
                    Expression.Block(assignments),
                    targetParameter,
                    sourceParameter);

                assigner = lambda.Compile();
                _assignerCache.Add(sourceType, assigner);
                return assigner;
            }

        }


        static readonly Dictionary<Type, Action<dynamic, object>> _assignerCache = new Dictionary<Type, Action<dynamic, object>>();

    }
}
