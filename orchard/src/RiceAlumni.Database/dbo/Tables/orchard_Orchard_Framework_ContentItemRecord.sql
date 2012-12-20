CREATE TABLE [dbo].[orchard_Orchard_Framework_ContentItemRecord] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Data]           NVARCHAR (MAX) NULL,
    [ContentType_id] INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

