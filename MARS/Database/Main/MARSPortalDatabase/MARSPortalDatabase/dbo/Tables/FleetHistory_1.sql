﻿CREATE TABLE [dbo].[FleetHistory] (
    [FleetHistoryId]         INT        IDENTITY (1, 1) NOT NULL,
    [Timestamp]              DATE       NOT NULL,
    [CarGroupId]             INT        NOT NULL,
    [LocationId]             INT        NOT NULL,
    [FleetTypeId]            TINYINT    NOT NULL,
    [AvgTotal]               FLOAT (53) NOT NULL,
    [MinTotal]               INT        NOT NULL,
    [MaxTotal]               INT        NOT NULL,
    [PeakTotal]              INT        NOT NULL,
    [TroughTotal]            INT        NOT NULL,
    [AvgBd]                  FLOAT (53) NOT NULL,
    [MinBd]                  INT        NOT NULL,
    [MaxBd]                  INT        NOT NULL,
    [PeakBd]                 INT        NOT NULL,
    [TroughBd]               INT        NOT NULL,
    [AvgCu]                  FLOAT (53) NOT NULL,
    [MinCu]                  INT        NOT NULL,
    [MaxCu]                  INT        NOT NULL,
    [PeakCu]                 INT        NOT NULL,
    [TroughCu]               INT        NOT NULL,
    [AvgFs]                  FLOAT (53) NOT NULL,
    [MinFs]                  INT        NOT NULL,
    [MaxFs]                  INT        NOT NULL,
    [PeakFs]                 INT        NOT NULL,
    [TroughFs]               INT        NOT NULL,
    [AvgHa]                  FLOAT (53) NOT NULL,
    [MinHa]                  INT        NOT NULL,
    [MaxHa]                  INT        NOT NULL,
    [PeakHa]                 INT        NOT NULL,
    [TroughHa]               INT        NOT NULL,
    [AvgHl]                  FLOAT (53) NOT NULL,
    [MinHl]                  INT        NOT NULL,
    [MaxHl]                  INT        NOT NULL,
    [PeakHl]                 INT        NOT NULL,
    [TroughHl]               INT        NOT NULL,
    [AvgLl]                  FLOAT (53) NOT NULL,
    [MinLl]                  INT        NOT NULL,
    [MaxLl]                  INT        NOT NULL,
    [PeakLl]                 INT        NOT NULL,
    [TroughLl]               INT        NOT NULL,
    [AvgMm]                  FLOAT (53) NOT NULL,
    [MinMm]                  INT        NOT NULL,
    [MaxMm]                  INT        NOT NULL,
    [PeakMm]                 INT        NOT NULL,
    [TroughMm]               INT        NOT NULL,
    [AvgNc]                  FLOAT (53) NOT NULL,
    [MinNc]                  INT        NOT NULL,
    [MaxNc]                  INT        NOT NULL,
    [PeakNc]                 INT        NOT NULL,
    [TroughNc]               INT        NOT NULL,
    [AvgPl]                  FLOAT (53) NOT NULL,
    [MinPl]                  INT        NOT NULL,
    [MaxPl]                  INT        NOT NULL,
    [PeakPl]                 INT        NOT NULL,
    [TroughPl]               INT        NOT NULL,
    [AvgRl]                  FLOAT (53) NOT NULL,
    [MinRl]                  INT        NOT NULL,
    [MaxRl]                  INT        NOT NULL,
    [PeakRl]                 INT        NOT NULL,
    [TroughRl]               INT        NOT NULL,
    [AvgRp]                  FLOAT (53) NOT NULL,
    [MinRp]                  INT        NOT NULL,
    [MaxRp]                  INT        NOT NULL,
    [PeakRp]                 INT        NOT NULL,
    [TroughRp]               INT        NOT NULL,
    [AvgIdle]                FLOAT (53) NOT NULL,
    [MinIdle]                INT        NOT NULL,
    [MaxIdle]                INT        NOT NULL,
    [PeakIdle]               INT        NOT NULL,
    [TroughIdle]             INT        NOT NULL,
    [AvgSu]                  FLOAT (53) NOT NULL,
    [MinSu]                  INT        NOT NULL,
    [MaxSu]                  INT        NOT NULL,
    [PeakSu]                 INT        NOT NULL,
    [TroughSu]               INT        NOT NULL,
    [AvgSv]                  FLOAT (53) NOT NULL,
    [MinSv]                  INT        NOT NULL,
    [MaxSv]                  INT        NOT NULL,
    [PeakSv]                 INT        NOT NULL,
    [TroughSv]               INT        NOT NULL,
    [AvgTb]                  FLOAT (53) NOT NULL,
    [MinTb]                  INT        NOT NULL,
    [MaxTb]                  INT        NOT NULL,
    [PeakTb]                 INT        NOT NULL,
    [TroughTb]               INT        NOT NULL,
    [AvgTc]                  FLOAT (53) NOT NULL,
    [MinTc]                  INT        NOT NULL,
    [MaxTc]                  INT        NOT NULL,
    [PeakTc]                 INT        NOT NULL,
    [TroughTc]               INT        NOT NULL,
    [AvgTn]                  FLOAT (53) NOT NULL,
    [MinTn]                  INT        NOT NULL,
    [MaxTn]                  INT        NOT NULL,
    [PeakTn]                 INT        NOT NULL,
    [TroughTn]               INT        NOT NULL,
    [AvgTw]                  FLOAT (53) NOT NULL,
    [MinTw]                  INT        NOT NULL,
    [MaxTw]                  INT        NOT NULL,
    [PeakTw]                 INT        NOT NULL,
    [TroughTw]               INT        NOT NULL,
    [AvgWs]                  FLOAT (53) NOT NULL,
    [MinWs]                  INT        NOT NULL,
    [MaxWs]                  INT        NOT NULL,
    [PeakWs]                 INT        NOT NULL,
    [TroughWs]               INT        NOT NULL,
    [AvgOverdue]             FLOAT (53) NOT NULL,
    [MinOverdue]             INT        NOT NULL,
    [MaxOverdue]             INT        NOT NULL,
    [PeakOverdue]            INT        NOT NULL,
    [TroughOverdue]          INT        NOT NULL,
    [AvgOperationalFleet]    FLOAT (53) NOT NULL,
    [MinOperationalFleet]    INT        NOT NULL,
    [MaxOperationalFleet]    INT        NOT NULL,
    [PeakOperationalFleet]   INT        NOT NULL,
    [TroughOperationalFleet] INT        NOT NULL,
    [AvgAvailableFleet]      FLOAT (53) NOT NULL,
    [MinAvailableFleet]      INT        NOT NULL,
    [MaxAvailableFleet]      INT        NOT NULL,
    [PeakAvailableFleet]     INT        NOT NULL,
    [TroughAvailableFleet]   INT        NOT NULL,
    [AvgOnRent]              FLOAT (53) NOT NULL,
    [MinOnRent]              INT        NOT NULL,
    [MaxOnRent]              INT        NOT NULL,
    [PeakOnRent]             INT        NOT NULL,
    [TroughOnRent]           INT        NOT NULL,
    CONSTRAINT [PK_FleetHistory] PRIMARY KEY NONCLUSTERED ([FleetHistoryId] ASC),
    CONSTRAINT [FK_FleetHistory_CAR_GROUPS] FOREIGN KEY ([CarGroupId]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id]),
    CONSTRAINT [FK_FleetHistory_LOCATIONS] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id]),
    CONSTRAINT [FK_FleetHistory_VehicleFleetType] FOREIGN KEY ([FleetTypeId]) REFERENCES [dbo].[VehicleFleetType] ([VehicleFleetTypeId])
);




GO
CREATE NONCLUSTERED INDEX [FleetTypeIndex]
    ON [dbo].[FleetHistory]([FleetTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [LocationIdIndex]
    ON [dbo].[FleetHistory]([LocationId] ASC);


GO
CREATE NONCLUSTERED INDEX [CarGroupIndex]
    ON [dbo].[FleetHistory]([CarGroupId] ASC);


GO
CREATE CLUSTERED INDEX [TimestampIndex]
    ON [dbo].[FleetHistory]([Timestamp] ASC, [LocationId] ASC, [CarGroupId] ASC, [FleetTypeId] ASC);



