CREATE TABLE [dbo].[orchard_Orchard_Framework_DataMigrationRecord] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [DataMigrationClass] NVARCHAR (255) NULL,
    [Version]            INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

