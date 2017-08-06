CREATE TABLE [Fao].[MonthlyLimitOnCarGroup] (
    [MonthlyLimitOnCarGroupId] INT      IDENTITY (1, 1) NOT NULL,
    [FileUploadId]             INT      NOT NULL,
    [CarGroupId]               INT      NOT NULL,
    [Additions]                INT      NOT NULL,
    [Month]                    TINYINT  NOT NULL,
    [Year]                     SMALLINT NOT NULL,
    CONSTRAINT [PK_PlannedAdditionMonthly] PRIMARY KEY CLUSTERED ([MonthlyLimitOnCarGroupId] ASC),
    CONSTRAINT [FK_PlannedAdditionMonthly_CAR_GROUPS] FOREIGN KEY ([CarGroupId]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id]),
    CONSTRAINT [FK_PlannedAdditionMonthly_FileUpload] FOREIGN KEY ([FileUploadId]) REFERENCES [Fao].[FileUpload] ([FileUploadId])
);

