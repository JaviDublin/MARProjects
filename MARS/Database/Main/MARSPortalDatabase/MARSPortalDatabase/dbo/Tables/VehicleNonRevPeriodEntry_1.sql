CREATE TABLE [dbo].[VehicleNonRevPeriodEntry] (
    [VehicleNonRevPeriodEntryId] INT          IDENTITY (1, 1) NOT NULL,
    [VehicleNonRevPeriodId]      INT          NOT NULL,
    [OperationalStatusId]        INT          NOT NULL,
    [MovementTypeId]             INT          NOT NULL,
    [LastLocationCode]           VARCHAR (10) NULL,
    [TimeStamp]                  DATETIME     NULL,
    [LastChangeDateTime]         DATETIME     NULL,
    [VehicleFleetTypeId]         TINYINT      NULL,
    [DaysNonRev]                 INT          NULL,
    CONSTRAINT [PK_VehicleNonRevPeriodEntry] PRIMARY KEY CLUSTERED ([VehicleNonRevPeriodEntryId] ASC) WITH (FILLFACTOR = 90, STATISTICS_NORECOMPUTE = ON),
    CONSTRAINT [FK_VehicleNonRevPeriodEntry_Movement_Types] FOREIGN KEY ([MovementTypeId]) REFERENCES [Settings].[Movement_Types] ([MovementTypeId]),
    CONSTRAINT [FK_VehicleNonRevPeriodEntry_Operational_Status] FOREIGN KEY ([OperationalStatusId]) REFERENCES [Settings].[Operational_Status] ([OperationalStatusId]),
    CONSTRAINT [FK_VehicleNonRevPeriodEntry_VehicleFleetType] FOREIGN KEY ([VehicleFleetTypeId]) REFERENCES [dbo].[VehicleFleetType] ([VehicleFleetTypeId]),
    CONSTRAINT [FK_VehicleNonRevPeriodEntry_VehicleNonRevPeriod] FOREIGN KEY ([VehicleNonRevPeriodId]) REFERENCES [dbo].[VehicleNonRevPeriod] ([VechicleNonRevPeriodId])
);


GO
ALTER TABLE [dbo].[VehicleNonRevPeriodEntry] NOCHECK CONSTRAINT [FK_VehicleNonRevPeriodEntry_Movement_Types];


GO
ALTER TABLE [dbo].[VehicleNonRevPeriodEntry] NOCHECK CONSTRAINT [FK_VehicleNonRevPeriodEntry_Operational_Status];


GO
ALTER TABLE [dbo].[VehicleNonRevPeriodEntry] NOCHECK CONSTRAINT [FK_VehicleNonRevPeriodEntry_VehicleFleetType];


GO
ALTER TABLE [dbo].[VehicleNonRevPeriodEntry] NOCHECK CONSTRAINT [FK_VehicleNonRevPeriodEntry_VehicleNonRevPeriod];






GO
CREATE NONCLUSTERED INDEX [IX_VehicleFleetType]
    ON [dbo].[VehicleNonRevPeriodEntry]([VehicleFleetTypeId] ASC) WITH (FILLFACTOR = 90, STATISTICS_NORECOMPUTE = ON);




GO
CREATE NONCLUSTERED INDEX [IX_MovementType]
    ON [dbo].[VehicleNonRevPeriodEntry]([MovementTypeId] ASC) WITH (FILLFACTOR = 90, STATISTICS_NORECOMPUTE = ON);




GO
CREATE NONCLUSTERED INDEX [IX_OperationalStatus]
    ON [dbo].[VehicleNonRevPeriodEntry]([OperationalStatusId] ASC) WITH (FILLFACTOR = 90, STATISTICS_NORECOMPUTE = ON);




GO
CREATE NONCLUSTERED INDEX [IX_VehicleNonRevPeriod]
    ON [dbo].[VehicleNonRevPeriodEntry]([VehicleNonRevPeriodId] ASC)
    INCLUDE([LastChangeDateTime]) WITH (FILLFACTOR = 90, STATISTICS_NORECOMPUTE = ON);



