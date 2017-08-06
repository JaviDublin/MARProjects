CREATE TABLE [General].[VEHICLE_LEASE] (
    [VehicleId]        INT           IDENTITY (1, 1) NOT NULL,
    [Serial]           VARCHAR (25)  NOT NULL,
    [Plate]            VARCHAR (25)  NULL,
    [Unit]             VARCHAR (25)  NULL,
    [ModelDescription] VARCHAR (255) NULL,
    [Country_Owner]    VARCHAR (2)   NOT NULL,
    [Country_Rent]     VARCHAR (2)   NOT NULL,
    [StartDate]        DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([VehicleId] ASC)
);

