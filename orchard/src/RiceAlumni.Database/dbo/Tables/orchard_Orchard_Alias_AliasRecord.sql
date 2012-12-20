CREATE TABLE [dbo].[orchard_Orchard_Alias_AliasRecord] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Path]        NVARCHAR (2048) NULL,
    [Action_id]   INT             NULL,
    [RouteValues] NVARCHAR (MAX)  NULL,
    [Source]      NVARCHAR (256)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

