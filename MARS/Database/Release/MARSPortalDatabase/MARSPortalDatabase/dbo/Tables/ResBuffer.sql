CREATE TABLE [dbo].[ResBuffer] (
    [Id]       INT IDENTITY (1, 1) NOT NULL,
    [LocId]    INT NOT NULL,
    [CarGrpId] INT NOT NULL,
    [Value]    INT NOT NULL,
    CONSTRAINT [PK_ResBuffer] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ResBuffer_CAR_GROUPS] FOREIGN KEY ([CarGrpId]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id]),
    CONSTRAINT [FK_ResBuffer_LOCATIONS] FOREIGN KEY ([LocId]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id])
);



