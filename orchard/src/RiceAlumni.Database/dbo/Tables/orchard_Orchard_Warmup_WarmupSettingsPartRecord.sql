CREATE TABLE [dbo].[orchard_Orchard_Warmup_WarmupSettingsPartRecord] (
    [Id]        INT            NOT NULL,
    [Urls]      NVARCHAR (MAX) NULL,
    [Scheduled] BIT            NULL,
    [Delay]     INT            NULL,
    [OnPublish] BIT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

