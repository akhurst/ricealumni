CREATE TABLE [dbo].[orchard_Orchard_Autoroute_AutoroutePartRecord] (
    [Id]                   INT             NOT NULL,
    [ContentItemRecord_id] INT             NULL,
    [CustomPattern]        NVARCHAR (2048) NULL,
    [UseCustomPattern]     BIT             DEFAULT ((0)) NULL,
    [DisplayAlias]         NVARCHAR (2048) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

