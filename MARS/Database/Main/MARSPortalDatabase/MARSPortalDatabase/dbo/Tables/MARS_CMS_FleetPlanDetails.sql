CREATE TABLE [dbo].[MARS_CMS_FleetPlanDetails] (
    [PKID]                  INT           IDENTITY (1, 1) NOT NULL,
    [fleetPlanEntryID]      INT           NOT NULL,
    [targetDate]            SMALLDATETIME NOT NULL,
    [cms_Location_Group_ID] INT           NOT NULL,
    [car_class_id]          INT           NOT NULL,
    [addition]              SMALLINT      CONSTRAINT [DF_MARS_CMS_FleetPlanDetails_temp_addition] DEFAULT ((0)) NOT NULL,
    [deletion]              SMALLINT      CONSTRAINT [DF_MARS_CMS_FleetPlanDetails_temp_deletion] DEFAULT ((0)) NOT NULL,
    [amount]                AS            ([addition]-[deletion]),
    CONSTRAINT [PK_MARS_CMS_FleetPlanDetails_temp] PRIMARY KEY CLUSTERED ([PKID] ASC),
    CONSTRAINT [FK_MARS_CMS_FleetPlanDetails_CAR_GROUPS] FOREIGN KEY ([car_class_id]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id]),
    CONSTRAINT [FK_MARS_CMS_FleetPlanDetails_CMS_LOCATION_GROUPS] FOREIGN KEY ([cms_Location_Group_ID]) REFERENCES [dbo].[CMS_LOCATION_GROUPS] ([cms_location_group_id]),
    CONSTRAINT [FK_MARS_CMS_FleetPlanDetails_MARS_CMS_FleetPlanEntry] FOREIGN KEY ([fleetPlanEntryID]) REFERENCES [dbo].[MARS_CMS_FleetPlanEntry] ([PKID])
);




GO



GO




