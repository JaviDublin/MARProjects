CREATE TABLE [dbo].[VehicleHistory] (
    [VehicleHistoryId]    INT          IDENTITY (1, 1) NOT NULL,
    [VehicleId]           INT          NOT NULL,
    [OperationalStatusId] INT          NOT NULL,
    [VehicleFleetTypeId]  INT          NOT NULL,
    [IsNonRev]            BIT          NOT NULL,
    [RemarkId]            INT          NULL,
    [TimeStamp]           DATE         NOT NULL,
    [DaysNonRev]          INT          NULL,
    [MovementTypeId]      INT          NOT NULL,
    [LocationCode]        VARCHAR (10) NULL,
    [IsFleet]             BIT          NOT NULL,
    CONSTRAINT [PK_VehicleHistory] PRIMARY KEY CLUSTERED ([VehicleHistoryId] ASC),
    CONSTRAINT [FK_VehicleHistory_Movement_Types] FOREIGN KEY ([MovementTypeId]) REFERENCES [Settings].[Movement_Types] ([MovementTypeId]),
    CONSTRAINT [FK_VehicleHistory_NonRev_Remarks_List] FOREIGN KEY ([RemarkId]) REFERENCES [Settings].[NonRev_Remarks_List] ([RemarkId]),
    CONSTRAINT [FK_VehicleHistory_Operational_Status] FOREIGN KEY ([OperationalStatusId]) REFERENCES [Settings].[Operational_Status] ([OperationalStatusId]),
    CONSTRAINT [FK_VehicleHistory_Vehicle] FOREIGN KEY ([VehicleId]) REFERENCES [dbo].[Vehicle] ([VehicleId]),
    CONSTRAINT [FK_VehicleHistory_VehicleFleetType] FOREIGN KEY ([VehicleFleetTypeId]) REFERENCES [dbo].[VehicleFleetType] ([VehicleFleetTypeId])
);


GO
CREATE NONCLUSTERED INDEX [IX_MovementType]
    ON [dbo].[VehicleHistory]([MovementTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Remark]
    ON [dbo].[VehicleHistory]([RemarkId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_VehicleFleetType]
    ON [dbo].[VehicleHistory]([VehicleFleetTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_OperationalStatus]
    ON [dbo].[VehicleHistory]([OperationalStatusId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_VehicleId]
    ON [dbo].[VehicleHistory]([VehicleId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TimeStamp]
    ON [dbo].[VehicleHistory]([TimeStamp] ASC);

