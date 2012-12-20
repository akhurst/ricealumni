CREATE TABLE [dbo].[orchard_Common_CommonPartRecord] (
    [Id]           INT      NOT NULL,
    [OwnerId]      INT      NULL,
    [CreatedUtc]   DATETIME NULL,
    [PublishedUtc] DATETIME NULL,
    [ModifiedUtc]  DATETIME NULL,
    [Container_id] INT      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

