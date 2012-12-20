CREATE TABLE [dbo].[orchard_Orchard_Users_UserPartRecord] (
    [Id]                  INT            NOT NULL,
    [UserName]            NVARCHAR (255) NULL,
    [Email]               NVARCHAR (255) NULL,
    [NormalizedUserName]  NVARCHAR (255) NULL,
    [Password]            NVARCHAR (255) NULL,
    [PasswordFormat]      NVARCHAR (255) NULL,
    [HashAlgorithm]       NVARCHAR (255) NULL,
    [PasswordSalt]        NVARCHAR (255) NULL,
    [RegistrationStatus]  NVARCHAR (255) DEFAULT ('Approved') NULL,
    [EmailStatus]         NVARCHAR (255) DEFAULT ('Approved') NULL,
    [EmailChallengeToken] NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

