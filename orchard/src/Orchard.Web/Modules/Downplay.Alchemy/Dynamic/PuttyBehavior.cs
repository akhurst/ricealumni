using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClaySharp;
using ClaySharp.Behaviors;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Collections;

namespace Downplay.Alchemy.Dynamic {
    public class StuffBehavior : ClayBehavior {

        protected object _localValue;

        public override object GetMember(Func<object> proceed, object self, string name) {
            // Already have a member?
            var existing = proceed();

            // Otherwise return a temp clay that will create a stuff when a property is set (or return nil if it's a nested get)
            if ((dynamic)existing == null) {
                return ClayActivator.CreateInstance(new IClayBehavior[] {                
                    new NilBehavior(), // Causes to object to be effectively null until a member is activated
                    new InterfaceProxyBehavior(),
                    new StuffBehavior(), // Means that child members will also return StuffOnDemand
                    new StuffOnDemandBehavior(self,name)});
                
            }
            return existing;
        }

        public override object Convert(Func<object> proceed, object self, Type type, bool isExplicit) {
            bool nullable = false;
            Type actualType = type;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                actualType = type.GetGenericArguments().First();
                nullable = true;
            }
            if (_localValue == null) {
                if (nullable) return null;
            }
            else {
                // See if the value can be return as is
                if (type.IsInstanceOfType(_localValue)) {
                    return _localValue;
                }
                // Convert to string
                if (typeof(String).IsAssignableFrom(actualType)) {
                    return _localValue.ToString();
                }
                var localType = _localValue.GetType();
                // Handle value types
                if ((actualType.IsPrimitive || actualType.IsValueType) && (typeof(String).IsInstanceOfType(_localValue) || localType.IsPrimitive || localType.IsValueType)) {
                    // Convert from string
                    if (typeof(String).IsInstanceOfType(_localValue)) {
                        string text = (string)_localValue;
                        if (actualType == typeof(int)) {
                            int result = 0;
                            if (Int32.TryParse(text, out result)) {
                                return result;
                            }
                            if (nullable) return null;
                            else throw new InvalidOperationException("Could not convert value");
                        }
                        else if (actualType == typeof(long)) {
                            long result = 0;
                            if (Int64.TryParse(text, out result)) {
                                return result;
                            }
                            if (nullable) return null;
                            else throw new InvalidOperationException("Could not convert value");
                        }
                        else if (actualType == typeof(float)) {
                            float result = 0f;
                            if (float.TryParse(text, out result)) {
                                return result;
                            }
                            if (nullable) return null;
                            else throw new InvalidOperationException("Could not convert value");
                        }
                        else if (actualType == typeof(double)) {
                            double result = 0;
                            if (double.TryParse(text, out result)) {
                                return result;
                            }
                            if (nullable) return null;
                            else throw new InvalidOperationException("Could not convert value");
                        }
                        else if (actualType == typeof(decimal)) {
                            decimal result = 0;
                            if (decimal.TryParse(text, out result)) {
                                return result;
                            }
                            if (nullable) return null;
                            else throw new InvalidOperationException("Could not convert value");
                        }
                        else if (actualType == typeof(bool)) {
                            bool result = false;
                            if (bool.TryParse(text, out result)) {
                                return result;
                            }
                            if (nullable) return null;
                            else throw new InvalidOperationException("Could not convert value");
                        }
                    }
                    else {
                        // Convert one value type to another
                        try {
                            var result = System.Convert.ChangeType(_localValue, type);
                            return result;
                        }
                        catch {
                            if (nullable) return null;
                            return proceed();
                        }
                    }
                }
            }

            return proceed();
        }

        public override object BinaryOperation(Func<object> proceed, object self, System.Linq.Expressions.ExpressionType operation, object value) {
            var supertype = ((dynamic)self).Supertype();
            var isObject = (supertype != "Value" && supertype != "Null");
            switch (operation) {
                case ExpressionType.Equal:
                    return (value==null)
                        ? (!isObject && _localValue == null)
                        : (isObject ? false : (_localValue != null && value.Equals(_localValue)));
                case ExpressionType.NotEqual:
                    return (value == null)
                        ? (isObject || _localValue != null)
                        : (isObject ? false : (_localValue == null || !value.Equals(_localValue)));
                case ExpressionType.GreaterThan:
                    return (value == null) ? false :
                        (isObject ? false : (_localValue != null && (Compare(_localValue,value)>0)));
                case ExpressionType.GreaterThanOrEqual:
                    return (value == null) ? false :
                        (isObject ? false : (_localValue != null && (Compare(_localValue, value) >= 0)));
                case ExpressionType.LessThan:
                    return (value == null) ? false :
                        (isObject ? false : (_localValue != null && (Compare(_localValue, value) < 0)));
                case ExpressionType.LessThanOrEqual:
                    return (value == null) ? false :
                        (isObject ? false : (_localValue != null && (Compare(_localValue, value) <= 0)));
            }

            return proceed();
        }

