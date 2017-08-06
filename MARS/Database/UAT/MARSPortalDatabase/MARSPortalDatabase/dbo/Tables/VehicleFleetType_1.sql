CREATE TABLE [dbo].[VehicleFleetType] (
    [VehicleFleetTypeId] TINYINT       IDENTITY (1, 1) NOT NULL,
    [FleetTypeName]      VARCHAR (100) NULL,
    CONSTRAINT [PK_VehicleFleetType] PRIMARY KEY CLUSTERED ([VehicleFleetTypeId] ASC)
);



