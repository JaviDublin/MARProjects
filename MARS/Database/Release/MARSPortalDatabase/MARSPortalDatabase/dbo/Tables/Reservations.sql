CREATE TABLE [dbo].[Reservations] (
    [RES_ID_NBR]         VARCHAR (14) NOT NULL,
    [COUNTRY]            VARCHAR (10) NULL,
    [RENT_LOC]           INT          NULL,
    [RTRN_LOC]           INT          NULL,
    [ICIND]              VARCHAR (2)  NULL,
    [ONEWAY]             VARCHAR (2)  NULL,
    [RS_ARRIVAL_DATE]    DATETIME     NULL,
    [RS_ARRIVAL_TIME]    DATETIME     NULL,
    [RTRN_DATE]          DATETIME     NULL,
    [RTRN_TIME]          DATETIME     NULL,
    [RES_DAYS]           FLOAT (53)   NULL,
    [GR_INCL_GOLDUPGR]   INT          NULL,
    [RATE_QUOTED]        VARCHAR (15) NULL,
    [SUBTOTAL_2]         FLOAT (53)   NULL,
    [MOP]                VARCHAR (10) NULL,
    [PREPAID]            VARCHAR (2)  NULL,
    [NEVERLOST]          VARCHAR (2)  NULL,
    [PREDELIVERY]        VARCHAR (2)  NULL,
    [CUST_NAME]          VARCHAR (28) NULL,
    [PHONE]              VARCHAR (25) NULL,
    [CDPID_NBR]          VARCHAR (10) NULL,
    [CNTID_NBR]          VARCHAR (10) NULL,
    [NO1_CLUB_GOLD]      VARCHAR (10) NULL,
    [TACO]               VARCHAR (10) NULL,
    [FLIGHT_NBR]         VARCHAR (10) NULL,
    [GS]                 VARCHAR (2)  NULL,
    [N1TYPE]             VARCHAR (2)  NULL,
    [DATE_SOLD]          DATETIME     NULL,
    [CO_HOURS]           INT          NULL,
    [CO_DAYS]            INT          NULL,
    [CI_HOURS]           INT          NULL,
    [CI_HOURS_OFFSET]    INT          NULL,
    [CI_DAYS]            INT          NULL,
    [ArrivalDateTime]    DATETIME     NULL,
    [ReturnDateTime]     DATETIME     NULL,
    [LastUpdated]        DATETIME     NULL,
    [ReservedCarGroup]   CHAR (2)     NULL,
    [ReservedCarGroupId] INT          NULL,
    CONSTRAINT [PK_RES_ID_NBRTest] PRIMARY KEY CLUSTERED ([RES_ID_NBR] ASC),
    CONSTRAINT [FK_Reservations_CarGrp] FOREIGN KEY ([GR_INCL_GOLDUPGR]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id]),
    CONSTRAINT [FK_Reservations_LOCATIONS] FOREIGN KEY ([RTRN_LOC]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id]),
    CONSTRAINT [FK_Reservations_LOCATIONS1] FOREIGN KEY ([RENT_LOC]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id])
);








