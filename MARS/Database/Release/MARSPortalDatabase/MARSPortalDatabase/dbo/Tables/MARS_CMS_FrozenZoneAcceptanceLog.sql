CREATE TABLE [dbo].[MARS_CMS_FrozenZoneAcceptanceLog] (
    [PKID]               INT          IDENTITY (1, 1) NOT NULL,
    [Country]            VARCHAR (10) NULL,
    [Year]               CHAR (10)    NOT NULL,
    [acceptedWeekNumber] INT          NOT NULL,
    [acceptedBy]         VARCHAR (50) NOT NULL,
    [acceptedDate]       DATETIME     NOT NULL
);

