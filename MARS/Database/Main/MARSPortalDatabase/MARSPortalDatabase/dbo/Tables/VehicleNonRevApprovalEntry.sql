CREATE TABLE [dbo].[VehicleNonRevApprovalEntry] (
    [VehicleNonRevApprovalEntryId] INT            IDENTITY (1, 1) NOT NULL,
    [VehicleNonRevApprovalId]      INT            NOT NULL,
    [DaysNonRevAtApproval]         INT            NOT NULL,
    [OwningCountry]                VARCHAR (2)    NOT NULL,
    [OwningArea]                   NCHAR (5)      NOT NULL,
    [LicencePlate]                 VARCHAR (20)   NOT NULL,
    [Vin]                          VARCHAR (20)   NOT NULL,
    [Reason]                       VARCHAR (100)  NULL,
    [ExpectedResolutionDate]       DATETIME       NULL,
    [Remark]                       VARCHAR (2000) NULL,
    [UserId]                       NCHAR (20)     NOT NULL,
    [ApprovedDateTime]             DATETIME       NOT NULL,
    [ModelDescription]             VARCHAR (50)   NOT NULL,
    [OperationalStatus]            VARCHAR (5)    NOT NULL,
    [MovementType]                 VARCHAR (5)    NOT NULL,
    [FleetType]                    VARCHAR (20)   NOT NULL,
    [CarGroup]                     VARCHAR (10)   NOT NULL,
    [UnitNumber]                   INT            NOT NULL,
    CONSTRAINT [PK_VehicleNonRevApprovalEntry] PRIMARY KEY CLUSTERED ([VehicleNonRevApprovalEntryId] ASC),
    CONSTRAINT [FK_VehicleNonRevApprovalEntry_VehicleNonRevApproval] FOREIGN KEY ([VehicleNonRevApprovalId]) REFERENCES [dbo].[VehicleNonRevApproval] ([VehicleNonRevApprovalId])
);




GO



GO
CREATE NONCLUSTERED INDEX [IX_VehicleNonRevApproval]
    ON [dbo].[VehicleNonRevApprovalEntry]([VehicleNonRevApprovalId] ASC);

