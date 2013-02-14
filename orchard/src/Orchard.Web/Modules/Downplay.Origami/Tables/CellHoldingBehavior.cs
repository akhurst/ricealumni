using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClaySharp;
using ClaySharp.Behaviors;

namespace Downplay.Origami.Tables {

    /// <summary>
    /// Clay behavior for building table row cells
    /// 
    /// Works like zone holding; when you access a property it creates a cell
    /// </summary>
    public class CellHoldingBehavior : ClayBehavior{

        private readonly Func<dynamic> _cellFactory;

        public CellHoldingBehavior(Func<dynamic> cellFactory) {
            _cellFactory = cellFactory;
        }

        public override object GetMember(Func<object> proceed, object self, string name) {
            if (name == "Cells") {
                // provide a robot for zone manipulation on parent object
                return ClayActivator.CreateInstance(new IClayBehavior[] {                
                    new InterfaceProxyBehavior(),
                    new CellsBehavior(_cellFactory, self) 
                });
            }

            // Check for other result
            var result = proceed();

            // No result exists so create a zone
            if (((dynamic)result) == null) {

                // substitute nil results with a robot that turns adds a zone on
                // the parent when .Add is invoked
                return ClayActivator.CreateInstance(new IClayBehavior[] { 
                    new InterfaceProxyBehavior(),
                    new NilBehavior(),
                    new CellOnDemandBehavior(_cellFactory, self, name) 
                });
            }
            return result;
        }

        public class CellsBehavior : ClayBehavior {
            private readonly Func<dynamic> _cellFactory;
            private readonly object _parent;

            public CellsBehavior(Func<dynamic> cellFactory, object parent) {
                _cellFactory = cellFactory;
                _parent = parent;
            }

            public override object GetMember(Func<object> proceed, object self, string name) {
                var parentMember = ((dynamic)_parent)[name];
                if (parentMember == null) {
                    return ClayActivator.CreateInstance(new IClayBehavior[] { 
                        new InterfaceProxyBehavior(),
                        new NilBehavior(),
                        new CellOnDemandBehavior(_cellFactory, _parent, name) 
                    });
                }
                return parentMember;
            }
            public override object GetIndex(Func<object> proceed, object self, System.Collections.Generic.IEnumerable<object> keys) {
                if (keys.Count() == 1) {
                    return GetMember(proceed, null, System.Convert.ToString(keys.Single()));
                }
                return proceed();
            }
        }

        public class CellOnDemandBehavior : ClayBehavior {
            private readonly Func<dynamic> _zoneFactory;
            private readonly object _parent;
            private readonly string _potentialZoneName;

            public CellOnDemandBehavior(Func<dynamic> zoneFactory, object parent, string potentialZoneName) {
                _zoneFactory = zoneFactory;
                _parent = parent;
                _potentialZoneName = potentialZoneName;
            }

            public override object InvokeMember(Func<object> proceed, object self, string name, INamedEnumerable<object> args) {
                var argsCount = args.Count();
                if (name == "Add" && (argsCount == 1 || argsCount == 2)) {
                    // pszmyd: Ignore null shapes
                    if (args.First() == null)
                        return _parent;

                    dynamic parent = _parent;

                    dynamic zone = _zoneFactory();
                    zone.Parent = _parent;
                    zone.ZoneName = _potentialZoneName;
                    parent[_potentialZoneName] = zone;

                    if (argsCount == 1)
                        return zone.Add(args.Single());

                    return zone.Add(args.First(), (string)args.Last());
                }
                return proceed();
            }
        }
 

    }
}