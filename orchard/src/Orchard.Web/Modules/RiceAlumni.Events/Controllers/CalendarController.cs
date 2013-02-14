using System;
using System.Web.Mvc;
using Orchard.Themes;
using Orchard.ContentManagement;
using RiceAlumni.Events.Models;
using RiceAlumni.Events.Models.DTOs;
using System.Linq;
using System.Diagnostics;

namespace RiceAlumni.Events.Controllers
{
    [Themed]
    public class CalendarController : Controller
    {
        IContentManager _contentManager;
        IContentQuery _contentQuery;

        public CalendarController(IContentManager contentManager, IContentQuery contentQuery)
        {
            _contentManager = contentManager;
            _contentQuery = contentQuery;
        }

        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetEvents(DateTime from, DateTime to)
        {

            var events = _contentManager.Query(VersionOptions.AllVersions, "Event")
                .Where<EventPartRecord>(eventRecord => eventRecord.StartDate >= from && eventRecord.StartDate <= to).List();
            var eventDtos = events.Select<ContentItem, Event>(x =>
            {
                var eventPart = x.Parts.First(part => part is EventPart) as EventPart;
                var locationPart = x.Parts.First(part => part is LocationPart) as LocationPart;

                return new Event(eventPart.Record, locationPart.Record);
            });

            return Json(eventDtos, JsonRequestBehavior.AllowGet);
        }
    }
}