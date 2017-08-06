﻿CREATE TABLE [General].[Fact_NonRevLog_DailyReport] (
    [StatId]                    INT           IDENTITY (1, 1) NOT NULL,
    [Rep_Date]                  DATETIME      NULL,
    [dim_calendar_id]           INT           NULL,
    [Rep_Year]                  INT           NULL,
    [Rep_Month]                 INT           NULL,
    [Rep_WeekOfYear]            INT           NULL,
    [Rep_DayOfWeek]             INT           NULL,
    [CountryCar]                VARCHAR (10)  NULL,
    [CountryLoc]                VARCHAR (10)  NULL,
    [LocationCode]              VARCHAR (50)  NULL,
    [LocationId]                INT           NULL,
    [CarGroup]                  VARCHAR (50)  NULL,
    [CarGroupId]                INT           NULL,
    [OperStat]                  VARCHAR (10)  NULL,
    [DayGroupCode]              VARCHAR (10)  NULL,
    [TotalFleet]                INT           NULL,
    [TotalFleet_CarSales]       INT           NULL,
    [TotalFleet_RacOps]         INT           NULL,
    [TotalFleet_RacTtl]         INT           NULL,
    [TotalFleet_Hod]            INT           NULL,
    [TotalFleet_Adv]            INT           NULL,
    [TotalFleet_Licensee]       INT           NULL,
    [OperationalFleet]          INT           NULL,
    [OperationalFleet_CarSales] INT           NULL,
    [OperationalFleet_RacOps]   INT           NULL,
    [OperationalFleet_RacTtl]   INT           NULL,
    [OperationalFleet_Hod]      INT           NULL,
    [OperationalFleet_Adv]      INT           NULL,
    [OperationalFleet_Licensee] INT           NULL,
    [TotalFleetNR]              INT           NULL,
    [TotalFleetNR_CarSales]     INT           NULL,
    [TotalFleetNR_RacOps]       INT           NULL,
    [TotalFleetNR_RacTtl]       INT           NULL,
    [TotalFleetNR_Hod]          INT           NULL,
    [TotalFleetNR_Adv]          INT           NULL,
    [TotalFleetNR_Licensee]     INT           NULL,
    [TotalFleetNR_Remark]       INT           NULL,
    [Cms_Group_Id]              INT           NULL,
    [Cms_Group]                 VARCHAR (255) NULL,
    [Cms_Pool_Id]               INT           NULL,
    [Cms_Pool]                  VARCHAR (255) NULL,
    [Ops_Area_Id]               INT           NULL,
    [Ops_Area]                  VARCHAR (255) NULL,
    [Ops_Region_Id]             INT           NULL,
    [Ops_Region]                VARCHAR (255) NULL,
    [Car_Segment_Id]            INT           NULL,
    [Car_Segment]               VARCHAR (255) NULL,
    [Car_Class_Id]              INT           NULL,
    [Car_Class]                 VARCHAR (255) NULL,
    [rns]                       INT           NULL,
    PRIMARY KEY CLUSTERED ([StatId] ASC)
);
