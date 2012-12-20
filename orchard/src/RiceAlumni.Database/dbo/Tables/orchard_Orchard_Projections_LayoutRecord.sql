﻿CREATE TABLE [dbo].[orchard_Orchard_Projections_LayoutRecord] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [Category]           NVARCHAR (64)  NULL,
    [Type]               NVARCHAR (64)  NULL,
    [Description]        NVARCHAR (255) NULL,
    [State]              NVARCHAR (MAX) NULL,
    [DisplayType]        NVARCHAR (64)  NULL,
    [Display]            INT            NULL,
    [QueryPartRecord_id] INT            NULL,
    [GroupProperty_id]   INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

