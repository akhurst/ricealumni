using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace RiceAlumni.Homepage 
{
	public class Migrations : DataMigrationImpl
	{
		public Migrations()
		{
		}

		public int Create()
		{
			SchemaBuilder.CreateTable("HomepagePartRecord", table => table
				.ContentPartRecord()
				.Column<string>("LinkText")
				.Column<string>("LinkTarget")
			);

			ContentDefinitionManager.AlterPartDefinition("HomepageSecondaryImage", part => part
				.WithField("Image", field => field
					.OfType("ImageField")
					.WithSetting("ImageFieldSettings.Width", "691")
					.WithSetting("ImageFieldSettings.Height", "95")
					.WithSetting("ImageFieldSettings.ResizeAction", "Validate")
				)
			);

			ContentDefinitionManager.AlterTypeDefinition("HomepageLink", builder => builder
				.Draftable()
				.Creatable()
				.WithPart("WidgetPart")
				.WithPart("CommonPart")
				.WithPart("BodyPart", partBuilder => partBuilder.WithSetting("BodyTypePartSettings.Flavor", "text"))
				.WithPart("HomepageSecondaryImage")
				.WithPart("HomepagePart")
				.WithPart("AdminMenuPart")
				.WithSetting("Stereotype", "Widget")
			);

			ContentDefinitionManager.AlterPartDefinition("HomepagePrimaryImage", part => part
				.WithField("Image", field => field
					.OfType("ImageField")
					.WithSetting("ImageFieldSettings.Width", "1980")
					.WithSetting("ImageFieldSettings.Height", "650")
					.WithSetting("ImageFieldSettings.ResizeAction", "Validate")
				)
			);

			ContentDefinitionManager.AlterTypeDefinition("HomepageSlide", builder => builder
				.Draftable()
				.Creatable()
				.WithPart("WidgetPart")
				.WithPart("CommonPart")
				.WithPart("BodyPart", partBuilder => partBuilder.WithSetting("BodyTypePartSettings.Flavor", "text"))
				.WithPart("HomepagePrimaryImage")
				.WithPart("HomepagePart")
				.WithPart("AdminMenuPart")
				.WithSetting("Stereotype", "Widget")
			);

			return 1;
		}
	}
}