﻿CREATE TABLE [dbo].[MARS_CMS_FORECAST_ADJUSTMENT] (
    [REP_DATE]              DATETIME       NOT NULL,
    [COUNTRY]               VARCHAR (10)   NULL,
    [CMS_LOCATION_GROUP_ID] INT            NULL,
    [CAR_CLASS_ID]          INT            NULL,
    [ADJUSTMENT_TD]         NUMERIC (9, 2) NULL,
    [ADJUSTMENT_BU1]        NUMERIC (9, 2) NULL,
    [ADJUSTMENT_BU2]        NUMERIC (9, 2) NULL,
    [ADJUSTMENT_RC]         NUMERIC (9, 2) NULL,
    CONSTRAINT [FK_MARS_CMS_FORECAST_ADJUSTMENT_CAR_GROUPS] FOREIGN KEY ([CAR_CLASS_ID]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MARS_CMS_FORECAST_ADJUSTMENT_CMS_LOCATION_GROUPS] FOREIGN KEY ([CMS_LOCATION_GROUP_ID]) REFERENCES [dbo].[CMS_LOCATION_GROUPS] ([cms_location_group_id]) ON DELETE CASCADE
);

