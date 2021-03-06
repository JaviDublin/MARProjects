﻿CREATE TABLE [dbo].[FleetNow] (
    [FleetNowId]       INT        IDENTITY (1, 1) NOT NULL,
    [Timestamp]        DATETIME   NOT NULL,
    [CarGroupId]       INT        NOT NULL,
    [LocationId]       INT        NOT NULL,
    [FleetTypeId]      TINYINT    NOT NULL,
    [SumTotal]         FLOAT (53) NOT NULL,
    [SumBd]            FLOAT (53) NOT NULL,
    [SumCu]            FLOAT (53) NOT NULL,
    [SumFs]            FLOAT (53) NOT NULL,
    [SumHa]            FLOAT (53) NOT NULL,
    [SumHl]            FLOAT (53) NOT NULL,
    [SumLl]            FLOAT (53) NOT NULL,
    [SumMm]            FLOAT (53) NOT NULL,
    [SumNc]            FLOAT (53) NOT NULL,
    [SumPl]            FLOAT (53) NOT NULL,
    [SumRl]            FLOAT (53) NOT NULL,
    [SumRp]            FLOAT (53) NOT NULL,
    [SumIdle]          FLOAT (53) NOT NULL,
    [SumSu]            FLOAT (53) NOT NULL,
    [SumSv]            FLOAT (53) NOT NULL,
    [SumTb]            FLOAT (53) NOT NULL,
    [SumTc]            FLOAT (53) NOT NULL,
    [SumTn]            FLOAT (53) NOT NULL,
    [SumTw]            FLOAT (53) NOT NULL,
    [SumWs]            FLOAT (53) NOT NULL,
    [SumOverdue]       FLOAT (53) NOT NULL,
    [Utilization]      FLOAT (53) NOT NULL,
    [OperationalFleet] AS         (((((((([SumTotal]-[SumCU])-[SumHA])-[SumHL])-[SumNC])-[SumPL])-[SumTC])-[SumSV])-[SumWS]),
    [AvailableFleet]   AS         (((((((((((((((([SumTotal]-[SumCU])-[SumHA])-[SumHL])-[SumNC])-[SumPL])-[SumTC])-[SumSV])-[SumWS])-[SumBD])-[SumMM])-[SumTW])-[SumTB])-[SumFS])-[SumRL])-[SumRP])-[SumTN]),
    [OnRent]           AS         ((((((((((((((((((([SumTotal]-[SumCU])-[SumHA])-[SumHL])-[SumNC])-[SumPL])-[SumTC])-[SumSV])-[SumWS])-[SumBD])-[SumMM])-[SumTW])-[SumTB])-[SumFS])-[SumRL])-[SumRP])-[SumTN])-[SumIdle])-[SumSU])-[SumOverdue]),
    CONSTRAINT [PK_FleetNow] PRIMARY KEY CLUSTERED ([FleetNowId] ASC),
    CONSTRAINT [FK_FleetNow_LOCATIONS] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id]),
    CONSTRAINT [FK_FleetNow_VehicleFleetType] FOREIGN KEY ([FleetTypeId]) REFERENCES [dbo].[VehicleFleetType] ([VehicleFleetTypeId])
);



