CREATE TABLE [dbo].[VehicleNonRevPeriodEntry] (
    [VehicleNonRevPeriodEntryId] INT          IDENTITY (1, 1) NOT NULL,
    [VehicleNonRevPeriodId]      INT          NULL,
    [OperationalStatusId]        INT          NULL,
    [MovementTypeId]             INT          NULL,
    [LastLocationCode]           VARCHAR (10) NULL,
    [TimeStamp]                  DATETIME     NULL,
    [LastChangeDateTime]         DATETIME     NULL,
    CONSTRAINT [PK_VehicleNonRevPeriodEntry] PRIMARY KEY CLUSTERED ([VehicleNonRevPeriodEntryId] ASC),
    CONSTRAINT [FK_VehicleNonRevPeriodEntry_Movement_Types] FOREIGN KEY ([MovementTypeId]) REFERENCES [Settings].[Movement_Types] ([MovementTypeId]),
    CONSTRAINT [FK_VehicleNonRevPeriodEntry_Operational_Status] FOREIGN KEY ([OperationalStatusId]) REFERENCES [Settings].[Operational_Status] ([OperationalStatusId]),
    CONSTRAINT [FK_VehicleNonRevPeriodEntry_VehicleNonRevPeriod] FOREIGN KEY ([VehicleNonRevPeriodId]) REFERENCES [dbo].[VehicleNonRevPeriod] ([VechicleNonRevPeriodId])
);


GO
CREATE NONCLUSTERED INDEX [PeriodEntryLastChange]
    ON [dbo].[VehicleNonRevPeriodEntry]([VehicleNonRevPeriodId] ASC)
    INCLUDE([LastChangeDateTime]);

