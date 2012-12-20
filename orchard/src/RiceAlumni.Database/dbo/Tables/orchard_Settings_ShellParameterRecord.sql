CREATE TABLE [dbo].[orchard_Settings_ShellParameterRecord] (
    [Id]                       INT            IDENTITY (1, 1) NOT NULL,
    [Component]                NVARCHAR (255) NULL,
    [Name]                     NVARCHAR (255) NULL,
    [Value]                    NVARCHAR (255) NULL,
    [ShellDescriptorRecord_id] INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

