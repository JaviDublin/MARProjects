CREATE TABLE [Export].[Status] (
    [Id]     INT          IDENTITY (1, 1) NOT NULL,
    [Status] VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED ([Id] ASC)
);

