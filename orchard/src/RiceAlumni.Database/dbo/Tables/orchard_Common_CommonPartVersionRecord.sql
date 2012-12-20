CREATE TABLE [dbo].[orchard_Common_CommonPartVersionRecord] (
    [Id]                   INT      NOT NULL,
    [ContentItemRecord_id] INT      NULL,
    [CreatedUtc]           DATETIME NULL,
    [PublishedUtc]         DATETIME NULL,
    [ModifiedUtc]          DATETIME NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

