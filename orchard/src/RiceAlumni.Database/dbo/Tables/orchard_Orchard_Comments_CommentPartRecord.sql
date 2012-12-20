CREATE TABLE [dbo].[orchard_Orchard_Comments_CommentPartRecord] (
    [Id]                    INT            NOT NULL,
    [Author]                NVARCHAR (255) NULL,
    [SiteName]              NVARCHAR (255) NULL,
    [UserName]              NVARCHAR (255) NULL,
    [Email]                 NVARCHAR (255) NULL,
    [Status]                NVARCHAR (255) NULL,
    [CommentDateUtc]        DATETIME       NULL,
    [CommentText]           NVARCHAR (MAX) NULL,
    [CommentedOn]           INT            NULL,
    [CommentedOnContainer]  INT            NULL,
    [CommentsPartRecord_id] INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

