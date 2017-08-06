CREATE TABLE [Inp].[Fact_FLEET_EUROPE_SUMMARY_HISTORY_Aggregate] (
    [dim_Calendar_id_start]   INT          NOT NULL,
    [dim_Calendar_id_end]     INT          NOT NULL,
    [Type]                    INT          NOT NULL,
    [NumDays]                 INT          NOT NULL,
    [COUNTRY]                 VARCHAR (10) NOT NULL,
    [car_group]               VARCHAR (10) NULL,
    [dim_Location_id]         INT          NOT NULL,
    [FLEET_CARSALES]          BIT          NOT NULL,
    [FLEET_RAC_TTL]           BIT          NOT NULL,
    [FLEET_RAC_OPS]           BIT          NOT NULL,
    [FLEET_LICENSEE]          BIT          NOT NULL,
    [TOTAL_FLEET]             INT          NOT NULL,
    [CARSALES]                INT          NOT NULL,
    [CARHOLD_H]               INT          NOT NULL,
    [CARHOLD_L]               INT          NOT NULL,
    [CU]                      INT          NOT NULL,
    [HA]                      INT          NOT NULL,
    [HL]                      INT          NOT NULL,
    [LL]                      INT          NOT NULL,
    [NC]                      INT          NOT NULL,
    [PL]                      INT          NOT NULL,
    [TC]                      INT          NOT NULL,
    [SV]                      INT          NOT NULL,
    [WS]                      INT          NOT NULL,
    [WS_NONRAC]               INT          NOT NULL,
    [OPERATIONAL_FLEET]       INT          NOT NULL,
    [BD]                      INT          NOT NULL,
    [MM]                      INT          NOT NULL,
    [TW]                      INT          NOT NULL,
    [TB]                      INT          NOT NULL,
    [WS_RAC]                  INT          NOT NULL,
    [AVAILABLE_TB]            INT          NOT NULL,
    [FS]                      INT          NOT NULL,
    [RL]                      INT          NOT NULL,
    [RP]                      INT          NOT NULL,
    [TN]                      INT          NOT NULL,
    [AVAILABLE_FLEET]         INT          NOT NULL,
    [RT]                      INT          NOT NULL,
    [SU]                      INT          NOT NULL,
    [GOLD]                    INT          NOT NULL,
    [PREDELIVERY]             INT          NOT NULL,
    [OVERDUE]                 INT          NOT NULL,
    [ON_RENT]                 INT          NOT NULL,
    [RT_MIN_SUM]              INT          NOT NULL,
    [AVAILABLE_FLEET_MIN_SUM] INT          NOT NULL,
    [RT_MAX_SUM]              INT          NOT NULL,
    [AVAILABLE_FLEET_MAX_SUM] INT          NOT NULL,
    [z_inserted]              DATETIME     DEFAULT (getdate()) NOT NULL,
    [FLEET_ADV]               BIT          NULL,
    [FLEET_HOD]               BIT          NULL
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [ix_Fact_FLEET_EUROPE_SUMMARY_HISTORY_Aggregate_02]
    ON [Inp].[Fact_FLEET_EUROPE_SUMMARY_HISTORY_Aggregate]([dim_Calendar_id_start] ASC, [Type] ASC, [COUNTRY] ASC, [car_group] ASC, [dim_Location_id] ASC, [FLEET_CARSALES] ASC, [FLEET_RAC_TTL] ASC, [FLEET_RAC_OPS] ASC, [FLEET_LICENSEE] ASC);


GO
CREATE CLUSTERED INDEX [ix_Fact_FLEET_EUROPE_SUMMARY_HISTORY_Aggregate_01]
    ON [Inp].[Fact_FLEET_EUROPE_SUMMARY_HISTORY_Aggregate]([dim_Calendar_id_start] ASC, [Type] ASC);

