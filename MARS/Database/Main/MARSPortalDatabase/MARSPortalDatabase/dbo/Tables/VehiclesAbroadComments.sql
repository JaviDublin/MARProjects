CREATE TABLE [dbo].[VehiclesAbroadComments] (
    [License]    VARCHAR (25)   NOT NULL,
    [Comment]    NVARCHAR (MAX) NULL,
    [UpdateDate] DATETIME       NULL,
    CONSTRAINT [PK_VehiclesAbroadComments] PRIMARY KEY CLUSTERED ([License] ASC)
);

