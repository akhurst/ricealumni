CREATE TABLE [dbo].[orchard_Settings_ContentTypePartDefinitionRecord] (
    [Id]                             INT            IDENTITY (1, 1) NOT NULL,
    [Settings]                       NVARCHAR (MAX) NULL,
    [ContentPartDefinitionRecord_id] INT            NULL,
    [ContentTypeDefinitionRecord_Id] INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

