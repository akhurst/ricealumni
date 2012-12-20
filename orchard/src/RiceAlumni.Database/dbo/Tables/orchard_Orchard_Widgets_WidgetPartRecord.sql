CREATE TABLE [dbo].[orchard_Orchard_Widgets_WidgetPartRecord] (
    [Id]          INT            NOT NULL,
    [Title]       NVARCHAR (255) NULL,
    [Position]    NVARCHAR (255) NULL,
    [Zone]        NVARCHAR (255) NULL,
    [RenderTitle] BIT            DEFAULT ((1)) NULL,
    [Name]        NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

