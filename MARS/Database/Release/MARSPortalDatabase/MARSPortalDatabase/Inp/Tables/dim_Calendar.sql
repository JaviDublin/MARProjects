CREATE TABLE [Inp].[dim_Calendar] (
    [dim_Calendar_id] INT      NOT NULL,
    [Rep_Date]        DATETIME NOT NULL,
    [FirstDayOfMonth] DATETIME NOT NULL,
    [LastDayOfMonth]  DATETIME NOT NULL,
    [Rep_Year]        INT      NOT NULL,
    [Rep_Month]       INT      NOT NULL,
    [Rep_WeekOfYear]  INT      NOT NULL,
    [FirstDayOfWeek]  DATETIME NOT NULL,
    [LastDayOfWeek]   DATETIME NOT NULL,
    [Rep_DayOfWeek]   INT      NOT NULL,
    PRIMARY KEY CLUSTERED ([dim_Calendar_id] ASC)
);

