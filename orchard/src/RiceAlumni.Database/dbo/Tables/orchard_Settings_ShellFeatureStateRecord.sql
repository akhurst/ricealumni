CREATE TABLE [dbo].[orchard_Settings_ShellFeatureStateRecord] (
    [Id]                  INT            IDENTITY (1, 1) NOT NULL,
    [Name]                NVARCHAR (255) NULL,
    [InstallState]        NVARCHAR (255) NULL,
    [EnableState]         NVARCHAR (255) NULL,
    [ShellStateRecord_Id] INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

