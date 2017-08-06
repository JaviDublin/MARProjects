CREATE TABLE [dbo].[UkAddDel24-03-2014] (
    [COUNTRY]            NVARCHAR (255) NULL,
    [REP_YEAR]           NVARCHAR (255) NULL,
    [REP_MONTH]          NVARCHAR (255) NULL,
    [REP_DATE]           DATETIME       NULL,
    [CMS_POOL]           NVARCHAR (255) NULL,
    [CMS_LOCATION_GROUP] NVARCHAR (255) NULL,
    [CAR_CLASS]          NVARCHAR (255) NULL,
    [CAR_GROUP]          NVARCHAR (255) NULL,
    [ADDITION]           FLOAT (53)     NULL,
    [DELETION]           FLOAT (53)     NULL,
    [F11]                FLOAT (53)     NULL,
    [F12]                FLOAT (53)     NULL,
    [F13]                FLOAT (53)     NULL,
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_UkAddDel24-03-2014] PRIMARY KEY CLUSTERED ([Id] ASC)
);

