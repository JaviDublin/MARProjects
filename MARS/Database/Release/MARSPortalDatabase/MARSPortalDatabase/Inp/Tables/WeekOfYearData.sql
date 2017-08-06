CREATE TABLE [Inp].[WeekOfYearData] (
    [PeriodStart]         DATETIME NOT NULL,
    [PeriodEnd]           DATETIME NOT NULL,
    [min_dim_Calendar_id] INT      NOT NULL,
    [max_dim_Calendar_id] INT      NOT NULL,
    CONSTRAINT [pk_WeekOfYearData] PRIMARY KEY CLUSTERED ([PeriodStart] ASC)
);

