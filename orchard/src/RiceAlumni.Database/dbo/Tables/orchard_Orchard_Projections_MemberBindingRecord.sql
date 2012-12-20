CREATE TABLE [dbo].[orchard_Orchard_Projections_MemberBindingRecord] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Type]        NVARCHAR (255) NULL,
    [Member]      NVARCHAR (64)  NULL,
    [Description] NVARCHAR (500) NULL,
    [DisplayName] NVARCHAR (64)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

