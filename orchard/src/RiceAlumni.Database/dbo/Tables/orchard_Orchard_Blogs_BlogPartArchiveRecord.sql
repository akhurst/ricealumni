CREATE TABLE [dbo].[orchard_Orchard_Blogs_BlogPartArchiveRecord] (
    [Id]          INT IDENTITY (1, 1) NOT NULL,
    [Year]        INT NULL,
    [Month]       INT NULL,
    [PostCount]   INT NULL,
    [BlogPart_id] INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

