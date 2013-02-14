using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;
using Orchard.Environment.Configuration;

namespace Downplay.Mechanics.Plumbing {
    public class Migrations : DataMigrationImpl {

     
        // TODO: Inspect feature table to manually discover previous version number of Plumbing and run the SQL if so
        /*
                // Batch insert old form of title into Orchard 1.3 title records
                var sql = String.Format("INSERT INTO {0}Title_TitlePartRecord(Id,ContentItemRecord_id,Title) "
                                + "SELECT Id,ContentItemRecord_id,Title FROM {0}Downplay_Mechanics_TitlePartRecord",
                                _shellSettings.DataTablePrefix);
                SchemaBuilder.ExecuteSql(sql);
        */

        public int Create() {
            // This migration intentaionally left blank
            return 2;
        }
        public int UpdateFrom1() {
            ContentDefinitionManager.AlterPartDefinition("PipeRoutePart", part => part
                .Attachable(false));
            return 2;
        }

    }
}