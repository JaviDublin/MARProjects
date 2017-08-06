CREATE TABLE [dbo].[CMS_LOCATION_GROUPS] (
    [cms_location_group_id]      INT          IDENTITY (200, 1) NOT NULL,
    [cms_location_group_code_dw] VARCHAR (10) NOT NULL,
    [cms_location_group]         VARCHAR (50) NOT NULL,
    [cms_pool_id]                INT          NOT NULL,
    [IsActive]                   BIT          NULL,
    CONSTRAINT [PK_POOLS] PRIMARY KEY CLUSTERED ([cms_location_group_id] ASC),
    CONSTRAINT [FK_CMS_LOCATION_GROUPS_CMS_POOLS] FOREIGN KEY ([cms_pool_id]) REFERENCES [dbo].[CMS_POOLS] ([cms_pool_id])
);






GO
CREATE STATISTICS [manualStats]
    ON [dbo].[CMS_LOCATION_GROUPS]([cms_location_group_id]);

