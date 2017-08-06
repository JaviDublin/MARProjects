CREATE TABLE [dbo].[FleetSizeFutureTrendBackup] (
    [TargetDate]             DATETIME     NOT NULL,
    [OperationalFleet]       INT          NULL,
    [Amount]                 INT          NULL,
    [ExpectedFleet]          INT          NULL,
    [CarGrpId]               INT          NULL,
    [LocGrpId]               INT          NULL,
    [FleetPlanId]            INT          NULL,
    [Country]                VARCHAR (20) NULL,
    [FleetSizeFutureTrendId] INT          IDENTITY (1, 1) NOT NULL
);

