CREATE TABLE [Fao].[PredictionModelNonRevFactor] (
    [PredictionModelNonRevFactorId] INT        NOT NULL,
    [PredictionModelId]             INT        NOT NULL,
    [LocationId]                    INT        NOT NULL,
    [CarGroupId]                    INT        NOT NULL,
    [DayOfWeek]                     TINYINT    NOT NULL,
    [Percentage]                    FLOAT (53) NOT NULL,
    CONSTRAINT [PK_PredictionModelNonRevFactor] PRIMARY KEY CLUSTERED ([PredictionModelNonRevFactorId] ASC),
    CONSTRAINT [FK_PredictionModelNonRevFactor_CAR_GROUPS] FOREIGN KEY ([CarGroupId]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id]),
    CONSTRAINT [FK_PredictionModelNonRevFactor_LOCATIONS] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id]),
    CONSTRAINT [FK_PredictionModelNonRevFactor_PredictionModel] FOREIGN KEY ([PredictionModelId]) REFERENCES [Fao].[PredictionModel] ([PredictionModelId])
);

