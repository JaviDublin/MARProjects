CREATE TABLE [Fao].[FileUpload] (
    [FileUploadId]     INT           IDENTITY (1, 1) NOT NULL,
    [FileUploadTypeId] INT           NOT NULL,
    [FileName]         VARCHAR (200) NOT NULL,
    [FileUploadedBy]   VARCHAR (50)  NOT NULL,
    [CreatedDate]      DATETIME      NOT NULL,
    [CountryId]        INT           CONSTRAINT [DF_FileUpload_CountryId] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_FileUpload] PRIMARY KEY CLUSTERED ([FileUploadId] ASC),
    CONSTRAINT [FK_FileUpload_COUNTRIES] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[COUNTRIES] ([CountryId]),
    CONSTRAINT [FK_FileUpload_FileUploadType] FOREIGN KEY ([FileUploadTypeId]) REFERENCES [Fao].[FileUploadType] ([FileUploadTypeId])
);



