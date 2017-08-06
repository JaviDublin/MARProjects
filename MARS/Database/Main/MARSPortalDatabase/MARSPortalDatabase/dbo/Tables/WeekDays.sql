CREATE TABLE [dbo].[WeekDays] (
    [DayOfWeekId] TINYINT      IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_DayOfWeek] PRIMARY KEY CLUSTERED ([DayOfWeekId] ASC)
);

