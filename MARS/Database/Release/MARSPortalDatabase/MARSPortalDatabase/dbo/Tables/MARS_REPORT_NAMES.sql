CREATE TABLE [dbo].[MARS_REPORT_NAMES] (
    [marsReportId]   INT          NOT NULL,
    [marsToolId]     INT          NULL,
    [marsReportName] VARCHAR (50) NULL,
    CONSTRAINT [PK_MARS_REPORT_NAMES] PRIMARY KEY CLUSTERED ([marsReportId] ASC),
    CONSTRAINT [FK_MARS_REPORT_NAMES_MARS_TOOLS] FOREIGN KEY ([marsToolId]) REFERENCES [dbo].[MARS_TOOLS] ([marsToolId])
);

