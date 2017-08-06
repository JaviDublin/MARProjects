﻿CREATE TABLE [dbo].[FLEET_EUROPE_ACTUAL_STAGING] (
    [IMPORTTIME]             DATETIME        NULL,
    [BASEOP]                 NUMERIC (18, 2) NULL,
    [BDDAYS]                 INT             NULL,
    [CAPCOST]                NUMERIC (18, 2) NULL,
    [CAPDATE]                DATETIME        NULL,
    [CARHOLD1]               VARCHAR (5)     NULL,
    [COLOR]                  VARCHAR (5)     NULL,
    [DAYSAREA]               INT             NULL,
    [DAYSCTRY]               INT             NULL,
    [DAYSMOVE]               INT             NULL,
    [DAYSREV]                INT             NULL,
    [DEPAMT]                 NUMERIC (18, 2) NULL,
    [DEPSTAT]                VARCHAR (5)     NULL,
    [DRVNAME]                VARCHAR (25)    NULL,
    [DUEAREA]                VARCHAR (10)    NULL,
    [DUEDATE]                DATETIME        NULL,
    [DUETIME]                DATETIME        NULL,
    [DUEWWD]                 VARCHAR (10)    NULL,
    [IDATE]                  DATETIME        NULL,
    [LICENSE]                VARCHAR (25)    NULL,
    [LSTAREA]                VARCHAR (10)    NULL,
    [LSTDATE]                DATETIME        NULL,
    [LSTMLG]                 NUMERIC (18, 2) NULL,
    [LSTNO]                  VARCHAR (25)    NULL,
    [LSTOORC]                VARCHAR (10)    NULL,
    [LSTTIME]                DATETIME        NULL,
    [LSTTYPE]                VARCHAR (10)    NULL,
    [LSTWWD]                 VARCHAR (10)    NULL,
    [MMDAYS]                 INT             NULL,
    [MODDESC]                VARCHAR (50)    NULL,
    [MODEL]                  VARCHAR (10)    NULL,
    [MODGROUP]               VARCHAR (25)    NULL,
    [MOVETYPE]               VARCHAR (10)    NULL,
    [MSODATE]                DATETIME        NULL,
    [OPERDAYS]               INT             NULL,
    [OPERSTAT]               VARCHAR (50)    NULL,
    [OWNAREA]                VARCHAR (10)    NULL,
    [OWNWWD]                 VARCHAR (10)    NULL,
    [PONO]                   VARCHAR (25)    NULL,
    [PREVWWD]                VARCHAR (10)    NULL,
    [RADATE]                 DATETIME        NULL,
    [RALOC]                  VARCHAR (10)    NULL,
    [RC]                     VARCHAR (5)     NULL,
    [SDATE]                  DATETIME        NULL,
    [SERIAL]                 VARCHAR (25)    NOT NULL,
    [SPROCDAT]               DATETIME        NULL,
    [TERMDAYS]               INT             NULL,
    [UNIT]                   VARCHAR (10)    NULL,
    [VC]                     VARCHAR (5)     NULL,
    [VEHCLAS]                VARCHAR (5)     NULL,
    [VEHTYPE]                VARCHAR (5)     NULL,
    [VENDNBR]                VARCHAR (10)    NULL,
    [VISMODEL]               VARCHAR (25)    NULL,
    [COUNTRY]                VARCHAR (10)    NULL,
    [POOL]                   INT             NULL,
    [LOC_GROUP]              INT             NULL,
    [CARVAN]                 VARCHAR (2)     NULL,
    [CAR_CLASS]              INT             NULL,
    [FLEET_RAC_TTL]          BIT             NULL,
    [FLEET_RAC_OPS]          BIT             NULL,
    [FLEET_CARSALES]         BIT             NULL,
    [FLEET_LICENSEE]         BIT             NULL,
    [TOTAL_FLEET]            INT             NULL,
    [CARSALES]               INT             NULL,
    [CARHOLD_H]              INT             NOT NULL,
    [CARHOLD_L]              INT             NOT NULL,
    [CU]                     INT             NOT NULL,
    [HA]                     INT             NOT NULL,
    [HL]                     INT             NOT NULL,
    [LL]                     INT             NOT NULL,
    [NC]                     INT             NOT NULL,
    [PL]                     INT             NOT NULL,
    [TC]                     INT             NOT NULL,
    [SV]                     INT             NOT NULL,
    [WS]                     INT             NOT NULL,
    [WS_NONRAC]              INT             NULL,
    [OPERATIONAL_FLEET]      INT             NULL,
    [BD]                     INT             NOT NULL,
    [MM]                     INT             NOT NULL,
    [TW]                     INT             NOT NULL,
    [TB]                     INT             NOT NULL,
    [WS_RAC]                 INT             NULL,
    [AVAILABLE_TB]           INT             NOT NULL,
    [FS]                     INT             NOT NULL,
    [RL]                     INT             NOT NULL,
    [RP]                     INT             NOT NULL,
    [TN]                     INT             NOT NULL,
    [AVAILABLE_FLEET]        INT             NULL,
    [RT]                     INT             NOT NULL,
    [SU]                     INT             NOT NULL,
    [GOLD]                   INT             NOT NULL,
    [PREDELIVERY]            INT             NOT NULL,
    [OVERDUE]                INT             NULL,
    [ON_RENT]                INT             NULL,
    [CI_HOURS]               INT             NULL,
    [CI_HOURS_OFFSET]        INT             NULL,
    [CI_DAYS]                INT             NULL,
    [FLEET_ADV]              BIT             NULL,
    [FLEET_HOD]              BIT             NULL,
    [CountryCar]             VARCHAR (5)     NULL,
    [CountryLoc]             VARCHAR (5)     NULL,
    [IsNonRev]               BIT             NULL,
    [location_group]         VARCHAR (255)   NULL,
    [location_group_id]      INT             NULL,
    [location_pool]          VARCHAR (255)   NULL,
    [location_pool_id]       INT             NULL,
    [location_ops_area]      VARCHAR (255)   NULL,
    [location_ops_area_id]   INT             NULL,
    [location_ops_region]    VARCHAR (255)   NULL,
    [location_ops_region_id] INT             NULL,
    [DayGroupCode]           VARCHAR (20)    NULL,
    [DayGroupName]           VARCHAR (20)    NULL,
    [OperationalStatusCode]  VARCHAR (20)    NULL,
    [KCICode]                VARCHAR (20)    NULL,
    [car_group]              VARCHAR (20)    NULL,
    [car_group_id]           INT             NULL,
    [car_class_name]         VARCHAR (100)   NULL,
    [car_class_id]           INT             NULL,
    [car_segment]            VARCHAR (100)   NULL,
    [car_segment_id]         INT             NULL
);
