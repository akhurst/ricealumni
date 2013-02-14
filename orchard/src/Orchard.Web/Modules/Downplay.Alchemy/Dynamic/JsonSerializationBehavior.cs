using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClaySharp;
using System.Web.Script.Serialization;
using System.Collections;

namespace Downplay.Alchemy.Dynamic {
    public class JsonSerializationBehavior : ClayBehavior {

        public override object InvokeMember(Func<object> proceed, object self, string name, INamedEnumerable<object> args) {

            // Call to Json function
            // Stuff.Json() -> serialize to string
            // Stuff.Json(string foo) -> meld from Json string
            if (name == "Json") {
                if (args.Positional.Count() == 0) {
                    // Serialize
                    var serializer = new JavaScriptSerializer();
                    serializer.RegisterConverters(new[] { new ClayJavaScriptConverter() });
                    dynamic dself = self;
                    return serializer.Serialize(dself.Get());
                }
                else {
                    var serializer = new JavaScriptSerializer();
                    serializer.RegisterConverters(new[] { new ClayJavaScriptConverter() });
                    dynamic stuff = self;
                    // TODO: Accept some named parameters?
                    foreach (var p in args.Positional) {
                        // Deserialize and meld each Json string
                        // TODO: Throw parameter error if p is not a string
                        stuff.Meld(serializer.Deserialize((String)p, typeof(Stuff)));
                    }
                    return self;
                }
            }

            return proceed();
        }
    }
}
