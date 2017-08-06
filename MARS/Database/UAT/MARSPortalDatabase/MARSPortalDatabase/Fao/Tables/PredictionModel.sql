CREATE TABLE [Fao].[PredictionModel] (
    [PredictionModelId] INT          IDENTITY (1, 1) NOT NULL,
    [CountryId]         INT          NOT NULL,
    [Active]            BIT          CONSTRAINT [DF_PredictionModel_Active] DEFAULT ((0)) NOT NULL,
    [Processed]         BIT          CONSTRAINT [DF_PredictionModel_Processed] DEFAULT ((0)) NOT NULL,
    [UpdatedBy]         VARCHAR (50) NOT NULL,
    [UpdatedOn]         VARCHAR (50) NOT NULL,
    [CreatedBy]         VARCHAR (50) NOT NULL,
    [CreatedOn]         VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_PredictionModel] PRIMARY KEY CLUSTERED ([PredictionModelId] ASC)
);

