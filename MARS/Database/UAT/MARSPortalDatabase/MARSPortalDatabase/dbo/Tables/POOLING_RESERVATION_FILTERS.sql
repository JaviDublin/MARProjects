﻿CREATE TABLE [dbo].[POOLING_RESERVATION_FILTERS] (
    [filterId] INT          NOT NULL,
    [filter]   VARCHAR (50) NULL,
    CONSTRAINT [PK_POOLING_FILTERS] PRIMARY KEY CLUSTERED ([filterId] ASC)
);

