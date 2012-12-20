CREATE TABLE [dbo].[orchard_Orchard_Widgets_LayerPartRecord] (
    [Id]          INT            NOT NULL,
    [Name]        NVARCHAR (255) NULL,
    [Description] NVARCHAR (MAX) NULL,
    [LayerRule]   NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

