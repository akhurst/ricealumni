CREATE TABLE [dbo].[orchard_Navigation_MenuWidgetPartRecord] (
    [Id]             INT NOT NULL,
    [StartLevel]     INT NULL,
    [Levels]         INT NULL,
    [Breadcrumb]     BIT NULL,
    [AddHomePage]    BIT NULL,
    [AddCurrentPage] BIT NULL,
    [Menu_id]        INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

