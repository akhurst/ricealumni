using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClaySharp;
using ClaySharp.Behaviors;

namespace Downplay.Origami.Shapes {
    public class ZoneProxyBehavior : ClayBehavior {

        public IDictionary<string, Func<dynamic>> Proxies { get; set; }

        public ZoneProxyBehavior(IDictionary<string, Func<dynamic>> proxies) {
            Proxies = proxies;
        }

        public override object GetMember(Func<object> proceed, object self, string name) {

            if (name == "Zones") {
                return ClayActivator.CreateInstance(new IClayBehavior[] {                
                    new InterfaceProxyBehavior(),
                    new ZonesProxyBehavior(()=>proceed(), Proxies, self)
                });
            }
            
            // Otherwise proceed to other behaviours, including the original ZoneHoldingBehavior
            return proceed();
        }

        public class ZonesProxyBehavior : ClayBehavior {
            private Func<dynamic> _zonesActivator;
            private IDictionary<string, Func<dynamic>> _Proxies;
            private object _parent;

            public ZonesProxyBehavior(Func<dynamic> zonesActivator, IDictionary<string, Func<dynamic>> Proxies, object self) {
                _zonesActivator = zonesActivator;
                _Proxies = Proxies;
                _parent = self;
            }

            public override object GetIndex(Func<object> proceed, object self, System.Collections.Generic.IEnumerable<object> keys) {
                if (keys.Count() == 1) {

                    // Here's the new bit
                    var key = System.Convert.ToString(keys.Single());

                    // Check for the proxy symbol
                    if (key.Contains("@")) {
                        // Find the proxy!
                        var split = key.Split('@');
                        // Access the proxy shape
                        return _Proxies[split[0]]()
                            // Find the right zone on it
                            .Zones[split[1]];
                    }
                    else {
                        // Otherwise, defer to the ZonesBehavior activator, which we made available
                        // This will always return a ZoneOnDemandBehavior for the local shape
                        return _zonesActivator()[key];
                    }
                }
                return proceed();
            }

            public override object GetMember(Func<object> proceed, object self, string name) {
                // This is rarely called (shape.Zones.ZoneName - normally you'd just use shape.ZoneName)
                // But we can handle it easily also by deference to the ZonesBehavior activator
                return _zonesActivator()[name];
            }

        }


    }

}