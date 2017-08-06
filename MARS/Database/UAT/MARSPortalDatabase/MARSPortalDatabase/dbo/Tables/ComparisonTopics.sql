CREATE TABLE [dbo].[ComparisonTopics] (
    [ComparisonTopicId] INT            IDENTITY (1, 1) NOT NULL,
    [ComparisonName]    NVARCHAR (50)  NULL,
    [Description]       NVARCHAR (200) NULL,
    CONSTRAINT [PK_ComparisonTopics] PRIMARY KEY CLUSTERED ([ComparisonTopicId] ASC)
);

