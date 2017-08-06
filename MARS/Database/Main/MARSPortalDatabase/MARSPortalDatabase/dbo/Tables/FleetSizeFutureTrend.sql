CREATE TABLE [dbo].[FleetSizeFutureTrend] (
    [TargetDate]             DATETIME     NOT NULL,
    [OperationalFleet]       INT          NULL,
    [Amount]                 INT          NULL,
    [ExpectedFleet]          INT          NULL,
    [CarGrpId]               INT          NOT NULL,
    [LocGrpId]               INT          NOT NULL,
    [FleetPlanId]            INT          NOT NULL,
    [Country]                VARCHAR (10) NOT NULL,
    [FleetSizeFutureTrendId] INT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_FleetSizeFutureTrend] PRIMARY KEY NONCLUSTERED ([FleetSizeFutureTrendId] ASC),
    CONSTRAINT [FK_FleetSizeFutureTrend_CAR_GROUPS] FOREIGN KEY ([CarGrpId]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id]),
    CONSTRAINT [FK_FleetSizeFutureTrend_CMS_LOCATION_GROUPS] FOREIGN KEY ([LocGrpId]) REFERENCES [dbo].[CMS_LOCATION_GROUPS] ([cms_location_group_id]),
    CONSTRAINT [FK_FleetSizeFutureTrend_FleetSizeFutureTrend] FOREIGN KEY ([Country]) REFERENCES [dbo].[COUNTRIES] ([country])
);






GO
CREATE CLUSTERED INDEX [MyCluster]
    ON [dbo].[FleetSizeFutureTrend]([TargetDate] ASC, [FleetPlanId] ASC, [Country] ASC, [LocGrpId] ASC, [CarGrpId] ASC);

