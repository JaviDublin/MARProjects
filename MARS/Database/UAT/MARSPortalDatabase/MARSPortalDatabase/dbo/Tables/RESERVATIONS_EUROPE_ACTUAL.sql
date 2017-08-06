﻿CREATE TABLE [dbo].[RESERVATIONS_EUROPE_ACTUAL] (
    [IMPORTTIME]       DATETIME      NOT NULL,
    [REP_YEAR]         VARCHAR (4)   NOT NULL,
    [REP_MONTH]        VARCHAR (2)   NOT NULL,
    [COUNTRY]          VARCHAR (10)  NULL,
    [CMS_POOL]         VARCHAR (10)  NULL,
    [CMS_LOC_GRP]      VARCHAR (10)  NULL,
    [OPS_REGION]       VARCHAR (10)  NULL,
    [OPS_AREA]         VARCHAR (10)  NULL,
    [CAR_SEGMENT]      VARCHAR (10)  NULL,
    [CAR_CLASS]        VARCHAR (10)  NULL,
    [CARVAN]           VARCHAR (10)  NULL,
    [RES_ID_NBR]       VARCHAR (14)  NULL,
    [RES_LOC]          VARCHAR (10)  NULL,
    [RENT_LOC]         VARCHAR (10)  NULL,
    [RTRN_LOC]         VARCHAR (10)  NULL,
    [ICIND]            VARCHAR (2)   NULL,
    [ONEWAY]           VARCHAR (2)   NULL,
    [RS_ARRIVAL_DATE]  DATETIME      NULL,
    [RS_ARRIVAL_TIME]  DATETIME      NULL,
    [RTRN_DATE]        DATETIME      NULL,
    [RTRN_TIME]        DATETIME      NULL,
    [RES_DAYS]         FLOAT (53)    NULL,
    [RES_VEH_CLASS]    VARCHAR (2)   NULL,
    [GR_INCL_GOLDUPGR] VARCHAR (2)   NULL,
    [RATE_QUOTED]      VARCHAR (15)  NULL,
    [SUBTOTAL_2]       FLOAT (53)    NULL,
    [MOP]              VARCHAR (10)  NULL,
    [PREPAID]          VARCHAR (2)   NULL,
    [NEVERLOST]        VARCHAR (2)   NULL,
    [PREDELIVERY]      VARCHAR (2)   NULL,
    [CUST_NAME]        VARCHAR (28)  NULL,
    [PHONE]            VARCHAR (25)  NULL,
    [CDPID_NBR]        VARCHAR (10)  NULL,
    [CNTID_NBR]        VARCHAR (10)  NULL,
    [NO1_CLUB_GOLD]    VARCHAR (10)  NULL,
    [TACO]             VARCHAR (10)  NULL,
    [FLIGHT_NBR]       VARCHAR (10)  NULL,
    [REMARKS]          VARCHAR (500) NULL,
    [GS]               VARCHAR (2)   NULL,
    [N1TYPE]           VARCHAR (2)   NULL,
    [DATE_SOLD]        DATETIME      NULL,
    [R1]               VARCHAR (25)  NULL,
    [R2]               VARCHAR (25)  NULL,
    [R3]               VARCHAR (25)  NULL,
    [TS]               DATETIME      NULL,
    [CO_HOURS]         INT           NULL,
    [CO_DAYS]          INT           NULL,
    [CI_HOURS]         INT           NULL,
    [CI_HOURS_OFFSET]  INT           NULL,
    [CI_DAYS]          INT           NULL
);

