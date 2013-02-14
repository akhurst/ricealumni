using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace Downplay.Mechanics.Paperclips
{
    [OrchardFeature("Downplay.Mechanics.Paperclips")]
    public class Migrations : DataMigrationImpl
    {

        public int Create()
        {
            // Create paperclip part record
            SchemaBuilder.CreateTable("PaperclipPartRecord", table => table
                .ContentPartRecord()
                .Column<String>("Placement")
                .Column<String>("DisplayType"));
            // Make part attachable
            ContentDefinitionManager.AlterPartDefinition("PaperclipPart", part => part
                .Attachable());

            // Give sockets to Site
            ContentDefinitionManager.AlterTypeDefinition("Site", cfg => cfg
                .WithPart("SocketsPart"));
            // And to user
            ContentDefinitionManager.AlterTypeDefinition("User", cfg => cfg
                .WithPart("SocketsPart"));

            // Create Paperclip content type
    /* TODO: Downplay.Theory.Bookmarks will perform this role       
     * 
     * ContentDefinitionManager.AlterTypeDefinition("Paperclip", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("ConnectorPart")
                .WithPart("PaperclipPart")
                .WithSetting("Stereotype", "Content"));
            */
            return 1;
        }

    }
}