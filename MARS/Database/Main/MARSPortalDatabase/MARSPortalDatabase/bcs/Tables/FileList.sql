CREATE TABLE [bcs].[FileList] (
    [FileId]          INT           IDENTITY (1, 1) NOT NULL,
    [FileMask]        VARCHAR (255) NOT NULL,
    [FilePath]        VARCHAR (MAX) NOT NULL,
    [FilePathTemp]    VARCHAR (MAX) NULL,
    [FilePathArchive] VARCHAR (MAX) NULL,
    [BatchType]       VARCHAR (100) NULL,
    [IsActive]        BIT           NOT NULL,
    [HasDate]         BIT           NULL,
    [BatchTime]       VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([FileId] ASC)
);

