CREATE TABLE [bcs].[FileProcess] (
    [FileProcess_id] INT            IDENTITY (1, 1) NOT NULL,
    [Entity]         VARCHAR (50)   NOT NULL,
    [FileName]       VARCHAR (1000) NULL,
    [Status]         VARCHAR (100)  NOT NULL,
    [Data]           VARCHAR (MAX)  NULL,
    [z_inserted]     DATETIME       DEFAULT (getdate()) NOT NULL,
    [z_updated]      DATETIME       DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [pk_FileProcess] PRIMARY KEY CLUSTERED ([FileProcess_id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_FileProcess_01]
    ON [bcs].[FileProcess]([z_inserted] ASC, [FileProcess_id] ASC);

