using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using RiceAlumni.Core.Extensions;
using RiceAlumni.Events.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiceAlumni.Events.Drivers
{
    public class LocationPartDriver : ContentPartDriver<LocationPart>
    {
        protected override string Prefix
        {
            get { return "LocationPart"; }
        }

        protected override DriverResult Display(LocationPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_Location", () => shapeHelper.Parts_Location(LocationPart: part));
        }

        protected override DriverResult Editor(LocationPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Location_Edit", () => shapeHelper.EditorTemplate(TemplateName: "Parts/Location", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(LocationPart part, Orchard.ContentManagement.IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }

        protected override void Importing(LocationPart part, ImportContentContext context)
        {
            this.DoImport(part, context);
        }

        protected override void Exporting(LocationPart part, ExportContentContext context)
        {
            this.DoExport(part, context);
        }
    }
}