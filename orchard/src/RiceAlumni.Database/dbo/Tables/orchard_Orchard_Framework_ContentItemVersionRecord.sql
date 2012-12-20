CREATE TABLE [dbo].[orchard_Orchard_Framework_ContentItemVersionRecord] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [Number]               INT            NULL,
    [Published]            BIT            NULL,
    [Latest]               BIT            NULL,
    [Data]                 NVARCHAR (MAX) NULL,
    [ContentItemRecord_id] INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

