CREATE TABLE [Fao].[WeeklyLimitOnCarSegment] (
    [WeeklyLimitOnCarSegmentId] INT      IDENTITY (1, 1) NOT NULL,
    [FileUploadId]              INT      NOT NULL,
    [CarSegmentId]              INT      NOT NULL,
    [Additions]                 INT      NOT NULL,
    [Week]                      TINYINT  NOT NULL,
    [Year]                      SMALLINT NOT NULL,
    CONSTRAINT [PK_PlannedAdditionWeekly] PRIMARY KEY CLUSTERED ([WeeklyLimitOnCarSegmentId] ASC),
    CONSTRAINT [FK_PlannedAdditionWeekly_CAR_SEGMENTS] FOREIGN KEY ([CarSegmentId]) REFERENCES [dbo].[CAR_SEGMENTS] ([car_segment_id]),
    CONSTRAINT [FK_PlannedAdditionWeekly_FileUpload] FOREIGN KEY ([FileUploadId]) REFERENCES [Fao].[FileUpload] ([FileUploadId])
);