        private int Compare(object a, object b) {
            object c = b;
            if (a.GetType() != b.GetType()) {
                c = System.Convert.ChangeType(b, a.GetType());
            }
            return ((IComparable)a).CompareTo(c);
        }

        public override object InvokeMember(Func<object> proceed, object self, string name, INamedEnumerable<object> args) {

            if (name == "Set") {
                if (args.Positional.Count() == 1) {
                    SetLocal(self, args.First());
                    return self;
                }
            }
            if (name == "Get" && (args.Positional.Count() == 0)) {
                var type = ((dynamic)self).Supertype();
                if (type == "Null") return null;
                if (type == "Value") return _localValue;
                if (type == "Array") return ((IEnumerable<dynamic>)((dynamic)self)).Select(o => o.Get());
                return self;
            }
            return proceed();
        }

        public override object InvokeMemberMissing(Func<object> proceed, object self, string name, INamedEnumerable<object> args) {

            // Attempt to invoke on base object if we have one
            if (_localValue != null) {
                if (_localValue.GetType().GetMembers().Any(m => m.Name == name)) {
                    try {
                        return _localValue.GetType().InvokeMember(name, BindingFlags.InvokeMethod, null, _localValue, args.Positional.ToArray());
                    }
                    catch {
                        return proceed();
                    }
                }
            }
            return proceed();
        }

        public override object GetMemberMissing(Func<object> proceed, object self, string name) {
            // Attempt to get on base object if we have one
            if (_localValue != null) {
                var prop = _localValue.GetType().GetProperty(name);
                if (prop!=null) 
                    return prop.GetValue(_localValue, null);

                var field = _localValue.GetType().GetField(name);
                if (field!=null)
                    return field.GetValue(_localValue);
            }

            return proceed();
        }

        protected void SetLocal(object self, object value) {
            dynamic dself = self;
            if (value == null) return;

            // Strings love to pretend to be arrays
            if (typeof(String).IsInstanceOfType(value)) {
                dself.Supertype("Value");
                dself.Stereotype("String");
                _localValue = value;
            }
            else if (typeof(IDictionary).IsInstanceOfType(value)) {
                dself.Supertype("Object");
                dself.Stereotype("Dictionary");
                dself.Meld(value);
            }
            else if (typeof(IEnumerable).IsInstanceOfType(value)) {
                dself.AddRange(value);
            }
            else if (value.GetType().IsPrimitive || value.GetType().IsValueType) {
                dself.Supertype("Value");
                dself.Stereotype(value.GetType().Name);
                _localValue = value;
            }
            else {
                dself.Meld(value);
                dself.Supertype("Object");

                // From StackOverflow:
                // HACK: The only way to detect anonymous types right now.
                var type = value.GetType();
                if (Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                  && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                  && type.Name.Contains("AnonymousType")
                  && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic) {
                    dself.Stereotype("Anonymous");
                }
                else {
                    dself.Stereotype(type.Name);
                }
            }
        }

        /// <summary>
        /// Causes any setted properties to get wrapped in a stuff of their own
        /// </summary>
        /// <returns>Stuff</returns>
        public override object SetMember(Func<object> proceed, object self, string name, object value) {
            // Always delegate to SetIndex
            return ((dynamic)self)[name] = value;
        }
        
        public override object SetIndex(Func<object> proceed, object self, IEnumerable<object> keys, object value) {
            if (value == null) {
                return ((dynamic)self)[keys.First().ToString()] = new Stuff();
            }
            // Allow stuff to be set directly
            if (typeof(Stuff).IsInstanceOfType(value)) {
                ((dynamic)self).Supertype("Object").Stereotype("Stuff");
                return proceed();
            }
            // Otherwise wrap a new stuff
            if (keys.Count() == 1) {
                return ((dynamic)self)[keys.First().ToString()] = new Stuff(value);
            }
            return proceed();
        }
    }
}
