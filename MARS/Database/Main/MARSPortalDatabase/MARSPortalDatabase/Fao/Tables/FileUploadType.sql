CREATE TABLE [Fao].[FileUploadType] (
    [FileUploadTypeId] INT           IDENTITY (1, 1) NOT NULL,
    [FileType]         VARCHAR (200) NOT NULL,
    CONSTRAINT [PK_FileUploadType] PRIMARY KEY CLUSTERED ([FileUploadTypeId] ASC)
);

