using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard;
using Orchard.Mvc.Routes;

namespace RiceAlumni.Events {
    public class Routes : IRouteProvider {
        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

				public IEnumerable<RouteDescriptor> GetRoutes()
				{
					return new[]
					       {
						       new RouteDescriptor
						       {
										 Priority = 15,
							       Route = new Route(
								       "Calendar/{action}/{arg}",
								       new RouteValueDictionary
								       {
									       {"area", "RiceAlumni.Events"},
									       {"controller", "Calendar"},
												 {"action", "Index"},
												 {"arg", UrlParameter.Optional}
								       },
								       new RouteValueDictionary(),
								       new RouteValueDictionary
								       {
									       {"area", "RiceAlumni.Events"}
								       },
								       new MvcRouteHandler())
						       }
					       };
				}
    }
}