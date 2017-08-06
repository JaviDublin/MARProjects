CREATE TABLE [Fao].[IsoWeekToMonth] (
    [IsoWeekToMonthId] INT      IDENTITY (1, 1) NOT NULL,
    [IsoWeekNumber]    TINYINT  NOT NULL,
    [Year]             SMALLINT NOT NULL,
    [Month]            TINYINT  NOT NULL,
    CONSTRAINT [PK_IsoWeekToMonth] PRIMARY KEY CLUSTERED ([IsoWeekToMonthId] ASC)
);

