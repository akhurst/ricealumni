CREATE TABLE [dbo].[orchard_Scheduling_ScheduledTaskRecord] (
    [Id]                          INT            IDENTITY (1, 1) NOT NULL,
    [TaskType]                    NVARCHAR (255) NULL,
    [ScheduledUtc]                DATETIME       NULL,
    [ContentItemVersionRecord_id] INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

