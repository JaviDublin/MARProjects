CREATE TABLE [dbo].[CMS_POOLS] (
    [cms_pool_id] INT          IDENTITY (1, 1) NOT NULL,
    [cms_pool]    VARCHAR (50) NOT NULL,
    [country]     VARCHAR (10) NOT NULL,
    [IsActive]    BIT          NULL,
    CONSTRAINT [PK_REGIONS] PRIMARY KEY CLUSTERED ([cms_pool_id] ASC),
    CONSTRAINT [FK_REGIONS_COUNTRIES] FOREIGN KEY ([country]) REFERENCES [dbo].[COUNTRIES] ([country])
);



