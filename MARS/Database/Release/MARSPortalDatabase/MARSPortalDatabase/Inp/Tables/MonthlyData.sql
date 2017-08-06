CREATE TABLE [Inp].[MonthlyData] (
    [MonthStart]          DATETIME NOT NULL,
    [MonthEnd]            DATETIME NOT NULL,
    [min_dim_Calendar_id] INT      NOT NULL,
    [max_dim_Calendar_id] INT      NOT NULL,
    CONSTRAINT [pk_MonthlyData] PRIMARY KEY CLUSTERED ([MonthStart] ASC)
);

