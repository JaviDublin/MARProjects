CREATE TABLE [dbo].[ResDeletions] (
    [Id]       INT      IDENTITY (1, 1) NOT NULL,
    [LocId]    INT      NOT NULL,
    [CarGrpId] INT      NOT NULL,
    [RepDate]  DATETIME NOT NULL,
    [Value]    INT      NOT NULL,
    CONSTRAINT [PK_ResDeletions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ResDeletions_CAR_GROUPS] FOREIGN KEY ([CarGrpId]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id]),
    CONSTRAINT [FK_ResDeletions_LOCATIONS] FOREIGN KEY ([LocId]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id])
);



