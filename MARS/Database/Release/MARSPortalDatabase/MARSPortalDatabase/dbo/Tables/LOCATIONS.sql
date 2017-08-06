CREATE TABLE [dbo].[LOCATIONS] (
    [location]              VARCHAR (50) NOT NULL,
    [location_dw]           VARCHAR (50) NOT NULL,
    [real_location_name]    VARCHAR (50) NOT NULL,
    [location_name]         VARCHAR (50) NOT NULL,
    [location_name_dw]      VARCHAR (50) NOT NULL,
    [active]                BIT          NOT NULL,
    [ap_dt_rr]              VARCHAR (2)  NOT NULL,
    [cal]                   VARCHAR (1)  NOT NULL,
    [cms_location_group_id] INT          NULL,
    [ops_area_id]           INT          NOT NULL,
    [served_by_locn]        VARCHAR (50) NULL,
    [turnaround_hours]      INT          NULL,
    [ownarea]               VARCHAR (5)  NULL,
    [location_area_id]      VARCHAR (10) NULL,
    [city_desc]             VARCHAR (50) NULL,
    [country]               VARCHAR (10) NULL,
    [dim_Location_id]       INT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_LOCATIONS] PRIMARY KEY CLUSTERED ([dim_Location_id] ASC),
    CONSTRAINT [FK_LOCATIONS_AREACODES] FOREIGN KEY ([ownarea]) REFERENCES [dbo].[AREACODES] ([ownarea]),
    CONSTRAINT [FK_LOCATIONS_CMS_LOCATION_GROUPS] FOREIGN KEY ([cms_location_group_id]) REFERENCES [dbo].[CMS_LOCATION_GROUPS] ([cms_location_group_id]),
    CONSTRAINT [FK_LOCATIONS_COUNTRIES] FOREIGN KEY ([country]) REFERENCES [dbo].[COUNTRIES] ([country]),
    CONSTRAINT [FK_LOCATIONS_OPS_AREAS] FOREIGN KEY ([ops_area_id]) REFERENCES [dbo].[OPS_AREAS] ([ops_area_id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [ix_LOCATIONS_01]
    ON [dbo].[LOCATIONS]([location] ASC);

