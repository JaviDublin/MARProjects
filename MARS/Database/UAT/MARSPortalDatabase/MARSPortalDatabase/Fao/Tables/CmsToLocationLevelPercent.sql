CREATE TABLE [Fao].[CmsToLocationLevelPercent] (
    [CmsToLocationLevelPercentId] INT        IDENTITY (1, 1) NOT NULL,
    [CarGroupId]                  INT        NOT NULL,
    [LocationId]                  INT        NOT NULL,
    [LocationGroupId]             INT        NOT NULL,
    [PercentVehiclesAllocated]    FLOAT (53) NOT NULL,
    CONSTRAINT [PK_CmsToLocationLevelPercent] PRIMARY KEY CLUSTERED ([CmsToLocationLevelPercentId] ASC),
    CONSTRAINT [FK_CmsToLocationLevelPercent_CAR_GROUPS] FOREIGN KEY ([CarGroupId]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id]),
    CONSTRAINT [FK_CmsToLocationLevelPercent_CMS_LOCATION_GROUPS] FOREIGN KEY ([LocationGroupId]) REFERENCES [dbo].[CMS_LOCATION_GROUPS] ([cms_location_group_id]),
    CONSTRAINT [FK_CmsToLocationLevelPercent_LOCATIONS] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id])
);

