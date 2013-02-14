using System;
using System.Linq;

using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace Downplay.Theory.Cartography {
    public class Migrations : DataMigrationImpl {

        public int Create() {

            // This method contains everything and skips straight past the other migrations. Makes it easier to see the full works at once.

            // Give sockets to menu item
            ContentDefinitionManager.AlterTypeDefinition("Page", cfg => cfg
                .WithPart("SocketsPart"));

            // Menu connectors
            ContentDefinitionManager.AlterTypeDefinition("MenuChild", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("TitlePart")
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "Page")
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "Page")
                    .WithSetting("ConnectorTypePartSettings.InverseConnectorType", "MenuParent")
                    .WithSetting("ConnectorTypePartSettings.DefaultEditorParadigms", "")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Child menu items")
                    )
                .WithSetting("Stereotype", "Content"));

            ContentDefinitionManager.AlterTypeDefinition("MenuParent", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("ConnectorPart", 
                part => part
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "Page")
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "Page")
                    .WithSetting("ConnectorTypePartSettings.AllowMany", "false")
                    .WithSetting("ConnectorTypePartSettings.InverseConnectorType", "MenuChild")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Parent menus")
                    )
                .WithSetting("Stereotype","Content"));

            ContentDefinitionManager.AlterTypeDefinition("MenuRootToSite", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "Page")
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "Site")
                    .WithSetting("ConnectorTypePartSettings.InverseConnectorType", "SiteToMenuRoot")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Add to site menu")
                    )
                .WithPart("SequencePart", part => part
                    .WithSetting("SequenceTypePartSettings.EnablePaging", "false"))
                );
            
            ContentDefinitionManager.AlterTypeDefinition("SiteToMenuRoot", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("TitlePart")
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "Site")
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "Page")
                    .WithSetting("ConnectorTypePartSettings.InverseConnectorType", "MenuRootToSite")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Site menu items")
                    )
                    // Send menu to navigation when it's connected to Site
                .WithPart("PaperclipPart", part=>part
                    .WithSetting("PaperclipTypePartSettings.DefaultPlacement", "Navigation:0")
                    .WithSetting("PaperclipTypePartSettings.DefaultDisplayType", "Menu")
                    .WithSetting("PaperclipTypePartSettings.AllowChangePlacement", "true"))
                .WithPart("SequencePart",part=>part
                    .WithSetting("SequenceTypePartSettings.EnablePaging","false"))
                );

            return 7;
        }

        public int UpdateFrom1()
        {
            ContentDefinitionManager.AlterTypeDefinition("MenuRootToSite", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "Page")
                    )
                .WithPart("SequencePart", part => part
                    .WithSetting("SequenceTypePartSettings.EnablePaging", "false"))
                );

            ContentDefinitionManager.AlterTypeDefinition("SiteToMenuRoot", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "Page")
                    )
                );
            ContentDefinitionManager.AlterTypeDefinition("MenuChild", cfg => cfg
               .WithPart("ConnectorPart", part => part
                   .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "Page")
                   .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "Page")
                   ));

            ContentDefinitionManager.AlterTypeDefinition("MenuParent", cfg => cfg
                .WithPart("ConnectorPart",
                part => part
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "Page")
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "Page")
                    ));
            if (ContentDefinitionManager.ListTypeDefinitions().Any(d => d.Name == "MenuRoot"))
            {
                ContentDefinitionManager.AlterTypeDefinition("MenuRoot", cfg => cfg.Creatable(false));
            }
            return 2;
        }

        public int UpdateFrom2()
        {
            ContentDefinitionManager.AlterTypeDefinition("SiteToMenuRoot", cfg => cfg
                .WithPart("TitlePart"));
            ContentDefinitionManager.AlterTypeDefinition("MenuChild", cfg => cfg
                .WithPart("TitlePart"));
            return 3;
        }

        public int UpdateFrom3()
        {

            // Descriptions for connectors
            ContentDefinitionManager.AlterTypeDefinition("MenuChild", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Child menu items")
                    ));

            ContentDefinitionManager.AlterTypeDefinition("MenuParent", cfg => cfg
                .WithPart("ConnectorPart", 
                part => part
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Parent menus")
                    ));

            ContentDefinitionManager.AlterTypeDefinition("MenuRootToSite", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Add to site menu")
                    )
                );

            ContentDefinitionManager.AlterTypeDefinition("SiteToMenuRoot", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Site menu items")
                    ));

            return 4;
       }

        public int UpdateFrom4()
        {
            ContentDefinitionManager.AlterTypeDefinition("MenuChild", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.DefaultEditorParadigms", "")
                    ));

            return 6;
        }

        public int UpdateFrom5()
        {
            // Messed up update #1 and had to repeat some bits here
            ContentDefinitionManager.AlterTypeDefinition("MenuRootToSite", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Add to site menu")
                    )
                );

            ContentDefinitionManager.AlterTypeDefinition("SiteToMenuRoot", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Site menu items")
                    ));
            ContentDefinitionManager.AlterTypeDefinition("MenuChild", cfg => cfg
               .WithPart("ConnectorPart", part => part
                   .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "Page")
                   .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "Page")
                   ));

            ContentDefinitionManager.AlterTypeDefinition("MenuParent", cfg => cfg
                .WithPart("ConnectorPart",
                part => part
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "Page")
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "Page")
                    ));

            return 6;
        }

        public int UpdateFrom6()
        {
            // Fix MenuChild definition
            ContentDefinitionManager.AlterTypeDefinition("MenuChild", cfg => cfg
            .WithPart("ConnectorPart", part => part
                .WithSetting("ConnectorTypePartSettings.InverseConnectorType", "MenuParent")));

            return 7;
        }
        public int UpdateFrom7()
        {

            // Sequence was missing on parent/child
            ContentDefinitionManager.AlterTypeDefinition("MenuChild", cfg => cfg
                .WithPart("SequencePart")
            );
            ContentDefinitionManager.AlterTypeDefinition("MenuParent", cfg => cfg
                .WithPart("SequencePart")
            );

            return 8;
        }

        public int UpdateFrom8()
        {
            // Prevent feeds exposed from menus
            ContentDefinitionManager.AlterTypeDefinition("MenuChild", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("AggregationTypePartSettings.ExposeFeed","false")
                )
            );
            ContentDefinitionManager.AlterTypeDefinition("MenuParent", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("AggregationTypePartSettings.ExposeFeed", "false")
                    )
            );
            ContentDefinitionManager.AlterTypeDefinition("MenuRootToSite", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("AggregationTypePartSettings.ExposeFeed", "false")
                    )
                );
            ContentDefinitionManager.AlterTypeDefinition("SiteToMenuRoot", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("AggregationTypePartSettings.ExposeFeed", "false")
                    ));
            return 9;
        }
        public int UpdateFrom9() {

            ContentDefinitionManager.AlterTypeDefinition("MenuRootToSite", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.DefaultParadigms", "Navigation")
                ));            

            ContentDefinitionManager.AlterTypeDefinition("MenuChild", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.DefaultParadigms", "Navigation,NavigationChild")
                ));

            return 10;
        }

        public int UpdateFrom10() {

            ContentDefinitionManager.AlterTypeDefinition("SiteToMenuRoot", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.SocketGroupName", "Menu")
                )
            );
            return 11;
        }

        public int UpdateFrom11() {
            ContentDefinitionManager.AlterTypeDefinition("SiteToMenuRoot", cfg => cfg
                // Send menu to navigation when it's connected to Site
                    .WithPart("PaperclipPart", part => part
                        .WithSetting("PaperclipTypePartSettings.DefaultDisplayType", "Navigation")
                        ));
            return 12;
        }

        public int UpdateFrom12() {
            ContentDefinitionManager.AlterTypeDefinition("SiteToMenuRoot", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.DefaultParadigms", "Navigation")
                ));
            ContentDefinitionManager.AlterTypeDefinition("MenuRootToSite", cfg => cfg
               .WithPart("ConnectorPart", part => part
                   .WithSetting("ConnectorTypePartSettings.DefaultParadigms", "")
               ));
            return 13;
        }
        
        public int UpdateFrom13()
        {
            // Support timed publishing and versioning
            ContentDefinitionManager.AlterTypeDefinition("SiteToMenuRoot",
                cfg => cfg
                    .WithPart("PublishLaterPart")
                    .Draftable()
                    );
            ContentDefinitionManager.AlterTypeDefinition("MenuRootToSite",
                cfg => cfg
                    .WithPart("PublishLaterPart")
                    .Draftable()
                    );
            ContentDefinitionManager.AlterTypeDefinition("MenuChild",
                cfg => cfg
                    .WithPart("PublishLaterPart")
                    .Draftable()
                    );
            ContentDefinitionManager.AlterTypeDefinition("MenuParent",
                cfg => cfg
                    .WithPart("PublishLaterPart")
                    .Draftable()
                    );

            return 14;
        }
        public int UpdateFrom14() {
            ContentDefinitionManager.AlterTypeDefinition("MenuChild", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.DefaultParadigms", "Navigation,NavigationChild")
                ));
            return 15;
        }
        public int UpdateFrom15() {

            ContentDefinitionManager.AlterTypeDefinition("SiteToMenuRoot",
                cfg => cfg
                    .RemovePart("PublishLaterPart")
                    );
            ContentDefinitionManager.AlterTypeDefinition("MenuRootToSite",
                cfg => cfg
                    .RemovePart("PublishLaterPart")
                    );
            ContentDefinitionManager.AlterTypeDefinition("MenuChild",
                cfg => cfg
                    .RemovePart("PublishLaterPart")
                    );
            ContentDefinitionManager.AlterTypeDefinition("MenuParent",
                cfg => cfg
                    .RemovePart("PublishLaterPart")
                    );
            return 16;
        }

    }
}