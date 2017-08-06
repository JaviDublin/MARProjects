CREATE TABLE [dbo].[MARS_USAGE_STATISTICS] (
    [reportId]              INT          NULL,
    [country]               VARCHAR (10) NULL,
    [cms_pool_id]           INT          NULL,
    [cms_location_group_id] INT          NULL,
    [ops_region_id]         INT          NULL,
    [ops_area_id]           INT          NULL,
    [location]              VARCHAR (50) NULL,
    [racfId]                VARCHAR (50) NULL,
    [report_date]           DATETIME     NULL,
    [UsageStatisticsId]     INT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_MARS_USAGE_STATISTICS] PRIMARY KEY CLUSTERED ([UsageStatisticsId] ASC),
    CONSTRAINT [FK_MARS_USAGE_STATISTICS_CMS_POOLS] FOREIGN KEY ([cms_pool_id]) REFERENCES [dbo].[CMS_POOLS] ([cms_pool_id]),
    CONSTRAINT [FK_MARS_USAGE_STATISTICS_COUNTRIES] FOREIGN KEY ([country]) REFERENCES [dbo].[COUNTRIES] ([country]),
    CONSTRAINT [FK_MARS_USAGE_STATISTICS_MARS_REPORT_NAMES] FOREIGN KEY ([reportId]) REFERENCES [dbo].[MARS_REPORT_NAMES] ([marsReportId]),
    CONSTRAINT [FK_MARS_USAGE_STATISTICS_OPS_AREAS] FOREIGN KEY ([ops_area_id]) REFERENCES [dbo].[OPS_AREAS] ([ops_area_id]),
    CONSTRAINT [FK_MARS_USAGE_STATISTICS_OPS_REGIONS] FOREIGN KEY ([ops_region_id]) REFERENCES [dbo].[OPS_REGIONS] ([ops_region_id])
);




GO
CREATE NONCLUSTERED INDEX [RACFID]
    ON [dbo].[MARS_USAGE_STATISTICS]([racfId] ASC);


GO
CREATE NONCLUSTERED INDEX [ReportDate]
    ON [dbo].[MARS_USAGE_STATISTICS]([report_date] ASC);


GO
CREATE NONCLUSTERED INDEX [ReportID]
    ON [dbo].[MARS_USAGE_STATISTICS]([reportId] ASC);

