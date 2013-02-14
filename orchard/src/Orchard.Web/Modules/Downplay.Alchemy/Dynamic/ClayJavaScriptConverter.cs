using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Diagnostics;
using ClaySharp;
using System.Collections;

namespace Downplay.Alchemy.Dynamic {
    /// <summary>
    /// Copied from an internal MVC class. Unfortunately that implementation didn't support deserialization. We'll have to fix that :)
    /// </summary>
    public class ClayJavaScriptConverter : JavaScriptConverter {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer) {
            dynamic stuff = new Stuff();
            foreach (var kv in dictionary) {
                stuff[kv.Key] = kv.Value;
            }
            return stuff;
        }
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer) {
            var values = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            
            // Get the value for each member in the dynamic object
            var clay = obj as IClayBehaviorProvider;
            clay.Behavior.GetMembers(() => null, clay, values);
            return values.ToDictionary(kv => kv.Key, kv => (object)((dynamic)kv.Value).Get());
        }

        public override IEnumerable<Type> SupportedTypes {
            get {
                yield return typeof(Stuff);
            }
        }

    }
}
