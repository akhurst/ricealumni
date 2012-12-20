CREATE TABLE [dbo].[orchard_Containers_ContainerPartRecord] (
    [Id]               INT            NOT NULL,
    [Paginated]        BIT            NULL,
    [PageSize]         INT            NULL,
    [OrderByProperty]  NVARCHAR (255) NULL,
    [OrderByDirection] INT            NULL,
    [ItemContentType]  NVARCHAR (255) NULL,
    [ItemsShown]       BIT            DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

