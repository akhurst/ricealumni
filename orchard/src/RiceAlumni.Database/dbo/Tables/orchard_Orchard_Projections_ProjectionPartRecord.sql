CREATE TABLE [dbo].[orchard_Orchard_Projections_ProjectionPartRecord] (
    [Id]                 INT            NOT NULL,
    [Items]              INT            NULL,
    [ItemsPerPage]       INT            NULL,
    [Skip]               INT            NULL,
    [PagerSuffix]        NVARCHAR (255) NULL,
    [MaxItems]           INT            NULL,
    [DisplayPager]       BIT            NULL,
    [QueryPartRecord_id] INT            NULL,
    [LayoutRecord_Id]    INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

