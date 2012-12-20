CREATE TABLE [dbo].[orchard_Navigation_MenuPartRecord] (
    [Id]           INT            NOT NULL,
    [MenuText]     NVARCHAR (255) NULL,
    [MenuPosition] NVARCHAR (255) NULL,
    [MenuId]       INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

