CREATE TABLE [dbo].[VehicleNonRevPeriod] (
    [VechicleNonRevPeriodId]  INT      IDENTITY (1, 1) NOT NULL,
    [VehicleId]               INT      NULL,
    [EnteredNonRevTimestamp]  DATETIME NULL,
    [LeftNonRevTimestamp]     DATETIME NULL,
    [EnteredNonRevLastUpdate] DATETIME NULL,
    [LeftNonRevLastUpdate]    DATETIME NULL,
    [Active]                  BIT      NOT NULL,
    CONSTRAINT [PK_VehicleNonRevPeriod] PRIMARY KEY CLUSTERED ([VechicleNonRevPeriodId] ASC),
    CONSTRAINT [FK_VehicleNonRevPeriod_Vehicle] FOREIGN KEY ([VehicleId]) REFERENCES [dbo].[Vehicle] ([VehicleId])
);


GO
CREATE NONCLUSTERED INDEX [ActiveVehiclePeriodIndex]
    ON [dbo].[VehicleNonRevPeriod]([VehicleId] ASC, [Active] ASC);

