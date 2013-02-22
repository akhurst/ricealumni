using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard;
using Orchard.Mvc.Routes;

namespace RiceAlumni.MainSite {
    public class Routes : IRouteProvider
    {
        private ILegacyContentConstraint legacyContentConstraint;

        public Routes(ILegacyContentConstraint legacyContentConstraint)
        {
            this.legacyContentConstraint = legacyContentConstraint;
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {

            return new[]
                   {
                       new RouteDescriptor
                       {
                           Route = new Route(
                               "{*path}",
                               new RouteValueDictionary
                               {
                                   {"area", "RiceAlumni.MainSite"},
                                   {"controller", "LegacyContent"},
                                   {"action", "RenderLegacyContent"}
                               },
                               new RouteValueDictionary
                               {
                                   {"path", legacyContentConstraint},
                               },
                               new RouteValueDictionary
                               {
                                   {"area", "RiceAlumni.MainSite"}
                               },
                               new MvcRouteHandler())
                       }
                   };
        }
    }

    public interface ILegacyContentConstraint : IRouteConstraint, ISingletonDependency
	{}
	public class LegacyContentConstraint : ILegacyContentConstraint
	{
		public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
		{
            object value;
            if (values.TryGetValue(parameterName, out value))
            {
	            var parameterValue = Convert.ToString(value);

	            return FindPath(httpContext,parameterValue)!=null;
            }
            else
            {
	            return false;
            }
		}

		public static string FindPath(HttpContextBase httpContext, string fileName)
		{
			string fullFileName = fileName;

			var splitPath = fullFileName.Split('/');

			if (splitPath.Length > 0 && !splitPath[splitPath.Length - 1].Contains("."))
			{
				fullFileName = fullFileName + "/index.html";
			}

			string filePath = httpContext.Request.MapPath("~/LegacyContent/" + fullFileName);

			if( File.Exists(filePath))
				return filePath;
			else
				return null;
		}

		/*
			var theme = httpContext.Request.RequestContext.GetWorkContext().CurrentTheme;
			var theme = orchardServices.WorkContext.CurrentTheme;
			UrlHelper url = new UrlHelper(httpContext.Request.RequestContext);
			var path = theme.Location + "/" + theme.Id + "/LegacyContent/" + fileName;
			var urlPath = url.Content(path);
			string filePath = httpContext.Request.MapPath(path);
			if( File.Exists(filePath))
				return filePath;
			else
				return null;
		 */
	}
}