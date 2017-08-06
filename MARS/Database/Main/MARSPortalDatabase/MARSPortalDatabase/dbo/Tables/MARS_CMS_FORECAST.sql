﻿CREATE TABLE [dbo].[MARS_CMS_FORECAST] (
    [REP_DATE]              DATETIME       NOT NULL,
    [COUNTRY]               VARCHAR (10)   NULL,
    [CMS_LOCATION_GROUP_ID] INT            NOT NULL,
    [CAR_CLASS_ID]          INT            NOT NULL,
    [CONSTRAINED]           NUMERIC (9, 2) NULL,
    [UNCONSTRAINED]         NUMERIC (9, 2) NULL,
    [FLEET]                 NUMERIC (9, 2) NULL,
    [RESERVATIONS_BOOKED]   NUMERIC (9, 2) NULL,
    [CURRENT_ONRENT]        NUMERIC (9, 2) NULL,
    [FORECASTED_RETURNS]    NUMERIC (9, 2) NULL,
    [ONRENT_LY]             NUMERIC (9, 2) NULL,
    [AVAILABLE_FLEET]       NUMERIC (9, 2) NULL,
    [OPERATIONAL_FLEET]     NUMERIC (9, 2) NULL,
    [OPERATIONAL_FLEET_S1]  NUMERIC (9, 2) NULL,
    [OPERATIONAL_FLEET_S2]  NUMERIC (9, 2) NULL,
    [OPERATIONAL_FLEET_S3]  NUMERIC (9, 2) NULL,
    [OPERATIONAL_FLEET_AV]  NUMERIC (9, 2) NULL,
    [TOTAL_FLEET]           NUMERIC (9, 2) NULL,
    [MarsCmsForecastId]     INT            IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_MARS_CMS_FORECAST] PRIMARY KEY CLUSTERED ([MarsCmsForecastId] ASC),
    CONSTRAINT [FK_MARS_CMS_FORECAST_CAR_GROUPS] FOREIGN KEY ([CAR_CLASS_ID]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MARS_CMS_FORECAST_CMS_LOCATION_GROUPS] FOREIGN KEY ([CMS_LOCATION_GROUP_ID]) REFERENCES [dbo].[CMS_LOCATION_GROUPS] ([cms_location_group_id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MARS_CMS_FORECAST_COUNTRIES] FOREIGN KEY ([COUNTRY]) REFERENCES [dbo].[COUNTRIES] ([country])
);










GO



GO



GO





GO
CREATE NONCLUSTERED INDEX [idx_rep_date_cms]
    ON [dbo].[MARS_CMS_FORECAST]([REP_DATE] ASC);




GO



GO
CREATE NONCLUSTERED INDEX [CountryAndRepDate]
    ON [dbo].[MARS_CMS_FORECAST]([COUNTRY] ASC, [REP_DATE] ASC)
    INCLUDE([CMS_LOCATION_GROUP_ID], [CAR_CLASS_ID], [CONSTRAINED], [UNCONSTRAINED], [RESERVATIONS_BOOKED]);


GO
CREATE NONCLUSTERED INDEX [MarsCmsForecastIndexOnRepDate]
    ON [dbo].[MARS_CMS_FORECAST]([REP_DATE] ASC)
    INCLUDE([COUNTRY], [CMS_LOCATION_GROUP_ID], [CAR_CLASS_ID], [OPERATIONAL_FLEET_AV]);


GO
CREATE NONCLUSTERED INDEX [indx_test_car_class]
    ON [dbo].[MARS_CMS_FORECAST]([CAR_CLASS_ID] ASC)
    INCLUDE([REP_DATE], [COUNTRY], [CMS_LOCATION_GROUP_ID], [CONSTRAINED], [UNCONSTRAINED], [FLEET], [RESERVATIONS_BOOKED]);


GO
CREATE NONCLUSTERED INDEX [FaoIndex]
    ON [dbo].[MARS_CMS_FORECAST]([CMS_LOCATION_GROUP_ID] ASC, [CAR_CLASS_ID] ASC, [REP_DATE] ASC)
    INCLUDE([UNCONSTRAINED]);

