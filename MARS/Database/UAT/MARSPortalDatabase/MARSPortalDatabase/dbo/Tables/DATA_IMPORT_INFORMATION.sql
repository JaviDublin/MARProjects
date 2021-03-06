﻿CREATE TABLE [dbo].[DATA_IMPORT_INFORMATION] (
    [importId]        INT      IDENTITY (1, 1) NOT NULL,
    [importTypeId]    INT      NULL,
    [importTimeStamp] DATETIME NULL,
    [nextUpdateDue]   DATETIME NULL,
    CONSTRAINT [PK_DATA_IMPORT_INFORMATION_1] PRIMARY KEY CLUSTERED ([importId] ASC),
    CONSTRAINT [FK_DATA_IMPORT_INFORMATION_DATA_IMPORT_TYPES] FOREIGN KEY ([importTypeId]) REFERENCES [dbo].[DATA_IMPORT_TYPES] ([importTypeId]) ON DELETE CASCADE
);

