﻿CREATE TABLE [dbo].[POOLING_TOPICS] (
    [topicId]   INT           NOT NULL,
    [topicName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_POOLING_TOPICS] PRIMARY KEY CLUSTERED ([topicId] ASC)
);
