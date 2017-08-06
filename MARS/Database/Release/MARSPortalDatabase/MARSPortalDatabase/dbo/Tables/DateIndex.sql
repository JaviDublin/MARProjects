CREATE TABLE [dbo].[DateIndex] (
    [REP_DATE]          DATETIME     NOT NULL,
    [CalendarDay]       TINYINT      NOT NULL,
    [CalendarDayName]   VARCHAR (20) NOT NULL,
    [CalendarMonth]     TINYINT      NOT NULL,
    [CalendarMonthName] VARCHAR (20) NOT NULL,
    [CalendarYear]      SMALLINT     NOT NULL,
    [CalendarWeek]      TINYINT      NOT NULL,
    [CalendarQuarter]   TINYINT      NOT NULL,
    [CalendarSemester]  TINYINT      NOT NULL,
    PRIMARY KEY CLUSTERED ([REP_DATE] ASC)
);

