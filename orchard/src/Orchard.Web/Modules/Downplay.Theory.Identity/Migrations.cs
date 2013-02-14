using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Downplay.Theory.Identity.Models;

namespace Downplay.Theory.Identity {
    public class Migrations : DataMigrationImpl {

        public int Create() {
            // Creating table AddressPartRecord
            SchemaBuilder.CreateTable("AddressPartRecord", table => table
                .ContentPartRecord()
                .Column("Address1", DbType.String)
                .Column("Address2", DbType.String)
                .Column("Address3", DbType.String)
                .Column("PostalCode", DbType.String)
                // Differentiator string for addresses
                .Column("AddressType", DbType.String, col => col.WithLength(32))
            );
            
            // Creating table TownRecord
            SchemaBuilder.CreateTable("TownPartRecord", table => table
                .ContentPartRecord()
            );
            
            // Creating table CountryRecord
            SchemaBuilder.CreateTable("CountryPartRecord", table => table
                .ContentPartRecord()
                .Column("CountryCode", DbType.String, col => col.WithLength(2))
            );

            SchemaBuilder.CreateTable("PhoneNumberPartRecord", table => table
                .ContentPartRecord()
                .Column("Number", DbType.String, col => col.WithLength(32))
                // Differentiator string for phone numbers
                .Column("NumberType", DbType.String, col => col.WithLength(32))
            );

            // Creating table RegionRecord
            /*SchemaBuilder.CreateTable("RegionRecord", table => table
                .ContentPartRecord()
                .Column("Id", DbType.Int32, column => column.PrimaryKey().Identity())
                .Column("CountryId", DbType.Int32)
                .Column("Title", DbType.String)
            );
            */
            
            // Make address attachable
            ContentDefinitionManager.AlterPartDefinition(typeof(CountryPart).Name, cfg => cfg.Attachable());
            ContentDefinitionManager.AlterPartDefinition(typeof(TownPart).Name, cfg => cfg.Attachable());
            ContentDefinitionManager.AlterPartDefinition(typeof(AddressPart).Name, cfg => cfg.Attachable());

            // Setup search widget
            ContentDefinitionManager.AlterTypeDefinition("PostalCodeSearchForm",
                cfg => cfg
                    .WithPart("PostalCodeSearchFormPart")
                    .WithPart("CommonPart")
                    .WithPart("WidgetPart")
                    .WithSetting("Stereotype", "Widget")
                );

            // User requires at least Sockets
            ContentDefinitionManager.AlterTypeDefinition("User", cfg => cfg
                .WithPart("SocketsPart"));

            // Business items
            ContentDefinitionManager.AlterTypeDefinition("Address", cfg=>cfg
                .WithPart("CommonPart")
                .WithPart("SocketsPart")
                .WithPart("AddressPart")
                );
            ContentDefinitionManager.AlterTypeDefinition("Country", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("TitlePart")
                .WithPart("PipeRoutePart", part => part
                    .WithSetting("PipeRouteTypePartSettings.RootRoute", "true")
                    .WithSetting("PipeRouteTypePartSettings.BaseRoute", "true")
                )
                .WithPart("SocketsPart")
                .WithPart("CountryPart")
                );
            ContentDefinitionManager.AlterTypeDefinition("Town", cfg => cfg
                            .WithPart("CommonPart")
                            .WithPart("TitlePart")
                            .WithPart("PipeRoutePart",part=>part
                                .WithSetting("PipeRouteTypePartSettings.RootRoute","false")
                                .WithSetting("PipeRouteTypePartSettings.BaseRoute", "false")
                            )
                            .WithPart("SocketsPart")
                            .WithPart("TownPart")
                            );
            ContentDefinitionManager.AlterTypeDefinition("PhoneNumber", cfg => cfg
                            .WithPart("CommonPart")
                            .WithPart("SocketsPart")
                            .WithPart("PhoneNumberPart")
                            );

            // Connectors
            ContentDefinitionManager.AlterTypeDefinition("AddressToTown", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "Address")
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "Town")
                    .WithSetting("ConnectorTypePartSettings.InverseConnectorType", "TownToAddress")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Town or City")
                    // Only 1 country for an address
                    .WithSetting("ConnectorTypePartSettings.AllowMany", "true")
                    // Address must have a town
                    .WithSetting("ConnectorTypePartSettings.AllowNone", "false")
                )
            );
            ContentDefinitionManager.AlterTypeDefinition("TownToAddress", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "Town")
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "Address")
                    .WithSetting("ConnectorTypePartSettings.InverseConnectorType", "AddressToTown")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Addresses in Town")
                    // Only 1 country for an address
                    .WithSetting("ConnectorTypePartSettings.AllowMany", "true")
                )
            );
            ContentDefinitionManager.AlterTypeDefinition("TownToCountry", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "Town")
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "Country")
                    .WithSetting("ConnectorTypePartSettings.InverseConnectorType", "CountryToTown")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Country")
                    // Only 1 country for an address
                    .WithSetting("ConnectorTypePartSettings.AllowMany", "true")
                    // Country must have town
                    .WithSetting("ConnectorTypePartSettings.AllowNone", "false")
                )
            );
            ContentDefinitionManager.AlterTypeDefinition("CountryToTown", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("DrillRoutePart")
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "Country")
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "Town")
                    .WithSetting("ConnectorTypePartSettings.InverseConnectorType", "TownToCountry")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Towns in Country")
                    // Only 1 country for an address
                    .WithSetting("ConnectorTypePartSettings.AllowMany", "true")
                )
            );
            ContentDefinitionManager.AlterTypeDefinition("UserToAddress", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("ConnectorPart",part => part
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "User")
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "Address")
                    .WithSetting("ConnectorTypePartSettings.InverseConnectorType", "AddressToUser")
                    // Users can have many addresses
                    .WithSetting("ConnectorTypePartSettings.AllowMany", "true")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Address(es)")
                    .WithSetting("ConnectorTypePartSettings.DefaultEditorParadigm", "AddressParadigm")
                )
            );
            ContentDefinitionManager.AlterTypeDefinition("AddressToUser", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "Address")
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "User")
                    .WithSetting("ConnectorTypePartSettings.InverseConnectorType", "UserToAddress")
                    // Many users can be at one address
                    .WithSetting("ConnectorTypePartSettings.AllowMany", "true")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "People")
                )
            );
            ContentDefinitionManager.AlterTypeDefinition("UserToPhoneNumber", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "User")
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "PhoneNumber")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Phone number(s)")
                    // Users can have many phone numbers
                    // TODO: Link phone numbers to addresses as well?
                    .WithSetting("ConnectorTypePartSettings.AllowMany", "true")
                )
            );

            return 1;
        }
        public int UpdateFrom1()
        {
            ContentDefinitionManager.AlterTypeDefinition("UserToAddress", cfg => cfg
               .WithPart("ConnectorPart", part => part
                   .WithSetting("ConnectorTypePartSettings.DefaultEditorParadigm", "AddressParadigm")
               )
           );
            ContentDefinitionManager.AlterTypeDefinition("UserToPhoneNumber", cfg => cfg
                .WithPart("ConnectorPart", part => part
                   .WithSetting("ConnectorTypePartSettings.DefaultEditorParadigm", "PhoneNumberParadigm")
                )
            );

            return 2;
        }

    }
}