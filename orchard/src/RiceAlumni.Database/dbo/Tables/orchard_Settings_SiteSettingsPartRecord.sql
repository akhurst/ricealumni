CREATE TABLE [dbo].[orchard_Settings_SiteSettingsPartRecord] (
    [Id]                 INT            NOT NULL,
    [SiteSalt]           NVARCHAR (255) NULL,
    [SiteName]           NVARCHAR (255) NULL,
    [SuperUser]          NVARCHAR (255) NULL,
    [PageTitleSeparator] NVARCHAR (255) NULL,
    [HomePage]           NVARCHAR (255) NULL,
    [SiteCulture]        NVARCHAR (255) NULL,
    [ResourceDebugMode]  NVARCHAR (255) DEFAULT ('FromAppSetting') NULL,
    [PageSize]           INT            NULL,
    [SiteTimeZone]       NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

