CREATE TABLE [dbo].[orchard_Orchard_Projections_DoubleFieldIndexRecord] (
    [Id]                      INT            IDENTITY (1, 1) NOT NULL,
    [PropertyName]            NVARCHAR (255) NULL,
    [Value]                   FLOAT (53)     NULL,
    [FieldIndexPartRecord_Id] INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

