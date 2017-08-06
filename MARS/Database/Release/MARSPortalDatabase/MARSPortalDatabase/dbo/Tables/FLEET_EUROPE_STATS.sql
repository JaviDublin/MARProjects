CREATE TABLE [dbo].[FLEET_EUROPE_STATS] (
    [IMPORTDATE]        DATETIME     NOT NULL,
    [REP_YEAR]          VARCHAR (4)  NULL,
    [REP_MONTH]         VARCHAR (2)  NULL,
    [REP_WEEK_OF_YEAR]  VARCHAR (2)  NULL,
    [REP_DAY_OF_WEEK]   VARCHAR (1)  NULL,
    [REP_DATE]          DATETIME     NULL,
    [COUNTRY]           VARCHAR (10) NULL,
    [LOCATION]          VARCHAR (50) NULL,
    [CAR_GROUP]         VARCHAR (3)  NULL,
    [FLEET_RAC_TTL]     BIT          NULL,
    [FLEET_RAC_OPS]     BIT          NULL,
    [FLEET_CARSALES]    BIT          NULL,
    [FLEET_LICENSEE]    BIT          NULL,
    [TOTAL_FLEET]       NUMERIC (18) NULL,
    [CARSALES]          NUMERIC (18) NULL,
    [CARHOLD_H]         NUMERIC (18) NULL,
    [CARHOLD_L]         NUMERIC (18) NULL,
    [CU]                NUMERIC (18) NULL,
    [HA]                NUMERIC (18) NULL,
    [HL]                NUMERIC (18) NULL,
    [LL]                NUMERIC (18) NULL,
    [NC]                NUMERIC (18) NULL,
    [PL]                NUMERIC (18) NULL,
    [TC]                NUMERIC (18) NULL,
    [SV]                NUMERIC (18) NULL,
    [WS]                NUMERIC (18) NULL,
    [WS_NONRAC]         NUMERIC (18) NULL,
    [OPERATIONAL_FLEET] NUMERIC (18) NULL,
    [BD]                NUMERIC (18) NULL,
    [MM]                NUMERIC (18) NULL,
    [TW]                NUMERIC (18) NULL,
    [TB]                NUMERIC (18) NULL,
    [WS_RAC]            NUMERIC (18) NULL,
    [AVAILABLE_TB]      NUMERIC (18) NULL,
    [FS]                NUMERIC (18) NULL,
    [RL]                NUMERIC (18) NULL,
    [RP]                NUMERIC (18) NULL,
    [TN]                NUMERIC (18) NULL,
    [AVAILABLE_FLEET]   NUMERIC (18) NULL,
    [RT]                NUMERIC (18) NULL,
    [SU]                NUMERIC (18) NULL,
    [GOLD]              NUMERIC (18) NULL,
    [PREDELIVERY]       NUMERIC (18) NULL,
    [OVERDUE]           NUMERIC (18) NULL,
    [ON_RENT]           NUMERIC (18) NULL,
    [FLEET_ADV]         BIT          NULL,
    [FLEET_HOD]         BIT          NULL,
    CONSTRAINT [FK_FLEET_EUROPE_STATS_COUNTRIES] FOREIGN KEY ([COUNTRY]) REFERENCES [dbo].[COUNTRIES] ([country])
);






GO



GO
CREATE CLUSTERED INDEX [idx_rep_date_stats]
    ON [dbo].[FLEET_EUROPE_STATS]([REP_DATE] ASC);


GO
CREATE NONCLUSTERED INDEX [idx__FESH__REPDATE_MONTH_stats]
    ON [dbo].[FLEET_EUROPE_STATS]([REP_DATE] ASC)
    INCLUDE([COUNTRY], [LOCATION], [CAR_GROUP], [FLEET_RAC_TTL], [FLEET_CARSALES]);

