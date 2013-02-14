using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Mvc.Routes;
using System.Web.Routing;
using System.Web.Mvc;

namespace Downplay.Theory.Identity
{
    public class Routes : IRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            const string areaName = "Downplay.Theory.Identity";

            var emptyConstraints = new RouteValueDictionary();
            var directoryRouteValueDictionary = new RouteValueDictionary { { "area", areaName } };
            var mvcRouteHandler = new MvcRouteHandler();

            // TODO: Allow multiple directories under different route slugs
            return new[] {
                new RouteDescriptor {
                    Route = new Route(
                        "Directory",
                        new RouteValueDictionary {
                            {"area", areaName},
                            {"controller", "Directory"},
                            {"action", "Directory"}

                        },
                        emptyConstraints, directoryRouteValueDictionary, mvcRouteHandler)
                },
                new RouteDescriptor {
                    Route = new Route(
                        "Directory/PostalCodeSearch",
                        new RouteValueDictionary {
                            {"area", areaName},
                            {"controller", "Directory"},
                            {"action", "PostalCodeSearch"}

                        },
                        emptyConstraints, directoryRouteValueDictionary, mvcRouteHandler)
                }

            };
            
        }
    }
}