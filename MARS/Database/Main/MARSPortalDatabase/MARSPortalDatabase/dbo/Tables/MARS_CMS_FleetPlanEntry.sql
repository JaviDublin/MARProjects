CREATE TABLE [dbo].[MARS_CMS_FleetPlanEntry] (
    [PKID]             INT          IDENTITY (1, 1) NOT NULL,
    [Country]          VARCHAR (10) NULL,
    [fleetPlanID]      TINYINT      NOT NULL,
    [uploadedBy]       VARCHAR (10) NOT NULL,
    [uploadedDate]     DATETIME     NOT NULL,
    [uploadedFileName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_MARS_CMS_FleetPlanEntry] PRIMARY KEY CLUSTERED ([PKID] ASC)
);

