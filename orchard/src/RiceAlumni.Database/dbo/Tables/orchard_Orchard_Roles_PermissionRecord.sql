CREATE TABLE [dbo].[orchard_Orchard_Roles_PermissionRecord] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (255) NULL,
    [FeatureName] NVARCHAR (255) NULL,
    [Description] NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

