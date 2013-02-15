using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using RiceAlumni.Core.Extensions;
using RiceAlumni.Homepage.Models;

namespace RiceAlumni.Homepage.Drivers
{
	public class HomepagePartDriver : ContentPartDriver<HomepagePart>
	{
		protected override string Prefix
		{
			get { return "HomepagePart"; }
		}

		protected override DriverResult Display(HomepagePart part, string displayType, dynamic shapeHelper)
		{
			return ContentShape("Parts_Homepage", () => shapeHelper.Parts_Homepage(HomepagePart: part));
		}

		protected override DriverResult Editor(HomepagePart part, dynamic shapeHelper)
		{
			return ContentShape("Parts_Homepage_Edit", () => shapeHelper.EditorTemplate(TemplateName: "Parts/Homepage", Model: part, Prefix: Prefix));
		}

		protected override DriverResult Editor(HomepagePart part, Orchard.ContentManagement.IUpdateModel updater, dynamic shapeHelper)
		{
			updater.TryUpdateModel(part, Prefix, null, null);
			return Editor(part, shapeHelper);
		}

		protected override void Importing(HomepagePart part, ImportContentContext context)
		{
			this.DoImport(part, context);
		}

		protected override void Exporting(HomepagePart part, ExportContentContext context)
		{
			this.DoExport(part, context);
		}

	}
}