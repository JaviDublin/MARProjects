CREATE TABLE [dbo].[OPERSTATS] (
    [operstat_name]  VARCHAR (50) NOT NULL,
    [sort_operstats] INT          NULL,
    [operstat_desc]  VARCHAR (50) NULL,
    CONSTRAINT [PK_STATUSES_1] PRIMARY KEY CLUSTERED ([operstat_name] ASC)
);

