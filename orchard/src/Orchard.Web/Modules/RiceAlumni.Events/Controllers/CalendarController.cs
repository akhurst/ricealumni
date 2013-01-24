using System;
using System.Web.Mvc;
using Orchard.Themes;

namespace RiceAlumni.Events.Controllers
{
	[Themed]
	public class CalendarController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public JsonResult GetEvents(DateTime arg)
		{
			var result = Json(new {Name = "My Event", Date = DateTime.Today.AddMonths(1)});
			return result;
		}
	}
}