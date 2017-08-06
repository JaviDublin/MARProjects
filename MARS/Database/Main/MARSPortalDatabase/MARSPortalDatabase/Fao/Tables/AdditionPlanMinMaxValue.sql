CREATE TABLE [Fao].[AdditionPlanMinMaxValue] (
    [AdditionPlanMinMaxValueId] INT      IDENTITY (1, 1) NOT NULL,
    [AdditionPlanId]            INT      NOT NULL,
    [Year]                      SMALLINT NOT NULL,
    [Week]                      TINYINT  NOT NULL,
    [CarGroupId]                INT      NOT NULL,
    [LocationId]                INT      NOT NULL,
    [Rank]                      INT      NOT NULL,
    [OperationalFleet]          INT      NOT NULL,
    [AdditionsAndDeletions]     INT      NOT NULL,
    [MinFleet]                  INT      NOT NULL,
    [MaxFleet]                  INT      NOT NULL,
    CONSTRAINT [PK_AdditionPlanMinMaxFigures] PRIMARY KEY CLUSTERED ([AdditionPlanMinMaxValueId] ASC),
    CONSTRAINT [FK_AdditionPlanMinMaxFigures_AdditionPlan] FOREIGN KEY ([AdditionPlanId]) REFERENCES [Fao].[AdditionPlan] ([AdditionPlanId]),
    CONSTRAINT [FK_AdditionPlanMinMaxFigures_CAR_GROUPS] FOREIGN KEY ([CarGroupId]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id]),
    CONSTRAINT [FK_AdditionPlanMinMaxFigures_LOCATIONS] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id])
);

