CREATE TABLE [Settings].[Operational_Status] (
    [OperationalStatusId]   INT          IDENTITY (1, 1) NOT NULL,
    [OperationalStatusCode] VARCHAR (5)  NOT NULL,
    [OperationalStatusName] VARCHAR (50) NULL,
    [KCICode]               VARCHAR (10) NULL,
    PRIMARY KEY CLUSTERED ([OperationalStatusId] ASC)
);

