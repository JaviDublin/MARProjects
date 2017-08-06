CREATE TABLE [dbo].[CAR_GROUPS] (
    [car_group_id]              INT         IDENTITY (1, 1) NOT NULL,
    [car_group]                 VARCHAR (3) NOT NULL,
    [car_group_gold]            VARCHAR (3) NULL,
    [car_class_id]              INT         NOT NULL,
    [sort_car_group]            INT         NOT NULL,
    [car_group_fivestar]        VARCHAR (3) NULL,
    [car_group_presidentCircle] VARCHAR (3) NULL,
    [car_group_platinum]        VARCHAR (3) NULL,
    CONSTRAINT [PK_GROUPS] PRIMARY KEY CLUSTERED ([car_group_id] ASC),
    CONSTRAINT [FK_CAR_GROUPS_CAR_CLASSES] FOREIGN KEY ([car_class_id]) REFERENCES [dbo].[CAR_CLASSES] ([car_class_id])
);



