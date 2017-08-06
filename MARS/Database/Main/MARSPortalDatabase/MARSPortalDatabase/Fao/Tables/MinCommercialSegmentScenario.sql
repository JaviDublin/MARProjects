CREATE TABLE [Fao].[MinCommercialSegmentScenario] (
    [MinCommercialSegmentScenarioId] INT            IDENTITY (1, 1) NOT NULL,
    [ScenarioName]                   VARCHAR (50)   NOT NULL,
    [Description]                    VARCHAR (1000) NULL,
    CONSTRAINT [PK_MinCommercialSegmentScenario] PRIMARY KEY CLUSTERED ([MinCommercialSegmentScenarioId] ASC)
);

