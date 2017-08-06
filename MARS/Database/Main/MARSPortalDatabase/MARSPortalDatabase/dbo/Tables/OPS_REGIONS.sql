CREATE TABLE [dbo].[OPS_REGIONS] (
    [ops_region_id] INT          IDENTITY (1, 1) NOT NULL,
    [ops_region]    VARCHAR (50) NOT NULL,
    [country]       VARCHAR (10) NOT NULL,
    [isActive]      BIT          CONSTRAINT [DF_OPS_REGIONS_isActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_OPS_REGIONS] PRIMARY KEY CLUSTERED ([ops_region_id] ASC),
    CONSTRAINT [FK_OPS_REGIONS_OPS_REGIONS] FOREIGN KEY ([country]) REFERENCES [dbo].[COUNTRIES] ([country])
);

