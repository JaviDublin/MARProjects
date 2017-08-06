CREATE TABLE [dbo].[VehicleNonRevApprovalEntry] (
    [VehicleNonRevApprovalEntryId] INT IDENTITY (1, 1) NOT NULL,
    [VehicleNonRevApprovalId]      INT NOT NULL,
    [VehicleNonRevPeriodEntryId]   INT NOT NULL,
    [DaysNonRevAtApproval]         INT NOT NULL,
    CONSTRAINT [PK_VehicleNonRevApprovalEntry] PRIMARY KEY CLUSTERED ([VehicleNonRevApprovalEntryId] ASC),
    CONSTRAINT [FK_VehicleNonRevApprovalEntry_VehicleNonRevApproval] FOREIGN KEY ([VehicleNonRevApprovalId]) REFERENCES [dbo].[VehicleNonRevApproval] ([VehicleNonRevApprovalId]),
    CONSTRAINT [FK_VehicleNonRevApprovalEntry_VehicleNonRevPeriodEntry] FOREIGN KEY ([VehicleNonRevPeriodEntryId]) REFERENCES [dbo].[VehicleNonRevPeriodEntry] ([VehicleNonRevPeriodEntryId])
);


GO
CREATE NONCLUSTERED INDEX [IX_VehicleNonRevPeriodEntry]
    ON [dbo].[VehicleNonRevApprovalEntry]([VehicleNonRevPeriodEntryId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_VehicleNonRevApproval]
    ON [dbo].[VehicleNonRevApprovalEntry]([VehicleNonRevApprovalId] ASC);

