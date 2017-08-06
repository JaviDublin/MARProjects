CREATE TABLE [General].[Fact_NonRevLog_Approval] (
    [ApprovalId]     INT            IDENTITY (1, 1) NOT NULL,
    [NonRevLogId]    INT            NULL,
    [VehicleId]      INT            NULL,
    [ApprovalDate]   DATETIME       NULL,
    [RacfId]         VARCHAR (20)   NULL,
    [ERDate]         DATETIME       NULL,
    [Remark]         VARCHAR (4000) NULL,
    [RemarkId]       INT            NULL,
    [ApprovalDateId] INT            NULL,
    PRIMARY KEY CLUSTERED ([ApprovalId] ASC)
);

