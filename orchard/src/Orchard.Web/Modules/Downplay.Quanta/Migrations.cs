using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace Downplay.Quanta
{
    [OrchardFeature("Downplay.Quanta.Effectors")]
    public class Migrations : DataMigrationImpl
    {

        public int Create()
        {
            // TODO: EffectiveRolesPart needs to check the added roles and make sure the user is *allowed* to apply role (x) to content (y). So any user
            // can apply effective roles on content they own but not other peoples content!
            
            // Creating table PermissionsPartRecord
            SchemaBuilder.CreateTable("EffectiveRolesPartRecord", table => table
                .ContentPartRecord()
                .Column<String>("EffectiveRoles")
            );
            // Make attachable
            ContentDefinitionManager.AlterPartDefinition("EffectiveRolesPart",part=>part.Attachable());

            // Ensure user has Sockets part to participate in relationships
            ContentDefinitionManager.AlterTypeDefinition("User", cfg=>cfg
                .WithPart("SocketsPart"));

            ContentDefinitionManager.AlterTypeDefinition("UserToEffectingContent", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("ConnectorPart", part => part
                    // Link from user on the left
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "User")
                    // To anything on the right
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "")
                    .WithSetting("ConnectorTypePartSettings.InverseConnectorType", "ContentToEffectingUser")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Roles for items")
                    ));

            ContentDefinitionManager.AlterTypeDefinition("ContentToEffectingUser", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("ConnectorPart", part => part
                    // Link from user on the left
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "")
                    // To anything on the right
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "User")
                    .WithSetting("ConnectorTypePartSettings.InverseConnectorType", "UserToEffectingContent")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Users with roles for this Item")
                    )
                // Need the roles part
                .WithPart("EffectiveRolesPart"));

            // Create Group type
            // TODO: Should move to separate feature
            ContentDefinitionManager.AlterTypeDefinition("Group", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("SocketsPart")
                // Really need our own part to keep title separate and handle view permissions
                .WithPart("RoutePart")
                .Creatable()
                );

            // Group connectors
            ContentDefinitionManager.AlterTypeDefinition("GroupToMemberUser", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("ConnectorPart", part => part
                    // Link from user on the left
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "Group")
                    // To anything on the right
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "User")
                    .WithSetting("ConnectorTypePartSettings.InverseConnectorType", "UserToGroupMembership")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Users with group membership")
                    )
                // What roles the user has in a group
                .WithPart("EffectiveRolesPart"));

            ContentDefinitionManager.AlterTypeDefinition("UserToGroupMembership", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("ConnectorPart", part => part
                    // Link from user on the left
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "User")
                    // To anything on the right
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "Group")
                    .WithSetting("ConnectorTypePartSettings.InverseConnectorType", "GroupToMemberUser")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Membership in groups")
                    )
                    );

            ContentDefinitionManager.AlterTypeDefinition("GroupToGroupContent", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("ConnectorPart", part => part
                    // Link from user on the left
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "Group")
                    // To anything on the right
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "")
                    .WithSetting("ConnectorTypePartSettings.InverseConnectorType", "GroupContentToGroup")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Group Content")
                    )
                // What roles group members have over the content
                .WithPart("EffectiveRolesPart"));
            ContentDefinitionManager.AlterTypeDefinition("GroupContentToGroup", cfg => cfg
                .WithPart("CommonPart")
                .WithPart("ConnectorPart", part => part
                    // Link from user on the left
                    .WithSetting("ConnectorTypePartSettings.AllowedContentLeft", "")
                    // To anything on the right
                    .WithSetting("ConnectorTypePartSettings.AllowedContentRight", "Group")
                    .WithSetting("ConnectorTypePartSettings.InverseConnectorType", "GroupToGroupContent")
                    .WithSetting("ConnectorTypePartSettings.SocketDisplayName", "Belongs to Groups")
                    )
                    );

            return 1;
        }
    }
}