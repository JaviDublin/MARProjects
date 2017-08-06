CREATE TABLE [General].[Fact_NonRevLog] (
    [NonRevLogId]      INT            IDENTITY (1, 1) NOT NULL,
    [VehicleId]        INT            NULL,
    [NRdays]           INT            NULL,
    [BDDays]           INT            NULL,
    [MMDays]           INT            NULL,
    [Lstwwd]           VARCHAR (50)   NULL,
    [LstNo]            VARCHAR (20)   NULL,
    [LstDate]          DATETIME       NULL,
    [Duewwd]           VARCHAR (50)   NULL,
    [DueDate]          DATETIME       NULL,
    [Prevwwd]          VARCHAR (50)   NULL,
    [LstMlg]           FLOAT (53)     NULL,
    [DrvName]          VARCHAR (255)  NULL,
    [StartDate]        DATETIME       NULL,
    [EndDate]          DATETIME       NULL,
    [OperStat]         VARCHAR (5)    NULL,
    [MoveType]         VARCHAR (5)    NULL,
    [DepStat]          VARCHAR (5)    NULL,
    [CarHold]          VARCHAR (5)    NULL,
    [IDate]            DATETIME       NULL,
    [SDate]            DATETIME       NULL,
    [ERDate]           DATETIME       NULL,
    [Remark]           VARCHAR (4000) NULL,
    [RemarkId]         INT            NULL,
    [IsOpen]           BIT            NULL,
    [Fleet_rac_ops]    BIT            NULL,
    [Fleet_rac_ttl]    BIT            NULL,
    [Fleet_carsales]   BIT            NULL,
    [Fleet_licensee]   BIT            NULL,
    [Fleet_adv]        BIT            NULL,
    [Fleet_hod]        BIT            NULL,
    [TotalFleet]       INT            NULL,
    [AvailableFleet]   INT            NULL,
    [OperationalFleet] INT            NULL,
    [KCICode]          VARCHAR (10)   NULL,
    [DayGroupCode]     VARCHAR (10)   NULL,
    [IsApproved]       BIT            NULL,
    [RacfId]           VARCHAR (20)   NULL,
    [ApprovalDate]     DATETIME       NULL,
    [RemarkIdDate]     DATETIME       NULL,
    [LocationId]       INT            NULL,
    [CountryLoc]       VARCHAR (10)   NULL,
    [CarGroup]         VARCHAR (10)   NULL,
    [CountryCar]       VARCHAR (5)    NULL,
    [RemarkRacfid]     VARCHAR (20)   NULL,
    PRIMARY KEY CLUSTERED ([NonRevLogId] ASC),
    CONSTRAINT [FK_Fact_NonRevLog_Dim_Fleet] FOREIGN KEY ([VehicleId]) REFERENCES [General].[Dim_Fleet] ([VehicleId])
);










GO
CREATE NONCLUSTERED INDEX [Fact_NonRevLog_IsOpen_Index]
    ON [General].[Fact_NonRevLog]([IsOpen] ASC)
    INCLUDE([VehicleId], [Lstwwd], [OperStat], [Fleet_rac_ops], [Fleet_rac_ttl], [Fleet_carsales], [Fleet_licensee], [Fleet_adv], [Fleet_hod], [CountryCar]);


GO
CREATE NONCLUSTERED INDEX [Fact_NonRev_RemarkId]
    ON [General].[Fact_NonRevLog]([RemarkId] ASC)
    INCLUDE([Lstwwd], [StartDate], [EndDate], [OperStat], [KCICode], [CountryLoc], [CarGroup], [CountryCar]);

