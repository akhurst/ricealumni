CREATE TABLE [dbo].[orchard_Settings_ShellFeatureRecord] (
    [Id]                       INT            IDENTITY (1, 1) NOT NULL,
    [Name]                     NVARCHAR (255) NULL,
    [ShellDescriptorRecord_id] INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

