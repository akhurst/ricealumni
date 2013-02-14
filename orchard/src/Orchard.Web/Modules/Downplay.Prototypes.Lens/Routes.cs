using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.Mvc.Routes;
using System.Web.Routing;
using System.Web.Mvc;
using Orchard.Environment.Extensions;

namespace Downplay.Prototypes.Lens {
    [OrchardFeature("Downplay.Prototypes.Lens.FallbackSearch")]
    public class Routes : IRouteProvider {
        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[]{
                new RouteDescriptor(){
                    // Extremely low priority to catch anything unmatched.
                    // There are some minor instances where a route can match and then still return 404; perhaps we could catch that with a filter instead?
                    Priority = -200,
                    Route = new Route("{*path}",
                        new RouteValueDictionary {
                            { "area", "Downplay.Alchemy" },
                            { "controller", "Display" },
                            { "action", "Index" },
                            { "id", "Search.Fallback" }
                        },                  
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "Downplay.Alchemy"}
                        },
                        new MvcRouteHandler())
                }
            };
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }
    }
}
