﻿CREATE TABLE [dbo].[VehicleNonRevPeriodEntryRemark] (
    [VehicleNonRevPeriodEntryRemarkId] INT             IDENTITY (1, 1) NOT NULL,
    [VehicleNonRevPeriodEntryId]       INT             NOT NULL,
    [RemarkId]                         INT             NOT NULL,
    [ExpectedResolutionDate]           DATETIME        NOT NULL,
    [Remark]                           NVARCHAR (2000) NULL,
    [TimeStamp]                        DATETIME        NOT NULL,
    [UserId]                           VARCHAR (20)    NULL,
    CONSTRAINT [PK_VehicleNonRevPeriodEntryRemark] PRIMARY KEY CLUSTERED ([VehicleNonRevPeriodEntryRemarkId] ASC) WITH (FILLFACTOR = 90, STATISTICS_NORECOMPUTE = ON),
    CONSTRAINT [FK_VehicleNonRevPeriodEntryRemark_NonRev_Remarks_List] FOREIGN KEY ([RemarkId]) REFERENCES [Settings].[NonRev_Remarks_List] ([RemarkId]),
    CONSTRAINT [FK_VehicleNonRevPeriodEntryRemark_VehicleNonRevPeriodEntry] FOREIGN KEY ([VehicleNonRevPeriodEntryId]) REFERENCES [dbo].[VehicleNonRevPeriodEntry] ([VehicleNonRevPeriodEntryId])
);


GO
ALTER TABLE [dbo].[VehicleNonRevPeriodEntryRemark] NOCHECK CONSTRAINT [FK_VehicleNonRevPeriodEntryRemark_NonRev_Remarks_List];


GO
ALTER TABLE [dbo].[VehicleNonRevPeriodEntryRemark] NOCHECK CONSTRAINT [FK_VehicleNonRevPeriodEntryRemark_VehicleNonRevPeriodEntry];




GO
CREATE NONCLUSTERED INDEX [IX_Remark]
    ON [dbo].[VehicleNonRevPeriodEntryRemark]([RemarkId] ASC) WITH (FILLFACTOR = 90, STATISTICS_NORECOMPUTE = ON);




GO
CREATE NONCLUSTERED INDEX [IX_VehicleNonRevPeriodEntry]
    ON [dbo].[VehicleNonRevPeriodEntryRemark]([VehicleNonRevPeriodEntryId] ASC) WITH (FILLFACTOR = 90, STATISTICS_NORECOMPUTE = ON);



