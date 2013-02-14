using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Downplay.Mechanics {
    public class Migrations : DataMigrationImpl {

        public int Create() {
            // Creating table ConnectorPartRecord
			SchemaBuilder.CreateTable("ConnectorPartRecord", table => table
				.ContentPartRecord()
				.Column("LeftContentItem_id", DbType.Int32)
				.Column("RightContentItem_id", DbType.Int32)
			);
            
            // Creating table SocketsPartRecord
			SchemaBuilder.CreateTable("SocketsPartRecord", table => table
				.ContentPartRecord()
			);

            // Attachable for custom building
            ContentDefinitionManager.AlterPartDefinition("ConnectorPart", part => part.Attachable());
            ContentDefinitionManager.AlterPartDefinition("SocketsPart", part => part.Attachable());

            return 1;
        }
        public int UpdateFrom1()
        {
            // Create SequencePart table
            SchemaBuilder.CreateTable("SequencePartRecord", table => table
                .ContentPartRecord()
                .Column<int>("Sequence")
            );
            return 2;
        }
        public int UpdateFrom2()
        {
            // Make sequence usable
            ContentDefinitionManager.AlterPartDefinition("SequencePart", part => part
                .Attachable());
            return 3;
        }
        public int UpdateFrom3()
        {
            SchemaBuilder.AlterTable("ConnectorPartRecord", table => table
            .AddColumn("InverseConnector_id", DbType.Int32)
        );
            return 4;
        }

        public int UpdateFrom4()
        {
            SchemaBuilder.AlterTable("ConnectorPartRecord", table => table.CreateIndex("MechanicsConnectorLeft", "LeftContentItem_id"));
            SchemaBuilder.AlterTable("ConnectorPartRecord", table => table.CreateIndex("MechanicsConnectorRight", "RightContentItem_id"));
            return 5;
        }
        public int UpdateFrom5()
        {
            // Speed up ordering on sequence
            SchemaBuilder.AlterTable("SequencePartRecord", table => table.CreateIndex("MechanicsSequenceSort", "Sequence"));
            return 6;
        }

        public int UpdateFrom6()
        {
            // Delete-on-publish for connectors
            // TODO: Not entirely happy with this...
            SchemaBuilder.AlterTable("ConnectorPartRecord", table => table
                .AddColumn("DeleteWhenLeftPublished", DbType.Int32, column => column.WithDefault(false))
                );

            SchemaBuilder.AlterTable("ConnectorPartRecord", table => table
                .AddColumn("LeftContentVersionId", DbType.Int32, column => column.Nullable())
                );
            SchemaBuilder.AlterTable("ConnectorPartRecord", table => table
                .AddColumn("RightContentVersionId", DbType.Int32, column => column.Nullable())
                );
            SchemaBuilder.AlterTable("ConnectorPartRecord", table => table
                .AddColumn("InverseConnectorVersionId", DbType.Int32, column => column.Nullable())
                );
            SchemaBuilder.AlterTable("ConnectorPartRecord", table => table
                .AddColumn<int>("ContentItemRecord_id")
                );

            return 7;
        }

        public int UpdateFrom7() {
            SchemaBuilder.AlterTable("SocketsPartRecord", table => table
                .AddColumn<int>("ContentItemRecord_id")
                );
            return 8;
        }
    }
}