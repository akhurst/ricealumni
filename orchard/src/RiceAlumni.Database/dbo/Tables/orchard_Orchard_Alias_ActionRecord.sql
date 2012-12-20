CREATE TABLE [dbo].[orchard_Orchard_Alias_ActionRecord] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Area]       NVARCHAR (255) NULL,
    [Controller] NVARCHAR (255) NULL,
    [Action]     NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

