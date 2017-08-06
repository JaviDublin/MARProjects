CREATE TABLE [dbo].[VehicleNonRevApproval] (
    [VehicleNonRevApprovalId] INT           IDENTITY (1, 1) NOT NULL,
    [UserId]                  VARCHAR (20)  NULL,
    [ApprovedOn]              DATETIME      NOT NULL,
    [VehiclesApproved]        INT           NOT NULL,
    [OwningCountry]           CHAR (2)      NULL,
    [LocationCountry]         CHAR (2)      NULL,
    [OperationalStatusIds]    VARCHAR (200) NULL,
    [MinimumDaysNonRev]       INT           NULL,
    CONSTRAINT [PK_VehicleNonRevApproval] PRIMARY KEY CLUSTERED ([VehicleNonRevApprovalId] ASC)
);

