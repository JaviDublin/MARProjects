CREATE TABLE [dbo].[CMS_ForecastTypes] (
    [FCTypeID] TINYINT      NOT NULL,
    [FCType]   VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_CMS_ForecastTypes] PRIMARY KEY CLUSTERED ([FCTypeID] ASC)
);

