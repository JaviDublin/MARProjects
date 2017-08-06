CREATE TABLE [dbo].[VehicleNonRevPeriod] (
    [VechicleNonRevPeriodId]  INT      IDENTITY (1, 1) NOT NULL,
    [VehicleId]               INT      NULL,
    [EnteredNonRevTimestamp]  DATETIME NULL,
    [LeftNonRevTimestamp]     DATETIME NULL,
    [EnteredNonRevLastUpdate] DATETIME NULL,
    [LeftNonRevLastUpdate]    DATETIME NULL,
    [Active]                  BIT      NOT NULL,
    [DaysInNonRev]            INT      NULL,
    CONSTRAINT [PK_VehicleNonRevPeriod] PRIMARY KEY CLUSTERED ([VechicleNonRevPeriodId] ASC) WITH (FILLFACTOR = 90, STATISTICS_NORECOMPUTE = ON),
    CONSTRAINT [FK_VehicleNonRevPeriod_Vehicle] FOREIGN KEY ([VehicleId]) REFERENCES [dbo].[Vehicle] ([VehicleId])
);


GO
ALTER TABLE [dbo].[VehicleNonRevPeriod] NOCHECK CONSTRAINT [FK_VehicleNonRevPeriod_Vehicle];




GO
CREATE NONCLUSTERED INDEX [IX_Vehicle]
    ON [dbo].[VehicleNonRevPeriod]([VehicleId] ASC) WITH (FILLFACTOR = 90, STATISTICS_NORECOMPUTE = ON);



