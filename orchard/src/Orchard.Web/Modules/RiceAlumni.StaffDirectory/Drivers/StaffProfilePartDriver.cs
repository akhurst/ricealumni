﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using RiceAlumni.Core.Extensions;
using RiceAlumni.StaffDirectory.Models;

namespace RiceAlumni.StaffDirectory.Drivers
{
	public class StaffProfilePartDriver: ContentPartDriver<StaffProfilePart>
	{
		protected override string Prefix
		{
			get { return "StaffProfile"; }
		}

		protected override DriverResult Display(StaffProfilePart part, string displayType, dynamic shapeHelper)
		{
			return ContentShape("Parts_StaffProfile", () => shapeHelper.Parts_StaffProfile(StaffProfilePart: part));
		}

		protected override DriverResult Editor(StaffProfilePart part, dynamic shapeHelper)
		{
			return ContentShape("Parts_StaffProfile_Edit", () => shapeHelper.EditorTemplate(TemplateName:"Parts/StaffProfile",Model:part,Prefix:Prefix));
		}

		protected override DriverResult Editor(StaffProfilePart part, Orchard.ContentManagement.IUpdateModel updater, dynamic shapeHelper)
		{
			updater.TryUpdateModel(part, Prefix, null, null);
			return Editor(part, shapeHelper);
		}

		protected override void Importing(StaffProfilePart part, ImportContentContext context)
		{
			this.DoImport(part, context);
		}

		protected override void Exporting(StaffProfilePart part, ExportContentContext context)
		{
			this.DoExport(part, context);
		}
	}
}