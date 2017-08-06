CREATE TABLE [dbo].[VehicleFleetType] (
    [VehicleFleetTypeId] INT           IDENTITY (1, 1) NOT NULL,
    [FleetTypeName]      VARCHAR (100) NULL,
    CONSTRAINT [PK_VehicleFleetType] PRIMARY KEY CLUSTERED ([VehicleFleetTypeId] ASC)
);

