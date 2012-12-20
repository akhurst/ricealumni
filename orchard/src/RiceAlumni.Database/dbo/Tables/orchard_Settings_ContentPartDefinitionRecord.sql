CREATE TABLE [dbo].[orchard_Settings_ContentPartDefinitionRecord] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (255) NULL,
    [Hidden]   BIT            NULL,
    [Settings] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

