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

namespace Downplay.Theory.Cartography.Topology {
    [OrchardFeature("Downplay.Theory.Cartography.Topology")]
    public class Migrations : DataMigrationImpl {

        public int Create() {

            // Give sockets to relevant item
            ContentDefinitionManager.AlterTypeDefinition("Page", cfg => cfg
                .WithPart("SocketsPart"));
            ContentDefinitionManager.AlterTypeDefinition("BlogPost", cfg => cfg
                .WithPart("SocketsPart"));
            ContentDefinitionManager.AlterTypeDefinition("Topic", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("TitlePart")
                .WithPart("BodyPart")
                .WithPart("SocketsPart")
                .Creatable()
                );

            // Menu connectors
            ContentDefinitionManager.AlterTypeDefinition("TopicToContent", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "Topic")
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "Page,BlogPost")
                    .WithSetting("ConnectorTypePartSettings.InverseConnectorType", "ContentToTopic")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Items in this topic")
                    )
                .WithSetting("Stereotype", "Content"));
            ContentDefinitionManager.AlterTypeDefinition("ContentToTopic", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "Topic")
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "Page,BlogPost")
                    .WithSetting("ConnectorTypePartSettings.InverseConnectorType", "TopicToContent")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Topics")
                    )
                .WithSetting("Stereotype", "Content"));

            ContentDefinitionManager.AlterTypeDefinition("TopicToSite", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "Topic")
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "Site")
                    .WithSetting("ConnectorTypePartSettings.InverseConnectorType", "SiteToTopic")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Add to menu")
                    )
                .WithPart("SequencePart", part => part
                    .WithSetting("SequenceTypePartSettings.EnablePaging", "false"))
                );
            
            ContentDefinitionManager.AlterTypeDefinition("SiteToTopic", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "Site")
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "Page")
                    .WithSetting("ConnectorTypePartSettings.InverseConnectorType", "MenuRootToSite")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Topics menu")
                    )
                    // Send topic to AsideFirst when it's connected to Site
                .WithPart("PaperclipPart", part=>part
                    .WithSetting("PaperclipTypePartSettings.DefaultPlacement", "AsideFirst:5")
                    .WithSetting("PaperclipTypePartSettings.DefaultDisplayType", "Menu")
                    .WithSetting("PaperclipTypePartSettings.AllowChangePlacement", "false"))
                .WithPart("SequencePart",part=>part
                    .WithSetting("SequenceTypePartSettings.EnablePaging","false"))
                );

            return 1;
        }
        public int UpdateFrom1()
        {
            ContentDefinitionManager.AlterTypeDefinition("Topic", cfg => cfg
                .WithPart("PipeRoutePart",part=>part
                    .WithSetting("PipeRouteTypePartSettings.BaseRoute", "true")
                    .WithSetting("PipeRouteTypePartSettings.RootRoute", "true")
                    .WithSetting("PipeRouteTypePartSettings.CustomBaseRoute", "topics")
                    .WithSetting("PipeRouteTypePartSettings.ContentTypeBaseRoute", "false")
                    ));
           return 2;
        }

        public int UpdateFrom2()
        {

            ContentDefinitionManager.AlterTypeDefinition("ContentToTopic", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.ConnectorDisplayType", "Link")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Categories")
                    )
                );
            ContentDefinitionManager.AlterTypeDefinition("SiteToTopic", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Categories")
                    )
                );

            return 3;
        }
        public int UpdateFrom3()
        {
            // Prevent exposing feeds from anywhere except Topic->Content
            ContentDefinitionManager.AlterTypeDefinition("SiteToTopic", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("AggregationTypePartSettings.ExposeFeed", "false")
                    )
                );
            ContentDefinitionManager.AlterTypeDefinition("TopicToSite", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("AggregationTypePartSettings.ExposeFeed", "false")
                    )
                );
            ContentDefinitionManager.AlterTypeDefinition("ContentToTopic", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("AggregationTypePartSettings.ExposeFeed", "false")
                    )
                );

            return 4;
        }

        public int UpdateFrom4() {
            ContentDefinitionManager.AlterTypeDefinition("ContentToTopic", cfg => cfg
                .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.DefaultParadigms", "Links")
                    ));
             ContentDefinitionManager.AlterTypeDefinition("SiteToTopic", cfg => cfg
                 .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.DefaultParadigms", "Navigation")
                    ));
             ContentDefinitionManager.AlterTypeDefinition("TopicToContent", cfg => cfg
                 .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.DefaultParadigms", "List")
                    ));
             ContentDefinitionManager.AlterTypeDefinition("TopicToSite", cfg => cfg
                 .WithPart("ConnectorPart", part => part
                    .WithSetting("ConnectorTypePartSettings.DefaultParadigms", "NavigationParent")
                    ));
            return 5;
       }

        public int UpdateFrom5() {

            ContentDefinitionManager.AlterTypeDefinition("SiteToTopic", cfg => cfg
                .WithPart("ConnectorPart", part => part
                   .WithSetting("ConnectorTypePartSettings.SocketGroupName", "Menu")
                   ));

            return 6;
        }
   }
}