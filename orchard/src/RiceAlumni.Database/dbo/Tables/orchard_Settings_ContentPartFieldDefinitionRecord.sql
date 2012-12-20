CREATE TABLE [dbo].[orchard_Settings_ContentPartFieldDefinitionRecord] (
    [Id]                              INT            IDENTITY (1, 1) NOT NULL,
    [Name]                            NVARCHAR (255) NULL,
    [Settings]                        NVARCHAR (MAX) NULL,
    [ContentFieldDefinitionRecord_id] INT            NULL,
    [ContentPartDefinitionRecord_Id]  INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

