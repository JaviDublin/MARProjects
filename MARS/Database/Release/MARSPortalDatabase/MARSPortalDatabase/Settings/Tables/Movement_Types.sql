CREATE TABLE [Settings].[Movement_Types] (
    [MovementTypeId]   INT          IDENTITY (1, 1) NOT NULL,
    [MovementTypeCode] VARCHAR (5)  NOT NULL,
    [MovementTypeName] VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([MovementTypeId] ASC)
);

