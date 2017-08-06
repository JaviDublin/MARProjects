CREATE TABLE [dbo].[StatisticsPageAccess] (
    [StatisticsPageAccessId] INT           IDENTITY (1, 1) NOT NULL,
    [Url]                    VARCHAR (200) NULL,
    [AccessedBy]             VARCHAR (50)  NULL,
    [AccessedOn]             DATETIME      NULL,
    [UserName]               VARCHAR (50)  NULL,
    CONSTRAINT [PK_StatisticsPageAccess] PRIMARY KEY CLUSTERED ([StatisticsPageAccessId] ASC)
);

