CREATE TABLE [dbo].[LogDashboardRestrictAreas] (
    [RestrictAreaID] INT          IDENTITY (1, 1) NOT NULL,
    [Country]        VARCHAR (50) NULL,
    [CountryCode]    NCHAR (10)   NULL,
    [AreaType]       VARCHAR (50) NULL,
    [AreaName]       VARCHAR (50) NULL,
    CONSTRAINT [PK_Dimension.RestrictAreas] PRIMARY KEY CLUSTERED ([RestrictAreaID] ASC)
);

