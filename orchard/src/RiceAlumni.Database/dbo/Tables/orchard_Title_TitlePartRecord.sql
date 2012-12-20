CREATE TABLE [dbo].[orchard_Title_TitlePartRecord] (
    [Id]                   INT             NOT NULL,
    [ContentItemRecord_id] INT             NULL,
    [Title]                NVARCHAR (1024) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

