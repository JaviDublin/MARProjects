﻿CREATE TABLE [dbo].[FLEET_EUROPE_ACTUAL] (
    [IMPORTTIME]        DATETIME        NULL,
    [BASEOP]            NUMERIC (18, 2) NULL,
    [BDDAYS]            INT             NULL,
    [CAPCOST]           NUMERIC (18, 2) NULL,
    [CAPDATE]           DATETIME        NULL,
    [CARHOLD1]          VARCHAR (5)     NULL,
    [COLOR]             VARCHAR (5)     NULL,
    [DAYSAREA]          INT             NULL,
    [DAYSCTRY]          INT             NULL,
    [DAYSMOVE]          INT             NULL,
    [DAYSREV]           INT             NULL,
    [DEPAMT]            NUMERIC (18, 2) NULL,
    [DEPSTAT]           VARCHAR (5)     NULL,
    [DRVNAME]           VARCHAR (25)    NULL,
    [DUEAREA]           VARCHAR (10)    NULL,
    [DUEDATE]           DATETIME        NULL,
    [DUETIME]           DATETIME        NULL,
    [DUEWWD]            VARCHAR (10)    NULL,
    [IDATE]             DATETIME        NULL,
    [LICENSE]           VARCHAR (25)    NULL,
    [LSTAREA]           VARCHAR (10)    NULL,
    [LSTDATE]           DATETIME        NULL,
    [LSTMLG]            NUMERIC (18, 2) NULL,
    [LSTNO]             VARCHAR (25)    NULL,
    [LSTOORC]           VARCHAR (10)    NULL,
    [LSTTIME]           DATETIME        NULL,
    [LSTTYPE]           VARCHAR (10)    NULL,
    [LSTWWD]            VARCHAR (10)    NULL,
    [MMDAYS]            INT             NULL,
    [MODDESC]           VARCHAR (50)    NULL,
    [MODEL]             VARCHAR (10)    NULL,
    [MODGROUP]          VARCHAR (25)    NULL,
    [MOVETYPE]          VARCHAR (10)    NULL,
    [MSODATE]           DATETIME        NULL,
    [OPERDAYS]          INT             NULL,
    [OPERSTAT]          VARCHAR (50)    NULL,
    [OWNAREA]           VARCHAR (10)    NULL,
    [OWNWWD]            VARCHAR (10)    NULL,
    [PONO]              VARCHAR (25)    NULL,
    [PREVWWD]           VARCHAR (10)    NULL,
    [RADATE]            DATETIME        NULL,
    [RALOC]             VARCHAR (10)    NULL,
    [RC]                VARCHAR (5)     NULL,
    [SDATE]             DATETIME        NULL,
    [SERIAL]            VARCHAR (25)    NOT NULL,
    [SPROCDAT]          DATETIME        NULL,
    [TERMDAYS]          INT             NULL,
    [UNIT]              VARCHAR (10)    NULL,
    [VC]                VARCHAR (5)     NULL,
    [VEHCLAS]           VARCHAR (5)     NULL,
    [VEHTYPE]           VARCHAR (5)     NULL,
    [VENDNBR]           VARCHAR (10)    NULL,
    [VISMODEL]          VARCHAR (25)    NULL,
    [COUNTRY]           VARCHAR (10)    NULL,
    [POOL]              INT             NULL,
    [LOC_GROUP]         INT             NULL,
    [CARVAN]            VARCHAR (2)     NULL,
    [CAR_CLASS]         INT             NULL,
    [FLEET_RAC_TTL]     BIT             NULL,
    [FLEET_RAC_OPS]     BIT             NULL,
    [FLEET_CARSALES]    BIT             NULL,
    [FLEET_LICENSEE]    BIT             NULL,
    [TOTAL_FLEET]       INT             NULL,
    [CARSALES]          INT             NULL,
    [CARHOLD_H]         INT             NOT NULL,
    [CARHOLD_L]         INT             NOT NULL,
    [CU]                INT             NOT NULL,
    [HA]                INT             NOT NULL,
    [HL]                INT             NOT NULL,
    [LL]                INT             NOT NULL,
    [NC]                INT             NOT NULL,
    [PL]                INT             NOT NULL,
    [TC]                INT             NOT NULL,
    [SV]                INT             NOT NULL,
    [WS]                INT             NOT NULL,
    [WS_NONRAC]         INT             NULL,
    [OPERATIONAL_FLEET] INT             NULL,
    [BD]                INT             NOT NULL,
    [MM]                INT             NOT NULL,
    [TW]                INT             NOT NULL,
    [TB]                INT             NOT NULL,
    [WS_RAC]            INT             NULL,
    [AVAILABLE_TB]      INT             NOT NULL,
    [FS]                INT             NOT NULL,
    [RL]                INT             NOT NULL,
    [RP]                INT             NOT NULL,
    [TN]                INT             NOT NULL,
    [AVAILABLE_FLEET]   INT             NULL,
    [RT]                INT             NOT NULL,
    [SU]                INT             NOT NULL,
    [GOLD]              INT             NOT NULL,
    [PREDELIVERY]       INT             NOT NULL,
    [OVERDUE]           INT             NULL,
    [ON_RENT]           INT             NULL,
    [CI_HOURS]          INT             NULL,
    [CI_HOURS_OFFSET]   INT             NULL,
    [CI_DAYS]           INT             NULL,
    [FLEET_ADV]         BIT             NULL,
    [FLEET_HOD]         BIT             NULL,
    [VehicleId]         INT             IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_FLEET_EUROPE_ACTUAL] PRIMARY KEY CLUSTERED ([VehicleId] ASC),
    CONSTRAINT [FK_FLEET_EUROPE_ACTUAL_COUNTRIES] FOREIGN KEY ([COUNTRY]) REFERENCES [dbo].[COUNTRIES] ([country]),
    CONSTRAINT [FK_FLEET_EUROPE_ACTUAL_OPERSTATS] FOREIGN KEY ([OPERSTAT]) REFERENCES [dbo].[OPERSTATS] ([operstat_name])
);








GO



GO



GO



GO
CREATE NONCLUSTERED INDEX [VC]
    ON [dbo].[FLEET_EUROPE_ACTUAL]([VC] ASC);


GO
CREATE NONCLUSTERED INDEX [SERIAL]
    ON [dbo].[FLEET_EUROPE_ACTUAL]([SERIAL] ASC);


GO
CREATE NONCLUSTERED INDEX [LSTWWD]
    ON [dbo].[FLEET_EUROPE_ACTUAL]([LSTWWD] ASC);


GO
CREATE NONCLUSTERED INDEX [COUNTRY]
    ON [dbo].[FLEET_EUROPE_ACTUAL]([COUNTRY] ASC);

