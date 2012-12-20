CREATE TABLE [dbo].[orchard_Navigation_AdminMenuPartRecord] (
    [Id]                INT            NOT NULL,
    [AdminMenuText]     NVARCHAR (255) NULL,
    [AdminMenuPosition] NVARCHAR (255) NULL,
    [OnAdminMenu]       BIT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

