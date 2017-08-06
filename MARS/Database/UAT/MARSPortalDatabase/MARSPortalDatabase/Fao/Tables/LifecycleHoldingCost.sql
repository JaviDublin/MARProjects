CREATE TABLE [Fao].[LifecycleHoldingCost] (
    [LifecycleHoldingCostId] INT        IDENTITY (1, 1) NOT NULL,
    [Month]                  TINYINT    NOT NULL,
    [Year]                   SMALLINT   NOT NULL,
    [CarGroupId]             INT        NOT NULL,
    [Cost]                   FLOAT (53) NOT NULL,
    CONSTRAINT [PK_LifecycleHoldingCost] PRIMARY KEY CLUSTERED ([LifecycleHoldingCostId] ASC),
    CONSTRAINT [FK_LifecycleHoldingCost_CAR_GROUPS] FOREIGN KEY ([CarGroupId]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UnqiueHoldingCost]
    ON [Fao].[LifecycleHoldingCost]([Month] ASC, [Year] ASC, [CarGroupId] ASC);

