CREATE TABLE [dbo].[ReservationStaging] (
    [EFF_DTTM]         VARCHAR (50) NULL,
    [RES_ID_NBR]       VARCHAR (14) NOT NULL,
    [COUNTRY]          VARCHAR (3)  NULL,
    [RENT_LOC]         VARCHAR (10) NULL,
    [RTRN_LOC]         VARCHAR (10) NULL,
    [ICIND]            VARCHAR (2)  NULL,
    [ONEWAY]           VARCHAR (2)  NULL,
    [RS_ARRIVAL_DATE]  DATETIME     NULL,
    [RS_ARRIVAL_TIME]  TIME (7)     NULL,
    [RTRN_DATE]        DATETIME     NULL,
    [RTRN_TIME]        TIME (7)     NULL,
    [RES_DAYS]         FLOAT (53)   NULL,
    [GR_INCL_GOLDUPGR] VARCHAR (4)  NULL,
    [RATE_QUOTED]      VARCHAR (15) NULL,
    [SUBTOTAL_2]       FLOAT (53)   NULL,
    [MOP]              VARCHAR (10) NULL,
    [PREPAID]          VARCHAR (2)  NULL,
    [NEVERLOST]        VARCHAR (20) NULL,
    [PREDELIVERY]      VARCHAR (2)  NULL,
    [CUST_NAME]        VARCHAR (28) NULL,
    [PHONE]            VARCHAR (25) NULL,
    [CDPID_NBR]        VARCHAR (10) NULL,
    [CNTID_NBR]        VARCHAR (10) NULL,
    [NO1_CLUB_GOLD]    VARCHAR (20) NULL,
    [TACO]             VARCHAR (10) NULL,
    [FLIGHT_NBR]       VARCHAR (10) NULL,
    [GS]               VARCHAR (2)  NULL,
    [N1TYPE]           VARCHAR (2)  NULL,
    [DATE_SOLD_DT]     DATETIME     NULL,
    [DATE_SOLD_TM]     TIME (7)     NULL
);









