using System;
using System.Web.Mvc;
using Orchard.Themes;
using Orchard.ContentManagement;
using RiceAlumni.Events.Models;
using System.Linq;

namespace RiceAlumni.Events.Controllers
{
    [Themed]
    public class CalendarController : Controller
    {
        IContentManager _contentManager;

        public CalendarController(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetEvents(DateTime from, DateTime to)
        {
            var events = _contentManager.Query<EventPart>(VersionOptions.AllVersions).Where<EventPartRecord>(eventRecord => eventRecord.StartDate >= from && eventRecord.StartDate <= to).List();

            var eventDtos = events.Select(x => new EventDTO(x));

            return Json(eventDtos, JsonRequestBehavior.AllowGet);
        }
    }
}