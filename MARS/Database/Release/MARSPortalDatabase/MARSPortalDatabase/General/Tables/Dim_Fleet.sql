﻿CREATE TABLE [General].[Dim_Fleet] (
    [VehicleId]          INT           IDENTITY (1, 1) NOT NULL,
    [Serial]             VARCHAR (50)  NULL,
    [Plate]              VARCHAR (50)  NULL,
    [Unit]               VARCHAR (50)  NULL,
    [MSODate]            DATETIME      NULL,
    [Country]            VARCHAR (10)  NULL,
    [Model]              VARCHAR (20)  NULL,
    [ModelDesc]          VARCHAR (255) NULL,
    [Color]              VARCHAR (10)  NULL,
    [CarGroup]           VARCHAR (20)  NULL,
    [OwnArea]            VARCHAR (50)  NULL,
    [IsActive]           BIT           NULL,
    [LastDailyFileDate]  DATETIME      NULL,
    [FirstDailyFileDate] DATETIME      NULL,
    [Total_Fleet]        BIT           NULL,
    [Operational_Fleet]  BIT           NULL,
    [Available_Fleet]    BIT           NULL,
    [Fleet_rac_ops]      BIT           NULL,
    [Fleet_rac_ttl]      BIT           NULL,
    [Fleet_licensee]     BIT           NULL,
    [Fleet_carsales]     BIT           NULL,
    [Fleet_adv]          BIT           NULL,
    [Fleet_hod]          BIT           NULL,
    PRIMARY KEY CLUSTERED ([VehicleId] ASC)
);

