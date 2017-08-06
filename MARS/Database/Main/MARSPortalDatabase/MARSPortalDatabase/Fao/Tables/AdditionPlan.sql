CREATE TABLE [Fao].[AdditionPlan] (
    [AdditionPlanId]                INT            IDENTITY (1, 1) NOT NULL,
    [MinComSegScenarioName]         VARCHAR (50)   NOT NULL,
    [MaxFleetScenarioName]          VARCHAR (50)   NOT NULL,
    [MinComSegSccenarioDescription] VARCHAR (1000) NOT NULL,
    [MaxFleetScenarioDescription]   VARCHAR (1000) NOT NULL,
    [StartRevenueDate]              DATE           NOT NULL,
    [EndRevenueDate]                DATE           NOT NULL,
    [CurrentDate]                   DATE           NOT NULL,
    [WeeksCalculated]               INT            NOT NULL,
    [DateCreated]                   DATETIME       NOT NULL,
    [CreatedBy]                     VARCHAR (20)   NOT NULL,
    [Name]                          VARCHAR (100)  CONSTRAINT [DF_AdditionPlan_Name] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_AdditionPlan] PRIMARY KEY CLUSTERED ([AdditionPlanId] ASC)
);

