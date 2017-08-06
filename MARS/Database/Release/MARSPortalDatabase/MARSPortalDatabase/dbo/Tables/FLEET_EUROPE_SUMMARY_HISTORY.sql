CREATE TABLE [dbo].[FLEET_EUROPE_SUMMARY_HISTORY] (
    [REP_YEAR]                       VARCHAR (4)  NULL,
    [REP_MONTH]                      VARCHAR (2)  NULL,
    [REP_WEEK_OF_YEAR]               VARCHAR (2)  NULL,
    [REP_DAY_OF_WEEK]                VARCHAR (1)  NULL,
    [REP_DATE]                       DATETIME     NULL,
    [COUNTRY]                        VARCHAR (10) NULL,
    [LOCATION]                       VARCHAR (50) NULL,
    [CAR_GROUP]                      VARCHAR (3)  NULL,
    [FLEET_RAC_TTL]                  BIT          NULL,
    [FLEET_RAC_OPS]                  BIT          NULL,
    [FLEET_CARSALES]                 BIT          NULL,
    [FLEET_LICENSEE]                 BIT          NULL,
    [TOTAL_FLEET]                    NUMERIC (9)  NULL,
    [CARSALES]                       NUMERIC (9)  NULL,
    [CARHOLD_H]                      NUMERIC (9)  NULL,
    [CARHOLD_L]                      NUMERIC (9)  NULL,
    [CU]                             NUMERIC (9)  NULL,
    [HA]                             NUMERIC (9)  NULL,
    [HL]                             NUMERIC (9)  NULL,
    [LL]                             NUMERIC (9)  NULL,
    [NC]                             NUMERIC (9)  NULL,
    [PL]                             NUMERIC (9)  NULL,
    [TC]                             NUMERIC (9)  NULL,
    [SV]                             NUMERIC (9)  NULL,
    [WS]                             NUMERIC (9)  NULL,
    [WS_NONRAC]                      NUMERIC (9)  NULL,
    [OPERATIONAL_FLEET]              NUMERIC (9)  NULL,
    [BD]                             NUMERIC (9)  NULL,
    [MM]                             NUMERIC (9)  NULL,
    [TW]                             NUMERIC (9)  NULL,
    [TB]                             NUMERIC (9)  NULL,
    [WS_RAC]                         NUMERIC (9)  NULL,
    [AVAILABLE_TB]                   NUMERIC (9)  NULL,
    [FS]                             NUMERIC (9)  NULL,
    [RL]                             NUMERIC (9)  NULL,
    [RP]                             NUMERIC (9)  NULL,
    [TN]                             NUMERIC (9)  NULL,
    [AVAILABLE_FLEET]                NUMERIC (9)  NULL,
    [RT]                             NUMERIC (9)  NULL,
    [SU]                             NUMERIC (9)  NULL,
    [GOLD]                           NUMERIC (9)  NULL,
    [PREDELIVERY]                    NUMERIC (9)  NULL,
    [OVERDUE]                        NUMERIC (9)  NULL,
    [ON_RENT]                        NUMERIC (9)  NULL,
    [TOTAL_FLEET_MIN]                NUMERIC (9)  NULL,
    [CARSALES_MIN]                   NUMERIC (9)  NULL,
    [CARHOLD_H_MIN]                  NUMERIC (9)  NULL,
    [CARHOLD_L_MIN]                  NUMERIC (9)  NULL,
    [CU_MIN]                         NUMERIC (9)  NULL,
    [HA_MIN]                         NUMERIC (9)  NULL,
    [HL_MIN]                         NUMERIC (9)  NULL,
    [LL_MIN]                         NUMERIC (9)  NULL,
    [NC_MIN]                         NUMERIC (9)  NULL,
    [PL_MIN]                         NUMERIC (9)  NULL,
    [TC_MIN]                         NUMERIC (9)  NULL,
    [SV_MIN]                         NUMERIC (9)  NULL,
    [WS_MIN]                         NUMERIC (9)  NULL,
    [WS_NONRAC_MIN]                  NUMERIC (9)  NULL,
    [OPERATIONAL_FLEET_MIN]          NUMERIC (9)  NULL,
    [BD_MIN]                         NUMERIC (9)  NULL,
    [MM_MIN]                         NUMERIC (9)  NULL,
    [TW_MIN]                         NUMERIC (9)  NULL,
    [TB_MIN]                         NUMERIC (9)  NULL,
    [WS_RAC_MIN]                     NUMERIC (9)  NULL,
    [AVAILABLE_TB_MIN]               NUMERIC (9)  NULL,
    [FS_MIN]                         NUMERIC (9)  NULL,
    [RL_MIN]                         NUMERIC (9)  NULL,
    [RP_MIN]                         NUMERIC (9)  NULL,
    [TN_MIN]                         NUMERIC (9)  NULL,
    [AVAILABLE_FLEET_MIN]            NUMERIC (9)  NULL,
    [RT_MIN]                         NUMERIC (9)  NULL,
    [SU_MIN]                         NUMERIC (9)  NULL,
    [GOLD_MIN]                       NUMERIC (9)  NULL,
    [PREDELIVERY_MIN]                NUMERIC (9)  NULL,
    [OVERDUE_MIN]                    NUMERIC (9)  NULL,
    [ON_RENT_MIN]                    NUMERIC (9)  NULL,
    [TOTAL_FLEET_MAX]                NUMERIC (9)  NULL,
    [CARSALES_MAX]                   NUMERIC (9)  NULL,
    [CARHOLD_H_MAX]                  NUMERIC (9)  NULL,
    [CARHOLD_L_MAX]                  NUMERIC (9)  NULL,
    [CU_MAX]                         NUMERIC (9)  NULL,
    [HA_MAX]                         NUMERIC (9)  NULL,
    [HL_MAX]                         NUMERIC (9)  NULL,
    [LL_MAX]                         NUMERIC (9)  NULL,
    [NC_MAX]                         NUMERIC (9)  NULL,
    [PL_MAX]                         NUMERIC (9)  NULL,
    [TC_MAX]                         NUMERIC (9)  NULL,
    [SV_MAX]                         NUMERIC (9)  NULL,
    [WS_MAX]                         NUMERIC (9)  NULL,
    [WS_NONRAC_MAX]                  NUMERIC (9)  NULL,
    [OPERATIONAL_FLEET_MAX]          NUMERIC (9)  NULL,
    [BD_MAX]                         NUMERIC (9)  NULL,
    [MM_MAX]                         NUMERIC (9)  NULL,
    [TW_MAX]                         NUMERIC (9)  NULL,
    [TB_MAX]                         NUMERIC (9)  NULL,
    [WS_RAC_MAX]                     NUMERIC (9)  NULL,
    [AVAILABLE_TB_MAX]               NUMERIC (9)  NULL,
    [FS_MAX]                         NUMERIC (9)  NULL,
    [RL_MAX]                         NUMERIC (9)  NULL,
    [RP_MAX]                         NUMERIC (9)  NULL,
    [TN_MAX]                         NUMERIC (9)  NULL,
    [AVAILABLE_FLEET_MAX]            NUMERIC (9)  NULL,
    [RT_MAX]                         NUMERIC (9)  NULL,
    [SU_MAX]                         NUMERIC (9)  NULL,
    [GOLD_MAX]                       NUMERIC (9)  NULL,
    [PREDELIVERY_MAX]                NUMERIC (9)  NULL,
    [OVERDUE_MAX]                    NUMERIC (9)  NULL,
    [ON_RENT_MAX]                    NUMERIC (9)  NULL,
    [TOTAL_FLEET_MAX_ABSOLUTE]       NUMERIC (9)  NULL,
    [OPERATIONAL_FLEET_MAX_ABSOLUTE] NUMERIC (9)  NULL,
    [OVERDUE_MAX_ABSOLUTE]           NUMERIC (9)  NULL,
    [ON_RENT_MAX_ABSOLUTE]           NUMERIC (9)  NULL,
    [FLEET_ADV]                      BIT          NULL,
    [FLEET_HOD]                      BIT          NULL,
    CONSTRAINT [FK_FLEET_EUROPE_SUMMARY_HISTORY_COUNTRIES] FOREIGN KEY ([COUNTRY]) REFERENCES [dbo].[COUNTRIES] ([country])
);








GO



GO



GO



GO
CREATE CLUSTERED INDEX [idx_rep_date]
    ON [dbo].[FLEET_EUROPE_SUMMARY_HISTORY]([REP_DATE] ASC)
    ON [scheme_fleet_partition_month] ([REP_DATE]);


GO


