CREATE TABLE [dbo].[orchard_Containers_ContainerWidgetPartRecord] (
    [Id]               INT            NOT NULL,
    [ContainerId]      INT            NULL,
    [PageSize]         INT            NULL,
    [OrderByProperty]  NVARCHAR (255) NULL,
    [OrderByDirection] INT            NULL,
    [ApplyFilter]      BIT            NULL,
    [FilterByProperty] NVARCHAR (255) NULL,
    [FilterByOperator] NVARCHAR (255) NULL,
    [FilterByValue]    NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

