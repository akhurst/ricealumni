using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace RiceAlumni.Events
{
	public class Migrations : DataMigrationImpl
	{
//		public int Create()
//		{
//			SchemaBuilder.CreateTable("EventPartRecord", table => table
//				.ContentPartRecord()
//				.Column<string>("LinkText")
//				.Column<string>("LinkTarget")
//				.Column<DateTime>("StartDate")
//				.Column<DateTime>("EndDate")
//				.Column<int>("PciEventId")
//			);
//
//			SchemaBuilder.CreateTable("LocationPartRecord", table => table
//				.ContentPartRecord()
//				.Column<string>("Name")
//				.Column<string>("Building")
//				.Column<string>("Room")
//				.Column<string>("Address1")
//				.Column<string>("Address2")
//				.Column<string>("City")
//				.Column<string>("State")
//				.Column<string>("Zip")
//				.Column<string>("MapUrl")
//				.Column<double>("Latitude")
//				.Column<double>("Longitude")
//			);
//
//			ContentDefinitionManager.AlterPartDefinition("HomepageSecondaryImage", part => part
//				.WithField("Image", field => field
//					.OfType("ImageField")
//					.WithSetting("ImageFieldSettings.Width", "691")
//					.WithSetting("ImageFieldSettings.Height", "95")
//					.WithSetting("ImageFieldSettings.ResizeAction", "Validate")
//				)
//			);
//
//			ContentDefinitionManager.AlterTypeDefinition("HomepageLink", builder => builder
//				.Draftable()
//				.WithPart("WidgetPart")
//				.WithPart("CommonPart")
//				.WithPart("BodyPart", partBuilder => partBuilder.WithSetting("BodyTypePartSettings.Flavor", "text"))
//				.WithPart("HomepageSecondaryImage")
//				.WithPart("HomepagePart")
//				.WithPart("AdminMenuPart")
//				.WithSetting("Stereotype", "Widget")
//			);
//
//			ContentDefinitionManager.AlterPartDefinition("HomepagePrimaryImage", part => part
//				.WithField("Image", field => field
//					.OfType("ImageField")
//					.WithSetting("ImageFieldSettings.Width", "1980")
//					.WithSetting("ImageFieldSettings.Height", "650")
//					.WithSetting("ImageFieldSettings.ResizeAction", "Validate")
//				)
//			);
//
//			ContentDefinitionManager.AlterTypeDefinition("HomepageSlide", builder => builder
//				.Draftable()
//				.WithPart("WidgetPart")
//				.WithPart("CommonPart")
//				.WithPart("BodyPart", partBuilder => partBuilder.WithSetting("BodyTypePartSettings.Flavor", "text"))
//				.WithPart("HomepagePrimaryImage")
//				.WithPart("HomepagePart")
//				.WithPart("AdminMenuPart")
//				.WithSetting("Stereotype", "Widget")
//			);
//
//			return 1;
//		}
	}
}