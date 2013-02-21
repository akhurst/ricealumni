using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard;
using Orchard.Environment.Configuration;

namespace RiceAlumni.Core.Helpers
{
    public static class RiceUrlHelperExtensions
    {
        public static string SiteRelativeUrl(this UrlHelper urlHelper, string path, WorkContext workContext)
        {
            var shellSettings = workContext.Resolve<ShellSettings>();

            return urlHelper.Content(string.Format("~/{0}/{1}",shellSettings.RequestUrlPrefix,path.TrimStart('/','~')));
        }
    }
}