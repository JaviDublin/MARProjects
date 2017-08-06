CREATE TABLE [Fao].[MaxFleetFactorScenario] (
    [MaxFleetFactorScenarioId] INT            IDENTITY (1, 1) NOT NULL,
    [ScenarioName]             VARCHAR (50)   NOT NULL,
    [Description]              VARCHAR (1000) NULL,
    CONSTRAINT [PK_MaxFleetFactorScenario] PRIMARY KEY CLUSTERED ([MaxFleetFactorScenarioId] ASC)
);

