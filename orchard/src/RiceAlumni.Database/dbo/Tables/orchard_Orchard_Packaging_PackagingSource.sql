CREATE TABLE [dbo].[orchard_Orchard_Packaging_PackagingSource] (
    [Id]        INT             IDENTITY (1, 1) NOT NULL,
    [FeedTitle] NVARCHAR (255)  NULL,
    [FeedUrl]   NVARCHAR (2048) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

