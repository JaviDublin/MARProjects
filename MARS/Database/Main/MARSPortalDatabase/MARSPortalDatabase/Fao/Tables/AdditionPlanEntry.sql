CREATE TABLE [Fao].[AdditionPlanEntry] (
    [AdditionPlanEntryId] INT      IDENTITY (1, 1) NOT NULL,
    [AdditionPlanId]      INT      NOT NULL,
    [Year]                SMALLINT NOT NULL,
    [Week]                TINYINT  NOT NULL,
    [CarGroupId]          INT      NOT NULL,
    [LocationId]          INT      NOT NULL,
    [Additions]           INT      NOT NULL,
    CONSTRAINT [PK_AdditionPlanEntry] PRIMARY KEY CLUSTERED ([AdditionPlanEntryId] ASC),
    CONSTRAINT [FK_AdditionPlanEntry_AdditionPlan] FOREIGN KEY ([AdditionPlanId]) REFERENCES [Fao].[AdditionPlan] ([AdditionPlanId]),
    CONSTRAINT [FK_AdditionPlanEntry_CAR_GROUPS] FOREIGN KEY ([CarGroupId]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id]),
    CONSTRAINT [FK_AdditionPlanEntry_LOCATIONS] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id])
);

