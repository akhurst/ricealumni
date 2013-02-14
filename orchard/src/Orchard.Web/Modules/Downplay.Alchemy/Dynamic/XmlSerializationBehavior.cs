using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClaySharp;
using System.Xml.Linq;

namespace Downplay.Alchemy.Dynamic {
    public class XmlSerializationBehavior : ClayBehavior {

        public override object InvokeMember(Func<object> proceed, object self, string name, INamedEnumerable<object> args) {

            if (name == "Xml") {
                if (args.Positional.Count() == 0) {
                    dynamic dself = self;
                    // Build an XElement
                    var x = new XElement(
                        (string)dself.Stereotype(),
                        XContents(dself)
                    );
                    return x;
                }
                else {
                    // Deserialize
                    foreach (var p in args.Positional) {
                        if (p is string) {
                            var x = XElement.Parse((string)p);
                            MeldXml(self,x);
                        }
                        else if (p is XElement) {
                            MeldXml(self, (XElement)p);
                        }

                    }
                    return self;
                }

            }
            return proceed();

        }

        /// <summary>
        /// Generate XML node contents from a dynamic
        /// </summary>
        /// <param name="dself"></param>
        /// <returns></returns>
        private static IEnumerable<XObject> XContents(dynamic dself) {

            string supertype = dself.Supertype();

            switch (supertype) {

                case "Array":
                    foreach (dynamic item in dself) {
                        yield return item.Value.Xml();
                    }
                    break;

                case "Value":
                    yield return new XText(dself.Get().ToString());
                    break;

                case "Object":

                    // Get members
                    var values = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

                    // Get the value for each member in the dynamic object
                    IClayBehaviorProvider clay = dself as IClayBehaviorProvider;
                    clay.Behavior.GetMembers(() => null, clay, values);

                    foreach (KeyValuePair<string, object> item in values) {

                        if (item.Key == "Metadata") continue;
                        if (((dynamic)item.Value).Metadata.XmlAttribute == true) {
                            yield return new XAttribute(item.Key, item.Value);
                        }
                        else {
                            dynamic ditem = item.Value;
                            yield return new XElement(item.Key, ((IEnumerable<XObject>)XContents(ditem)).ToArray());
                        }

                    }
                    break;

                case "Null":
                    yield break;

            }
        }

        private static void MeldXml(dynamic self, XElement x) {

            self.Supertype(x.Name);
            foreach (var attribute in x.Attributes()) {
                self[attribute.Name.ToString()] = attribute.Value;
                self[attribute.Name.ToString()].Metadata.XmlAttribute = true;
            }
            foreach (var node in x.Elements()) {
                if (node == null) continue;
                self[node.Name.ToString()].Xml(node);
            }

        }

    }
}
