CREATE TABLE [dbo].[IsoWeekOfYear] (
    [IsoWeekOfYearId] INT     IDENTITY (1, 1) NOT NULL,
    [Day]             DATE    NOT NULL,
    [WeekOfYear]      TINYINT NOT NULL,
    CONSTRAINT [PK_IsoWeekOfYear] PRIMARY KEY CLUSTERED ([IsoWeekOfYearId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [DateIndex]
    ON [dbo].[IsoWeekOfYear]([Day] ASC)
    INCLUDE([WeekOfYear]);

