CREATE TABLE [dbo].[orchard_Common_BodyPartRecord] (
    [Id]                   INT            NOT NULL,
    [ContentItemRecord_id] INT            NULL,
    [Text]                 NVARCHAR (MAX) NULL,
    [Format]               NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

