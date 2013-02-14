using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Environment.Extensions;
using Orchard.UI.Zones;
using ClaySharp.Implementation;
using Orchard.ContentManagement.Handlers;
using Orchard;

namespace Downplay.Origami.Shapes {

    [OrchardFeature("Downplay.Origami.ZoneProxy")]
    public class ZoneProxyShapes : IShapeTableProvider {
        private readonly IWorkContextAccessor _workContextAccessor;
        public ZoneProxyShapes(IWorkContextAccessor workContextAccessor) {
            _workContextAccessor = workContextAccessor;
        }

        public void Discover(ShapeTableBuilder builder) {

            builder.Describe("Content")
                .OnCreating(creating => {
                    creating.Behaviors.Add(
                        new ZoneProxyBehavior(new Dictionary<string, Func<dynamic>> { { "Layout", () => _workContextAccessor.GetContext().Layout } })
                        );
                });

        }

    }

}
