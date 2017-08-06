CREATE TABLE [dbo].[ResErrorType] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [ErrorType] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ResErrorType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

