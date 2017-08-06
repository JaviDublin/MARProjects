CREATE TABLE [dbo].[MARS_CMS_FleetPlanEntryUploadArchive] (
    [PKID]             INT           IDENTITY (1, 1) NOT NULL,
    [Country]          VARCHAR (10)  NULL,
    [fleetPlanID]      TINYINT       NOT NULL,
    [uploadedBy]       VARCHAR (100) NOT NULL,
    [uploadedDate]     DATETIME      NOT NULL,
    [uploadedFileName] VARCHAR (50)  NOT NULL,
    [archiveFileName]  VARCHAR (500) NULL,
    [isAddition]       BIT           NULL
);

