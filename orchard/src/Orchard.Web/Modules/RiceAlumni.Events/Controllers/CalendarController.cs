using System;
using System.Web.Mvc;
using Orchard.Themes;
using Orchard.ContentManagement;
using RiceAlumni.Events.Models;
using RiceAlumni.Events.Models.DTOs;
using System.Linq;
using System.Diagnostics;
using Orchard.Core.Common.Models;

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

            var events = _contentManager.Query(VersionOptions.Published, "Event")
                .Where<EventPartRecord>(eventRecord => eventRecord.StartDate >= from && eventRecord.StartDate <= to).List();
            var eventDtos = events.Select<ContentItem, Event>(x =>
            {
                var eventPart = x.Parts.First(part => part is EventPart) as EventPart;
                var locationPart = x.Parts.First(part => part is LocationPart) as LocationPart;
                var bodyPart = x.Parts.First(part => part is BodyPart) as BodyPart;

                var riceEvent = new Event(eventPart.Record, locationPart.Record);
                riceEvent.Details.Description = bodyPart.Record.Text;

                return riceEvent;
            });

            return Json(eventDtos, JsonRequestBehavior.AllowGet);
        }
    }
}