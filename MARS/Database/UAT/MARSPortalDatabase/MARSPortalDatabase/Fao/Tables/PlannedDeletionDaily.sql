CREATE TABLE [Fao].[PlannedDeletionDaily] (
    [PlannedDeletionDailyId] INT      IDENTITY (1, 1) NOT NULL,
    [FileUploadId]           INT      NOT NULL,
    [LocationId]             INT      NOT NULL,
    [CarGroupId]             INT      NOT NULL,
    [Deletions]              INT      NOT NULL,
    [Week]                   TINYINT  NOT NULL,
    [Year]                   SMALLINT NOT NULL,
    CONSTRAINT [PK_PlannedDeletionDaily] PRIMARY KEY CLUSTERED ([PlannedDeletionDailyId] ASC),
    CONSTRAINT [FK_PlannedDeletionDaily_CAR_GROUPS] FOREIGN KEY ([CarGroupId]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id]),
    CONSTRAINT [FK_PlannedDeletionDaily_LOCATIONS] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id]),
    CONSTRAINT [FK_PlannedDeletionDaily_PredictionModelFileUpload] FOREIGN KEY ([FileUploadId]) REFERENCES [Fao].[FileUpload] ([FileUploadId])
);

