CREATE TABLE [Settings].[NonRev_Day_Groups] (
    [NonRevDayGroupId] INT          IDENTITY (1, 1) NOT NULL,
    [DayGroupCode]     VARCHAR (10) NULL,
    [DayMin]           INT          NULL,
    [DayMax]           INT          NULL,
    [DayGroupName]     VARCHAR (20) NULL,
    PRIMARY KEY CLUSTERED ([NonRevDayGroupId] ASC)
);

