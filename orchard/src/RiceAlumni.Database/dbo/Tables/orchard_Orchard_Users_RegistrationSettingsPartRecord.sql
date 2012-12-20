CREATE TABLE [dbo].[orchard_Orchard_Users_RegistrationSettingsPartRecord] (
    [Id]                             INT            NOT NULL,
    [UsersCanRegister]               BIT            DEFAULT ((0)) NULL,
    [UsersMustValidateEmail]         BIT            DEFAULT ((0)) NULL,
    [ValidateEmailRegisteredWebsite] NVARCHAR (255) NULL,
    [ValidateEmailContactEMail]      NVARCHAR (255) NULL,
    [UsersAreModerated]              BIT            DEFAULT ((0)) NULL,
    [NotifyModeration]               BIT            DEFAULT ((0)) NULL,
    [NotificationsRecipients]        NVARCHAR (MAX) NULL,
    [EnableLostPassword]             BIT            DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

