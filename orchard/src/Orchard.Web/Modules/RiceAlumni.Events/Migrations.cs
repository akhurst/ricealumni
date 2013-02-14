using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using RiceAlumni.Events.Models;

namespace RiceAlumni.Events
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("LocationPartRecord", table => table
                .ContentPartRecord()
                .Column<string>("Name")
                .Column<string>("Building")
                .Column<string>("Room")
                .Column<string>("Address1")
                .Column<string>("Address2")
                .Column<string>("City")
                .Column<string>("State")
                .Column<string>("Zip")
                .Column<string>("MapUrl")
                .Column<double>("Latitude")
                .Column<double>("Longitude")
            );

            SchemaBuilder.CreateTable("EventPartRecord", tableBuilder => tableBuilder
                .ContentPartRecord()
                .Column<DateTime>("StartDate")
                .Column<DateTime>("EndDate")
                .Column<string>("LinkTarget")
                .Column<string>("LinkText")
                .Column<int>("PciEventId")
                .Column<string>("Title")
                .Column<string>("Description")
                .Column<bool>("RegistrationRequired")
                .Column<string>("ContactEmail")
            );

            ContentDefinitionManager.AlterTypeDefinition("Event", type => type
                .Creatable()
                .WithPart("EventPart")
                .WithPart("LocationPart")
                .WithPart("CommonPart")
            );

            return 1;
        }
    }
}