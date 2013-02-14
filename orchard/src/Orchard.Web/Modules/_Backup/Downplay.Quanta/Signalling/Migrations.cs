using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Downplay.Quanta.Signalling.Models;
using Orchard.Environment.Extensions;

namespace Downplay.Quanta.Signalling {
    [OrchardFeature("Downplay.Quanta.Signalling")]
    public class Migrations : DataMigrationImpl
    {

        public int Create() {
			// Creating table PublishNotificationPartRecord
			SchemaBuilder.CreateTable("PublishNotificationPartRecord", table => table
				.ContentPartRecord()
			);

            // Attachable
            ContentDefinitionManager.AlterPartDefinition(typeof(PublishNotificationPart).Name,
                cfg => cfg.Attachable());

            // Complete #1

            // Begin #2

            // Creating table PublishNotificationPartRecord
            SchemaBuilder.CreateTable("CreateNotificationPartRecord", table => table
                .ContentPartRecord()
            );

            // Attachable
            ContentDefinitionManager.AlterPartDefinition(typeof(CreateNotificationPart).Name,
                cfg => cfg.Attachable());

            // Complete #2
            return 2;
        }

        public int UpdateFrom1()
        {
            // Creating table PublishNotificationPartRecord
            SchemaBuilder.CreateTable("CreateNotificationPartRecord", table => table
                .ContentPartRecord()
            );

            // Attachable
            ContentDefinitionManager.AlterPartDefinition(typeof(CreateNotificationPart).Name,
                cfg => cfg.Attachable());

            // Complete #2
            return 2;
        }
    }
}