CREATE TABLE [Fao].[PredictionModelFileUpload] (
    [PredictionModelFileUploadId] INT IDENTITY (1, 1) NOT NULL,
    [PredictionModelId]           INT NOT NULL,
    [FileUploadId]                INT NOT NULL,
    CONSTRAINT [PK_PredictionModelFileUpload] PRIMARY KEY CLUSTERED ([PredictionModelFileUploadId] ASC),
    CONSTRAINT [FK_PredictionModelFileUpload_FileUpload] FOREIGN KEY ([FileUploadId]) REFERENCES [Fao].[FileUpload] ([FileUploadId]),
    CONSTRAINT [FK_PredictionModelFileUpload_PredictionModel] FOREIGN KEY ([PredictionModelId]) REFERENCES [Fao].[PredictionModel] ([PredictionModelId])
);

