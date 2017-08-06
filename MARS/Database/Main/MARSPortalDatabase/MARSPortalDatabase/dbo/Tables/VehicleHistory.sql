CREATE TABLE [dbo].[VehicleHistory] (
    [VehicleHistoryId]    INT          IDENTITY (1, 1) NOT NULL,
    [VehicleId]           INT          NOT NULL,
    [OperationalStatusId] INT          NOT NULL,
    [VehicleFleetTypeId]  TINYINT      NOT NULL,
    [IsNonRev]            BIT          NOT NULL,
    [RemarkId]            INT          NULL,
    [TimeStamp]           DATE         NOT NULL,
    [DaysNonRev]          INT          NULL,
    [MovementTypeId]      INT          NOT NULL,
    [LocationCode]        VARCHAR (10) NULL,
    [IsFleet]             BIT          NOT NULL,
    [ExpectedDateTime]    DATETIME     NULL,
    [DaysInCountry]       INT          NULL,
    [LastLocationId]      INT          NULL,
    [ExpectedLocationId]  INT          NULL,
    CONSTRAINT [PK_VehicleHistory] PRIMARY KEY CLUSTERED ([VehicleHistoryId] ASC),
    CONSTRAINT [FK_VehicleHistory_LOCATIONS] FOREIGN KEY ([LastLocationId]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id]),
    CONSTRAINT [FK_VehicleHistory_LOCATIONS1] FOREIGN KEY ([ExpectedLocationId]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id]),
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




GO





GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20140610-110937]
    ON [dbo].[VehicleHistory]([ExpectedDateTime] ASC);


GO
CREATE NONCLUSTERED INDEX [HistoricalTrendIndex]
    ON [dbo].[VehicleHistory]([OperationalStatusId] ASC, [IsNonRev] ASC, [TimeStamp] ASC, [IsFleet] ASC, [VehicleFleetTypeId] ASC, [DaysNonRev] ASC)
    INCLUDE([VehicleId]);

