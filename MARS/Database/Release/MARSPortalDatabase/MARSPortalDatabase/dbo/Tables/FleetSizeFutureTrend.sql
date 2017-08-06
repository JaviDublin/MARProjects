CREATE TABLE [dbo].[FleetSizeFutureTrend] (
    [TargetDate]             DATETIME     NOT NULL,
    [OperationalFleet]       INT          NULL,
    [Amount]                 INT          NULL,
    [ExpectedFleet]          INT          NULL,
    [CarGrpId]               INT          NULL,
    [LocGrpId]               INT          NULL,
    [FleetPlanId]            INT          NULL,
    [Country]                VARCHAR (20) NULL,
    [FleetSizeFutureTrendId] INT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_FleetSizeFutureTrend] PRIMARY KEY CLUSTERED ([FleetSizeFutureTrendId] ASC)
);




GO
CREATE NONCLUSTERED INDEX [FleetPlanId]
    ON [dbo].[FleetSizeFutureTrend]([FleetPlanId] ASC, [Country] ASC, [TargetDate] ASC)
    INCLUDE([ExpectedFleet], [CarGrpId], [LocGrpId]);

