using Orchard.ContentManagement.Drivers;
using RiceAlumni.Events.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiceAlumni.Events.Drivers
{
    public class EventPartDriver : ContentPartDriver<EventPart>
    {
        protected override string Prefix
        {
            get { return "EventPart"; }
        }

        protected override DriverResult Display(EventPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_Event", () => shapeHelper.Parts_Event(EventPart: part));
        }

        protected override DriverResult Editor(EventPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Event_Edit", () => shapeHelper.EditorTemplate(TemplateName: "Parts/Event", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(EventPart part, Orchard.ContentManagement.IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}