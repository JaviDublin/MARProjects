CREATE TABLE [dbo].[MARS_Users_Saved_Searches] (
    [searchId]              INT          IDENTITY (1, 1) NOT NULL,
    [searchName]            VARCHAR (50) NULL,
    [userId]                VARCHAR (10) NULL,
    [country]               VARCHAR (10) NULL,
    [cms_pool_id]           INT          NULL,
    [cms_location_group_id] INT          NULL,
    [ops_region_id]         INT          NULL,
    [ops_area_id]           INT          NULL,
    [location]              VARCHAR (50) NULL,
    [car_segment_id]        INT          NULL,
    [car_class_id]          INT          NULL,
    [car_group_id]          INT          NULL,
    CONSTRAINT [PK_MARS_Users_Saved_Searches] PRIMARY KEY CLUSTERED ([searchId] ASC),
    CONSTRAINT [FK_MARS_Users_Saved_Searches_MARS_Users] FOREIGN KEY ([userId]) REFERENCES [dbo].[MARS_Users] ([racfId]) ON DELETE CASCADE
);

