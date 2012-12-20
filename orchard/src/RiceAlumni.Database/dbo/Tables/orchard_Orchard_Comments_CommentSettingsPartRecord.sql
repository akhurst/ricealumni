CREATE TABLE [dbo].[orchard_Orchard_Comments_CommentSettingsPartRecord] (
    [Id]                   INT            NOT NULL,
    [ModerateComments]     BIT            NULL,
    [EnableSpamProtection] BIT            NULL,
    [AkismetKey]           NVARCHAR (255) NULL,
    [AkismetUrl]           NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

