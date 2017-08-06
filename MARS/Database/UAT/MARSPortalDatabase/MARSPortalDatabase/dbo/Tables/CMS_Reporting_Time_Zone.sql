CREATE TABLE [dbo].[CMS_Reporting_Time_Zone] (
    [zoneID]          TINYINT      NOT NULL,
    [zoneDescription] VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_CMS_Reporing_Time_Zone] PRIMARY KEY CLUSTERED ([zoneID] ASC)
);

