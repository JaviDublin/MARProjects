CREATE TABLE [Fao].[PredictionModelUtilizationFactor] (
    [PredictionModelUtilizationFactorId] INT        IDENTITY (1, 1) NOT NULL,
    [PredictionModelId]                  INT        NOT NULL,
    [LocationId]                         INT        NOT NULL,
    [CarGroupId]                         INT        NOT NULL,
    [DayOfWeek]                          TINYINT    NOT NULL,
    [Percentage]                         FLOAT (53) NOT NULL,
    CONSTRAINT [PK_PredictionModelUtilizationFactor] PRIMARY KEY CLUSTERED ([PredictionModelUtilizationFactorId] ASC),
    CONSTRAINT [FK_PredictionModelUtilizationFactor_CAR_GROUPS] FOREIGN KEY ([CarGroupId]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id]),
    CONSTRAINT [FK_PredictionModelUtilizationFactor_LOCATIONS] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id]),
    CONSTRAINT [FK_PredictionModelUtilizationFactor_PredictionModel] FOREIGN KEY ([PredictionModelId]) REFERENCES [Fao].[PredictionModel] ([PredictionModelId])
);

