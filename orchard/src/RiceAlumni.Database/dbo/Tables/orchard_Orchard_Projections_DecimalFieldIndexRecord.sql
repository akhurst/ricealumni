CREATE TABLE [dbo].[orchard_Orchard_Projections_DecimalFieldIndexRecord] (
    [Id]                      INT             IDENTITY (1, 1) NOT NULL,
    [PropertyName]            NVARCHAR (255)  NULL,
    [Value]                   DECIMAL (19, 5) NULL,
    [FieldIndexPartRecord_Id] INT             NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

