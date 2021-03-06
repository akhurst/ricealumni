﻿using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Roles.Services;
using RiceAlumni.Core.Extensions;

namespace RiceAlumni.Homepage
{
    public class Migrations : DataMigrationImpl
    {
        private IRoleService roleService;

        public Migrations(IRoleService roleService)
        {
            this.roleService = roleService;
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
                .WithPart("WidgetPart")
                .WithPart("CommonPart")
                .WithPart("BodyPart", partBuilder => partBuilder.WithSetting("BodyTypePartSettings.Flavor", "text"))
                .WithPart("HomepageSecondaryImage")
                .WithPart("HomepagePart")
                .WithPart("IdentityPart")
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
                .WithPart("WidgetPart")
                .WithPart("CommonPart")
                .WithPart("IdentityPart")
                .WithPart("BodyPart", partBuilder => partBuilder.WithSetting("BodyTypePartSettings.Flavor", "text"))
                .WithPart("HomepagePrimaryImage")
                .WithPart("HomepagePart")
                .WithSetting("Stereotype", "Widget")
            );

            CreateLongPartnerBanner();
            CreateSquarePartnerBanner();

            return 1;
        }

        public int UpdateFrom1()
        {
            ContentDefinitionManager.AlterTypeDefinition("LongPartnerBanner", builder => builder.Creatable(false));
            ContentDefinitionManager.AlterTypeDefinition("SquarePartnerBanner", builder => builder.Creatable(false));
            return 2;
        }

        private void CreateLongPartnerBanner()
        {
            ContentDefinitionManager.AlterPartDefinition("HomepagePartnerBannerLong", part => part
                .WithField("Image", field => field
                    .OfType("ImageField")
                    .WithSetting("ImageFieldSettings.Width", "728")
                    .WithSetting("ImageFieldSettings.Height", "90")
                    .WithSetting("ImageFieldSettings.ResizeAction", "Validate")
                )
                .WithField("LinkTarget", field => field
                    .OfType("TextField")
                    .WithDisplayName("Link Target")
                )
            );

            ContentDefinitionManager.AlterTypeDefinition("LongPartnerBanner", builder => builder
                .Draftable()
                .Creatable()
                .WithPart("WidgetPart")
                .WithPart("HomepagePartnerBannerLong")
                .WithPart("CommonPart")
                .WithPart("AdminMenuPart")
                .WithPart("IdentityPart")
                .WithSetting("Stereotype", "Widget")
            );
        }

        private void CreateSquarePartnerBanner()
        {
            ContentDefinitionManager.AlterPartDefinition("HomepagePartnerBannerSquare", part => part
                .WithField("Image", field => field
                    .OfType("ImageField")
                    .WithSetting("ImageFieldSettings.Width", "400")
                    .WithSetting("ImageFieldSettings.Height", "300")
                    .WithSetting("ImageFieldSettings.ResizeAction", "Validate")
                )
                .WithField("LinkTarget", field => field
                    .OfType("TextField")
                    .WithDisplayName("Link Target")
                )
            );

            ContentDefinitionManager.AlterTypeDefinition("SquarePartnerBanner", builder => builder
                .Draftable()
                .Creatable()
                .WithPart("WidgetPart")
                .WithPart("HomepagePartnerBannerSquare")
                .WithPart("CommonPart")
                .WithPart("AdminMenuPart")
                .WithPart("IdentityPart")
                .WithSetting("Stereotype", "Widget")
            );
        }
    }
}