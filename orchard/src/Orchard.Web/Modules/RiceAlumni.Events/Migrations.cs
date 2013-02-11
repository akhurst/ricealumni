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
        public int Create()
        {
            SchemaBuilder.CreateTable("EventContactRecord", tableBuilder =>
                tableBuilder.ContentPartRecord()
                .Column<string>("Name")
                .Column<string>("Email")
                .Column<long>("Phone")
                );

            SchemaBuilder.CreateTable("EventPartRecord", tableBuilder =>
                tableBuilder.Column<DateTime>("StartDate")
                .ContentPartRecord()
                .Column<DateTime>("EndDate")
                .Column<string>("LinkTarget")
                .Column<string>("LinkText")
                .Column<int>("PciEvent")
                .Column<string>("Title")
                .Column<string>("Description")
                .Column<bool>("RegistrationRequired")
                .Column<int>("LocationPartRecord_Id")
                );

            return 1;
        }
    }
}