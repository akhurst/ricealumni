using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard;
using Orchard.Mvc.Filters;

namespace RiceAlumni.Theme
{
	public class HomepageLayoutFilter : FilterProvider, IResultFilter
	{
		private readonly IWorkContextAccessor _wca;

		public HomepageLayoutFilter(IWorkContextAccessor wca)
		{
			_wca = wca;
		}

		public void OnResultExecuting(ResultExecutingContext filterContext)
		{
			var workContext = _wca.GetContext();

			if (workContext.HttpContext.Request.AppRelativeCurrentExecutionFilePath=="~/")
				workContext.Layout.IsHomepage = true;
			else
				workContext.Layout.IsHomepage = false;
		}

		public void OnResultExecuted(ResultExecutedContext filterContext)
		{
		}
	}
}