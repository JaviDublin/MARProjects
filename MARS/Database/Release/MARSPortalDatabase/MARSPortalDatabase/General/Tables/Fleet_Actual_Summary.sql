﻿CREATE TABLE [General].[Fleet_Actual_Summary] (
    [Rowid]                     INT           IDENTITY (1, 1) NOT NULL,
    [Rep_Date]                  DATETIME      NULL,
    [dim_calendar_id]           INT           NULL,
    [Rep_Year]                  INT           NULL,
    [Rep_Month]                 INT           NULL,
    [Rep_WeekOfYear]            INT           NULL,
    [Rep_DayOfWeek]             INT           NULL,
    [Country]                   VARCHAR (10)  NULL,
    [LocationCode]              VARCHAR (20)  NULL,
    [LocationId]                INT           NULL,
    [CarGroup]                  VARCHAR (20)  NULL,
    [CarGroupId]                INT           NULL,
    [OperStat]                  VARCHAR (10)  NULL,
    [TotalFleet]                INT           NULL,
    [TotalFleet_rac_ttl]        INT           NULL,
    [TotalFleet_rac_ops]        INT           NULL,
    [TotalFleet_carsales]       INT           NULL,
    [TotalFleet_licensee]       INT           NULL,
    [TotalFleet_adv]            INT           NULL,
    [TotalFleet_hod]            INT           NULL,
    [OperationalFleet]          INT           NULL,
    [OperationalFleet_rac_ttl]  INT           NULL,
    [OperationalFleet_rac_ops]  INT           NULL,
    [OperationalFleet_carsales] INT           NULL,
    [OperationalFleet_licensee] INT           NULL,
    [OperationalFleet_adv]      INT           NULL,
    [OperationalFleet_hod]      INT           NULL,
    [Cms_Pool_Id]               INT           NULL,
    [Cms_Group_Id]              INT           NULL,
    [Ops_Region_Id]             INT           NULL,
    [Ops_Area_Id]               INT           NULL,
    [Car_Segment_Id]            INT           NULL,
    [Car_Class_Id]              INT           NULL,
    [Cms_Pool]                  VARCHAR (100) NULL,
    [Cms_Group]                 VARCHAR (100) NULL,
    [Ops_Area]                  VARCHAR (100) NULL,
    [Ops_Region]                VARCHAR (100) NULL,
    [Car_Segment]               VARCHAR (100) NULL,
    [Car_Class]                 VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([Rowid] ASC)
);

