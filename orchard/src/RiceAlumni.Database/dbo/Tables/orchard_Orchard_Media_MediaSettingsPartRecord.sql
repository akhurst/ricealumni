CREATE TABLE [dbo].[orchard_Orchard_Media_MediaSettingsPartRecord] (
    [Id]                             INT            NOT NULL,
    [UploadAllowedFileTypeWhitelist] NVARCHAR (255) DEFAULT ('jpg jpeg gif png txt doc docx xls xlsx pdf ppt pptx pps ppsx odt ods odp') NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

