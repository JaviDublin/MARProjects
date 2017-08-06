CREATE TABLE [dbo].[OPS_AREAS] (
    [ops_area_id]   INT          IDENTITY (1, 1) NOT NULL,
    [ops_area]      VARCHAR (50) NOT NULL,
    [ops_region_id] INT          NOT NULL,
    [isActive]      BIT          CONSTRAINT [DF_OPS_AREAS_isActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_OPS_AREAS] PRIMARY KEY CLUSTERED ([ops_area_id] ASC),
    CONSTRAINT [FK_OPS_AREAS_OPS_REGIONS] FOREIGN KEY ([ops_region_id]) REFERENCES [dbo].[OPS_REGIONS] ([ops_region_id])
);

