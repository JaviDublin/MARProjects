CREATE TABLE [dbo].[MARS_CMS_FleetPlan_pre] (
    [country]     VARCHAR (10)   NULL,
    [fleetPlanID] INT            NOT NULL,
    [isAddition]  BIT            NOT NULL,
    [location]    VARCHAR (10)   NULL,
    [carclass]    VARCHAR (10)   NULL,
    [targetDate]  DATE           NULL,
    [amount]      NUMERIC (9, 2) NULL
);

