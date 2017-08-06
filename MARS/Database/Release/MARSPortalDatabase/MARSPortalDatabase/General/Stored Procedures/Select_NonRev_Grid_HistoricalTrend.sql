-- =============================================
-- Author:		Javier
-- Create date: January 2013
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Select_NonRev_Grid_HistoricalTrend]
		
		@operstat_name			VARCHAR(300)	= NULL,
		@dateRangeValue			VARCHAR(20)		= NULL,
		
		@customStartDate		VARCHAR(30)		= NULL,
		@customEndDate			VARCHAR(30)		= NULL,
		@dateComparison			VARCHAR(30)		= NULL,
		@dateRange				VARCHAR(50)		= NULL,
		@country				VARCHAR(10)		= NULL,
		@cms_pool_id			INT				= NULL,
		@cms_location_group_id	INT				= NULL, 
		@ops_region_id			INT				= NULL,
		@ops_area_id			INT				= NULL,	
		@location				VARCHAR(50)		= NULL,
		@car_segment_id			INT				= NULL,
		@car_class_id			INT				= NULL,
		@car_group_id			INT				= NULL,
		@daygroupcode			VARCHAR(100)	= NULL,
		@day_of_week			INT				= NULL,
		@fleet_name				VARCHAR(50)		= NULL,
		@sortExpression			VARCHAR(50)		= NULL,
		@startRowIndex			INT				= NULL,
		@maximumRows			INT				= NULL
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	IF @customStartDate = '01/01/0001 00:00:00' BEGIN SET @customStartDate	= NULL END
	IF @customEndDate	= '01/01/0001 00:00:00' BEGIN SET @customEndDate	= NULL END
	IF @dateComparison	= '01/01/0001 00:00:00' BEGIN SET @dateComparison	= NULL END
	
	IF @customStartDate IS NULL AND @customEndDate IS NULL AND @dateComparison IS NULL
	BEGIN
		SET @dateComparison = DATEADD(D,0,DATEDIFF(D,0,GETDATE()))
	END
	
	DECLARE @day_diff INT
		
	IF @dateRange IS NULL				BEGIN SET @day_diff = 0		END
	IF @dateRange = '1 day'				BEGIN SET @day_diff = 0		END
	IF @dateRange = 'Previous 7 days'	BEGIN SET @day_diff = 6		END
	IF @dateRange = 'Previous 30 days'	BEGIN SET @day_diff = 29	END
	IF @dateRange = 'Previous 90 days'	BEGIN SET @day_diff = 89	END
	IF @dateRange = 'Custom'			BEGIN SET @day_diff = 0		END  -- Initial Value
	
	DECLARE @theDateStart DATETIME
	DECLARE @theDateEnd   DATETIME
		
	IF @dateComparison IS NULL
	BEGIN
		SET @theDateStart = CONVERT(DATETIME,@customStartDate,103)
		SET @theDateEnd   = CONVERT(DATETIME,@customEndDate,103) 
		SET @day_diff	  = DATEDIFF(D,@theDateStart,@theDateEnd)	
	END
	ELSE
	BEGIN
		SET @theDateEnd		= CONVERT(DATETIME,@dateComparison,103)
		SET @theDateStart   = @theDateEnd - @day_diff 
	END
	
	DECLARE @start_calendar_id INT, @end_calendar_id INT
	
	SET @start_calendar_id = (SELECT dim_Calendar_id FROM Inp.dim_Calendar WHERE Rep_Date = @theDateStart)
	
	SET @end_calendar_id   = (SELECT dim_Calendar_id FROM Inp.dim_Calendar WHERE Rep_Date = DATEADD(D,0,DATEDIFF(D,0,@theDateEnd)))
	
	DECLARE @calendar_ids TABLE (dim_calendar_id INT)
	
	INSERT INTO @calendar_ids (dim_calendar_id)
	SELECT 
		dim_Calendar_id 
	FROM 
		Inp.dim_Calendar 
	WHERE 
		dim_Calendar_id BETWEEN @start_calendar_id AND @end_calendar_id
	
	DECLARE @PAGING TABLE
	(
		pageIndex		INT IDENTITY (1,1) NOT NULL,
		rowId			INT NULL
	)
	
	DECLARE @RESULT TABLE 
	(
		rowId			INT IDENTITY (1,1) NOT NULL,
		Rep_Date		DATETIME,
		Total_Fleet_NR	INT,
		CU				INT,
		HA				INT,
		HL				INT,
		LL				INT,
		NC				INT,
		PL				INT,
		TC				INT,
		SV				INT,
		WS				INT,
		BD				INT,
		MM				INT,
		TW				INT,
		TB				INT,
		FS				INT,
		RL				INT,
		RP				INT,	
		TN				INT,
		RT				INT,
		SU				INT,
		UNK				INT
	)
	
	INSERT INTO @RESULT
	(
		Rep_Date , Total_Fleet_NR , 
		CU , HA , HL , LL , NC , PL , TC , SV , WS , BD , MM , TW , TB , FS , RL , RP , TN , RT , SU , UNK
	)
	
	SELECT 
		--Dr.Rep_Year , Dr.Rep_Month , Dr.Rep_WeekOfYear ,
		Dr.Rep_Date , 
		CASE	
			WHEN @fleet_name = 'CARSALES'			THEN SUM(Dr.TotalFleetNR_CarSales) 
			WHEN @fleet_name = 'RAC OPS'			THEN SUM(Dr.TotalFleetNR_RacOps) 
			WHEN @fleet_name = 'RAC TTL'			THEN SUM(Dr.TotalFleetNR_RacTtl) 
			WHEN @fleet_name = 'ADVANTAGE'			THEN SUM(Dr.TotalFleetNR_Adv) 
			WHEN @fleet_name = 'HERTZ ON DEMAND'	THEN SUM(Dr.TotalFleetNR_Hod) 
			WHEN @fleet_name = 'LICENSEE'			THEN SUM(Dr.TotalFleetNR_Licensee) 
		ELSE
			SUM(Dr.TotalFleetNR_RacTtl)  + SUM(Dr.TotalFleetNR_CarSales) 
		END AS [Total_Fleet_NR],
			
			ISNULL(
		CASE	
WHEN @fleet_name = 'CARSALES'		 THEN SUM(case when Dr.OperStat = 'CU' then Dr.TotalFleetNR_CarSales else null end) 
WHEN @fleet_name = 'RAC OPS'		 THEN SUM(case when Dr.OperStat = 'CU' then Dr.TotalFleetNR_RacOps else null end) 
WHEN @fleet_name = 'RAC TTL'		 THEN SUM(case when Dr.OperStat = 'CU' then Dr.TotalFleetNR_RacTtl else null end) 
WHEN @fleet_name = 'ADVANTAGE'		 THEN SUM(case when Dr.OperStat = 'CU' then Dr.TotalFleetNR_Adv else null end) 
WHEN @fleet_name = 'HERTZ ON DEMAND' THEN SUM(case when Dr.OperStat = 'CU' then Dr.TotalFleetNR_Hod else null end) 
WHEN @fleet_name = 'LICENSEE'		 THEN SUM(case when Dr.OperStat = 'CU' then Dr.TotalFleetNR_Licensee else null end) 
		ELSE
			SUM(case when Dr.OperStat = 'CU' then Dr.TotalFleetNR_RacTtl + Dr.TotalFleetNR_CarSales else null end) 
		END ,0) AS [CU],
		
			ISNULL(
		CASE	
WHEN @fleet_name = 'CARSALES'		 THEN SUM(case when Dr.OperStat = 'HA' then Dr.TotalFleetNR_CarSales else null end) 
WHEN @fleet_name = 'RAC OPS'		 THEN SUM(case when Dr.OperStat = 'HA' then Dr.TotalFleetNR_RacOps else null end) 
WHEN @fleet_name = 'RAC TTL'		 THEN SUM(case when Dr.OperStat = 'HA' then Dr.TotalFleetNR_RacTtl else null end) 
WHEN @fleet_name = 'ADVANTAGE'		 THEN SUM(case when Dr.OperStat = 'HA' then Dr.TotalFleetNR_Adv else null end) 
WHEN @fleet_name = 'HERTZ ON DEMAND' THEN SUM(case when Dr.OperStat = 'HA' then Dr.TotalFleetNR_Hod else null end) 
WHEN @fleet_name = 'LICENSEE'		 THEN SUM(case when Dr.OperStat = 'HA' then Dr.TotalFleetNR_Licensee else null end) 
		ELSE
			SUM(case when Dr.OperStat = 'HA' then Dr.TotalFleetNR_RacTtl + Dr.TotalFleetNR_CarSales else null end) 
		END , 0) AS [HA],
				
			ISNULL(
		CASE	
WHEN @fleet_name = 'CARSALES'		 THEN SUM(case when Dr.OperStat = 'HL' then Dr.TotalFleetNR_CarSales else null end) 
WHEN @fleet_name = 'RAC OPS'		 THEN SUM(case when Dr.OperStat = 'HL' then Dr.TotalFleetNR_RacOps else null end) 
WHEN @fleet_name = 'RAC TTL'		 THEN SUM(case when Dr.OperStat = 'HL' then Dr.TotalFleetNR_RacTtl else null end) 
WHEN @fleet_name = 'ADVANTAGE'		 THEN SUM(case when Dr.OperStat = 'HL' then Dr.TotalFleetNR_Adv else null end) 
WHEN @fleet_name = 'HERTZ ON DEMAND' THEN SUM(case when Dr.OperStat = 'HL' then Dr.TotalFleetNR_Hod else null end) 
WHEN @fleet_name = 'LICENSEE'		 THEN SUM(case when Dr.OperStat = 'HL' then Dr.TotalFleetNR_Licensee else null end) 
		ELSE
			SUM(case when Dr.OperStat = 'HL' then Dr.TotalFleetNR_RacTtl + Dr.TotalFleetNR_CarSales else null end) 
		END , 0) AS [HL],
				
			ISNULL(
		CASE	
WHEN @fleet_name = 'CARSALES'		 THEN SUM(case when Dr.OperStat = 'LL' then Dr.TotalFleetNR_CarSales else null end) 
WHEN @fleet_name = 'RAC OPS'		 THEN SUM(case when Dr.OperStat = 'LL' then Dr.TotalFleetNR_RacOps else null end) 
WHEN @fleet_name = 'RAC TTL'		 THEN SUM(case when Dr.OperStat = 'LL' then Dr.TotalFleetNR_RacTtl else null end) 
WHEN @fleet_name = 'ADVANTAGE'		 THEN SUM(case when Dr.OperStat = 'LL' then Dr.TotalFleetNR_Adv else null end) 
WHEN @fleet_name = 'HERTZ ON DEMAND' THEN SUM(case when Dr.OperStat = 'LL' then Dr.TotalFleetNR_Hod else null end) 
WHEN @fleet_name = 'LICENSEE'		 THEN SUM(case when Dr.OperStat = 'LL' then Dr.TotalFleetNR_Licensee else null end) 
		ELSE
			SUM(case when Dr.OperStat = 'LL' then Dr.TotalFleetNR_RacTtl + Dr.TotalFleetNR_CarSales else null end) 
		END , 0) AS [LL],
				
			ISNULL(
		CASE	
WHEN @fleet_name = 'CARSALES'		 THEN SUM(case when Dr.OperStat = 'NC' then Dr.TotalFleetNR_CarSales else null end) 
WHEN @fleet_name = 'RAC OPS'		 THEN SUM(case when Dr.OperStat = 'NC' then Dr.TotalFleetNR_RacOps else null end) 
WHEN @fleet_name = 'RAC TTL'		 THEN SUM(case when Dr.OperStat = 'NC' then Dr.TotalFleetNR_RacTtl else null end) 
WHEN @fleet_name = 'ADVANTAGE'		 THEN SUM(case when Dr.OperStat = 'NC' then Dr.TotalFleetNR_Adv else null end) 
WHEN @fleet_name = 'HERTZ ON DEMAND' THEN SUM(case when Dr.OperStat = 'NC' then Dr.TotalFleetNR_Hod else null end) 
WHEN @fleet_name = 'LICENSEE'		 THEN SUM(case when Dr.OperStat = 'NC' then Dr.TotalFleetNR_Licensee else null end) 
		ELSE
			SUM(case when Dr.OperStat = 'NC' then Dr.TotalFleetNR_RacTtl + Dr.TotalFleetNR_CarSales else null end) 
		END , 0) AS [NC],
				
			ISNULL(
		CASE	
WHEN @fleet_name = 'CARSALES'		 THEN SUM(case when Dr.OperStat = 'PL' then Dr.TotalFleetNR_CarSales else null end) 
WHEN @fleet_name = 'RAC OPS'		 THEN SUM(case when Dr.OperStat = 'PL' then Dr.TotalFleetNR_RacOps else null end) 
WHEN @fleet_name = 'RAC TTL'		 THEN SUM(case when Dr.OperStat = 'PL' then Dr.TotalFleetNR_RacTtl else null end) 
WHEN @fleet_name = 'ADVANTAGE'		 THEN SUM(case when Dr.OperStat = 'PL' then Dr.TotalFleetNR_Adv else null end) 
WHEN @fleet_name = 'HERTZ ON DEMAND' THEN SUM(case when Dr.OperStat = 'PL' then Dr.TotalFleetNR_Hod else null end) 
WHEN @fleet_name = 'LICENSEE'		 THEN SUM(case when Dr.OperStat = 'PL' then Dr.TotalFleetNR_Licensee else null end) 
		ELSE
			SUM(case when Dr.OperStat = 'PL' then Dr.TotalFleetNR_RacTtl + Dr.TotalFleetNR_CarSales else null end) 
		END , 0 ) AS [PL],
				
			ISNULL(
		CASE	
WHEN @fleet_name = 'CARSALES'		 THEN SUM(case when Dr.OperStat = 'TC' then Dr.TotalFleetNR_CarSales else null end) 
WHEN @fleet_name = 'RAC OPS'		 THEN SUM(case when Dr.OperStat = 'TC' then Dr.TotalFleetNR_RacOps else null end) 
WHEN @fleet_name = 'RAC TTL'		 THEN SUM(case when Dr.OperStat = 'TC' then Dr.TotalFleetNR_RacTtl else null end) 
WHEN @fleet_name = 'ADVANTAGE'		 THEN SUM(case when Dr.OperStat = 'TC' then Dr.TotalFleetNR_Adv else null end) 
WHEN @fleet_name = 'HERTZ ON DEMAND' THEN SUM(case when Dr.OperStat = 'TC' then Dr.TotalFleetNR_Hod else null end) 
WHEN @fleet_name = 'LICENSEE'		 THEN SUM(case when Dr.OperStat = 'TC' then Dr.TotalFleetNR_Licensee else null end) 
		ELSE
			SUM(case when Dr.OperStat = 'TC' then Dr.TotalFleetNR_RacTtl + Dr.TotalFleetNR_CarSales else null end) 
		END , 0 ) AS [TC],
				
			ISNULL(
		CASE	
WHEN @fleet_name = 'CARSALES'		 THEN SUM(case when Dr.OperStat = 'SV' then Dr.TotalFleetNR_CarSales else null end) 
WHEN @fleet_name = 'RAC OPS'		 THEN SUM(case when Dr.OperStat = 'SV' then Dr.TotalFleetNR_RacOps else null end) 
WHEN @fleet_name = 'RAC TTL'		 THEN SUM(case when Dr.OperStat = 'SV' then Dr.TotalFleetNR_RacTtl else null end) 
WHEN @fleet_name = 'ADVANTAGE'		 THEN SUM(case when Dr.OperStat = 'SV' then Dr.TotalFleetNR_Adv else null end) 
WHEN @fleet_name = 'HERTZ ON DEMAND' THEN SUM(case when Dr.OperStat = 'SV' then Dr.TotalFleetNR_Hod else null end) 
WHEN @fleet_name = 'LICENSEE'		 THEN SUM(case when Dr.OperStat = 'SV' then Dr.TotalFleetNR_Licensee else null end) 
		ELSE
			SUM(case when Dr.OperStat = 'SV' then Dr.TotalFleetNR_RacTtl + Dr.TotalFleetNR_CarSales else null end) 
		END , 0 ) AS [SV],
				
			ISNULL(
		CASE	
WHEN @fleet_name = 'CARSALES'		 THEN SUM(case when Dr.OperStat = 'WS' then Dr.TotalFleetNR_CarSales else null end) 
WHEN @fleet_name = 'RAC OPS'		 THEN SUM(case when Dr.OperStat = 'WS' then Dr.TotalFleetNR_RacOps else null end) 
WHEN @fleet_name = 'RAC TTL'		 THEN SUM(case when Dr.OperStat = 'WS' then Dr.TotalFleetNR_RacTtl else null end) 
WHEN @fleet_name = 'ADVANTAGE'		 THEN SUM(case when Dr.OperStat = 'WS' then Dr.TotalFleetNR_Adv else null end) 
WHEN @fleet_name = 'HERTZ ON DEMAND' THEN SUM(case when Dr.OperStat = 'WS' then Dr.TotalFleetNR_Hod else null end) 
WHEN @fleet_name = 'LICENSEE'		 THEN SUM(case when Dr.OperStat = 'WS' then Dr.TotalFleetNR_Licensee else null end) 
		ELSE
			SUM(case when Dr.OperStat = 'WS' then Dr.TotalFleetNR_RacTtl + Dr.TotalFleetNR_CarSales else null end) 
		END , 0 ) AS [WS],

			ISNULL(
		CASE	
WHEN @fleet_name = 'CARSALES'		 THEN SUM(case when Dr.OperStat = 'BD' then Dr.TotalFleetNR_CarSales else null end) 
WHEN @fleet_name = 'RAC OPS'		 THEN SUM(case when Dr.OperStat = 'BD' then Dr.TotalFleetNR_RacOps else null end) 
WHEN @fleet_name = 'RAC TTL'		 THEN SUM(case when Dr.OperStat = 'BD' then Dr.TotalFleetNR_RacTtl else null end) 
WHEN @fleet_name = 'ADVANTAGE'		 THEN SUM(case when Dr.OperStat = 'BD' then Dr.TotalFleetNR_Adv else null end) 
WHEN @fleet_name = 'HERTZ ON DEMAND' THEN SUM(case when Dr.OperStat = 'BD' then Dr.TotalFleetNR_Hod else null end) 
WHEN @fleet_name = 'LICENSEE'		 THEN SUM(case when Dr.OperStat = 'BD' then Dr.TotalFleetNR_Licensee else null end) 
		ELSE
			SUM(case when Dr.OperStat = 'BD' then Dr.TotalFleetNR_RacTtl + Dr.TotalFleetNR_CarSales else null end) 
		END , 0 ) AS [BD],
		
			ISNULL(
		CASE	
WHEN @fleet_name = 'CARSALES'		 THEN SUM(case when Dr.OperStat = 'MM' then Dr.TotalFleetNR_CarSales else null end) 
WHEN @fleet_name = 'RAC OPS'		 THEN SUM(case when Dr.OperStat = 'MM' then Dr.TotalFleetNR_RacOps else null end) 
WHEN @fleet_name = 'RAC TTL'		 THEN SUM(case when Dr.OperStat = 'MM' then Dr.TotalFleetNR_RacTtl else null end) 
WHEN @fleet_name = 'ADVANTAGE'		 THEN SUM(case when Dr.OperStat = 'MM' then Dr.TotalFleetNR_Adv else null end) 
WHEN @fleet_name = 'HERTZ ON DEMAND' THEN SUM(case when Dr.OperStat = 'MM' then Dr.TotalFleetNR_Hod else null end) 
WHEN @fleet_name = 'LICENSEE'		 THEN SUM(case when Dr.OperStat = 'MM' then Dr.TotalFleetNR_Licensee else null end) 
		ELSE
			SUM(case when Dr.OperStat = 'MM' then Dr.TotalFleetNR_RacTtl + Dr.TotalFleetNR_CarSales else null end) 
		END , 0 ) AS [MM],
		
			ISNULL(
		CASE	
WHEN @fleet_name = 'CARSALES'		 THEN SUM(case when Dr.OperStat = 'TW' then Dr.TotalFleetNR_CarSales else null end) 
WHEN @fleet_name = 'RAC OPS'		 THEN SUM(case when Dr.OperStat = 'TW' then Dr.TotalFleetNR_RacOps else null end) 
WHEN @fleet_name = 'RAC TTL'		 THEN SUM(case when Dr.OperStat = 'TW' then Dr.TotalFleetNR_RacTtl else null end) 
WHEN @fleet_name = 'ADVANTAGE'		 THEN SUM(case when Dr.OperStat = 'TW' then Dr.TotalFleetNR_Adv else null end) 
WHEN @fleet_name = 'HERTZ ON DEMAND' THEN SUM(case when Dr.OperStat = 'TW' then Dr.TotalFleetNR_Hod else null end) 
WHEN @fleet_name = 'LICENSEE'		 THEN SUM(case when Dr.OperStat = 'TW' then Dr.TotalFleetNR_Licensee else null end) 
		ELSE
			SUM(case when Dr.OperStat = 'TW' then Dr.TotalFleetNR_RacTtl + Dr.TotalFleetNR_CarSales else null end) 
		END , 0 ) AS [TW],
		
			ISNULL(
		CASE	
WHEN @fleet_name = 'CARSALES'		 THEN SUM(case when Dr.OperStat = 'TB' then Dr.TotalFleetNR_CarSales else null end) 
WHEN @fleet_name = 'RAC OPS'		 THEN SUM(case when Dr.OperStat = 'TB' then Dr.TotalFleetNR_RacOps else null end) 
WHEN @fleet_name = 'RAC TTL'		 THEN SUM(case when Dr.OperStat = 'TB' then Dr.TotalFleetNR_RacTtl else null end) 
WHEN @fleet_name = 'ADVANTAGE'		 THEN SUM(case when Dr.OperStat = 'TB' then Dr.TotalFleetNR_Adv else null end) 
WHEN @fleet_name = 'HERTZ ON DEMAND' THEN SUM(case when Dr.OperStat = 'TB' then Dr.TotalFleetNR_Hod else null end) 
WHEN @fleet_name = 'LICENSEE'		 THEN SUM(case when Dr.OperStat = 'TB' then Dr.TotalFleetNR_Licensee else null end) 
		ELSE
			SUM(case when Dr.OperStat = 'TB' then Dr.TotalFleetNR_RacTtl + Dr.TotalFleetNR_CarSales else null end) 
		END , 0 ) AS [TB],
		
			ISNULL(
		CASE	
WHEN @fleet_name = 'CARSALES'		 THEN SUM(case when Dr.OperStat = 'FS' then Dr.TotalFleetNR_CarSales else null end) 
WHEN @fleet_name = 'RAC OPS'		 THEN SUM(case when Dr.OperStat = 'FS' then Dr.TotalFleetNR_RacOps else null end) 
WHEN @fleet_name = 'RAC TTL'		 THEN SUM(case when Dr.OperStat = 'FS' then Dr.TotalFleetNR_RacTtl else null end) 
WHEN @fleet_name = 'ADVANTAGE'		 THEN SUM(case when Dr.OperStat = 'FS' then Dr.TotalFleetNR_Adv else null end) 
WHEN @fleet_name = 'HERTZ ON DEMAND' THEN SUM(case when Dr.OperStat = 'FS' then Dr.TotalFleetNR_Hod else null end) 
WHEN @fleet_name = 'LICENSEE'		 THEN SUM(case when Dr.OperStat = 'FS' then Dr.TotalFleetNR_Licensee else null end) 
		ELSE
			SUM(case when Dr.OperStat = 'FS' then Dr.TotalFleetNR_RacTtl + Dr.TotalFleetNR_CarSales else null end) 
		END , 0 ) AS [FS],
		
			ISNULL(
		CASE	
WHEN @fleet_name = 'CARSALES'		 THEN SUM(case when Dr.OperStat = 'RL' then Dr.TotalFleetNR_CarSales else null end) 
WHEN @fleet_name = 'RAC OPS'		 THEN SUM(case when Dr.OperStat = 'RL' then Dr.TotalFleetNR_RacOps else null end) 
WHEN @fleet_name = 'RAC TTL'		 THEN SUM(case when Dr.OperStat = 'RL' then Dr.TotalFleetNR_RacTtl else null end) 
WHEN @fleet_name = 'ADVANTAGE'		 THEN SUM(case when Dr.OperStat = 'RL' then Dr.TotalFleetNR_Adv else null end) 
WHEN @fleet_name = 'HERTZ ON DEMAND' THEN SUM(case when Dr.OperStat = 'RL' then Dr.TotalFleetNR_Hod else null end) 
WHEN @fleet_name = 'LICENSEE'		 THEN SUM(case when Dr.OperStat = 'RL' then Dr.TotalFleetNR_Licensee else null end) 
		ELSE
			SUM(case when Dr.OperStat = 'RL' then Dr.TotalFleetNR_RacTtl + Dr.TotalFleetNR_CarSales else null end) 
		END , 0 ) AS [RL],
		
			ISNULL(
		CASE	
WHEN @fleet_name = 'CARSALES'		 THEN SUM(case when Dr.OperStat = 'RP' then Dr.TotalFleetNR_CarSales else null end) 
WHEN @fleet_name = 'RAC OPS'		 THEN SUM(case when Dr.OperStat = 'RP' then Dr.TotalFleetNR_RacOps else null end) 
WHEN @fleet_name = 'RAC TTL'		 THEN SUM(case when Dr.OperStat = 'RP' then Dr.TotalFleetNR_RacTtl else null end) 
WHEN @fleet_name = 'ADVANTAGE'		 THEN SUM(case when Dr.OperStat = 'RP' then Dr.TotalFleetNR_Adv else null end) 
WHEN @fleet_name = 'HERTZ ON DEMAND' THEN SUM(case when Dr.OperStat = 'RP' then Dr.TotalFleetNR_Hod else null end) 
WHEN @fleet_name = 'LICENSEE'		 THEN SUM(case when Dr.OperStat = 'RP' then Dr.TotalFleetNR_Licensee else null end) 
		ELSE
			SUM(case when Dr.OperStat = 'RP' then Dr.TotalFleetNR_RacTtl + Dr.TotalFleetNR_CarSales else null end) 
		END , 0 ) AS [RP],
		
			ISNULL(
		CASE	
WHEN @fleet_name = 'CARSALES'		 THEN SUM(case when Dr.OperStat = 'TN' then Dr.TotalFleetNR_CarSales else null end) 
WHEN @fleet_name = 'RAC OPS'		 THEN SUM(case when Dr.OperStat = 'TN' then Dr.TotalFleetNR_RacOps else null end) 
WHEN @fleet_name = 'RAC TTL'		 THEN SUM(case when Dr.OperStat = 'TN' then Dr.TotalFleetNR_RacTtl else null end) 
WHEN @fleet_name = 'ADVANTAGE'		 THEN SUM(case when Dr.OperStat = 'TN' then Dr.TotalFleetNR_Adv else null end) 
WHEN @fleet_name = 'HERTZ ON DEMAND' THEN SUM(case when Dr.OperStat = 'TN' then Dr.TotalFleetNR_Hod else null end) 
WHEN @fleet_name = 'LICENSEE'		 THEN SUM(case when Dr.OperStat = 'TN' then Dr.TotalFleetNR_Licensee else null end) 
		ELSE
			SUM(case when Dr.OperStat = 'TN' then Dr.TotalFleetNR_RacTtl + Dr.TotalFleetNR_CarSales else null end) 
		END , 0 ) AS [TN],
		
			ISNULL(	
		CASE	
WHEN @fleet_name = 'CARSALES'		 THEN SUM(case when Dr.OperStat = 'RT' then Dr.TotalFleetNR_CarSales else null end) 
WHEN @fleet_name = 'RAC OPS'		 THEN SUM(case when Dr.OperStat = 'RT' then Dr.TotalFleetNR_RacOps else null end) 
WHEN @fleet_name = 'RAC TTL'		 THEN SUM(case when Dr.OperStat = 'RT' then Dr.TotalFleetNR_RacTtl else null end) 
WHEN @fleet_name = 'ADVANTAGE'		 THEN SUM(case when Dr.OperStat = 'RT' then Dr.TotalFleetNR_Adv else null end) 
WHEN @fleet_name = 'HERTZ ON DEMAND' THEN SUM(case when Dr.OperStat = 'RT' then Dr.TotalFleetNR_Hod else null end) 
WHEN @fleet_name = 'LICENSEE'		 THEN SUM(case when Dr.OperStat = 'RT' then Dr.TotalFleetNR_Licensee else null end) 
		ELSE
			SUM(case when Dr.OperStat = 'RT' then Dr.TotalFleetNR_RacTtl + Dr.TotalFleetNR_CarSales else null end) 
		END , 0 ) AS [RT],
		
			ISNULL(
		CASE	
WHEN @fleet_name = 'CARSALES'		 THEN SUM(case when Dr.OperStat = 'SU' then Dr.TotalFleetNR_CarSales else null end) 
WHEN @fleet_name = 'RAC OPS'		 THEN SUM(case when Dr.OperStat = 'SU' then Dr.TotalFleetNR_RacOps else null end) 
WHEN @fleet_name = 'RAC TTL'		 THEN SUM(case when Dr.OperStat = 'SU' then Dr.TotalFleetNR_RacTtl else null end) 
WHEN @fleet_name = 'ADVANTAGE'		 THEN SUM(case when Dr.OperStat = 'SU' then Dr.TotalFleetNR_Adv else null end) 
WHEN @fleet_name = 'HERTZ ON DEMAND' THEN SUM(case when Dr.OperStat = 'SU' then Dr.TotalFleetNR_Hod else null end) 
WHEN @fleet_name = 'LICENSEE'		 THEN SUM(case when Dr.OperStat = 'SU' then Dr.TotalFleetNR_Licensee else null end) 
		ELSE
			SUM(case when Dr.OperStat = 'SU' then Dr.TotalFleetNR_RacTtl + Dr.TotalFleetNR_CarSales else null end) 
		END , 0 ) AS [SU],
		
			ISNULL(
		CASE	
WHEN @fleet_name = 'CARSALES'		 THEN SUM(case when Dr.OperStat = 'UNK' then Dr.TotalFleetNR_CarSales else null end) 
WHEN @fleet_name = 'RAC OPS'		 THEN SUM(case when Dr.OperStat = 'UNK' then Dr.TotalFleetNR_RacOps else null end) 
WHEN @fleet_name = 'RAC TTL'		 THEN SUM(case when Dr.OperStat = 'UNK' then Dr.TotalFleetNR_RacTtl else null end) 
WHEN @fleet_name = 'ADVANTAGE'		 THEN SUM(case when Dr.OperStat = 'UNK' then Dr.TotalFleetNR_Adv else null end) 
WHEN @fleet_name = 'HERTZ ON DEMAND' THEN SUM(case when Dr.OperStat = 'UNK' then Dr.TotalFleetNR_Hod else null end) 
WHEN @fleet_name = 'LICENSEE'		 THEN SUM(case when Dr.OperStat = 'UNK' then Dr.TotalFleetNR_Licensee else null end) 
		ELSE
			SUM(case when Dr.OperStat = 'UNK' then Dr.TotalFleetNR_RacTtl + Dr.TotalFleetNR_CarSales else null end) 
		END , 0 ) AS [UNK]
		

	FROM 
		[General].[Fact_NonRevLog_DailyReport] Dr
	WHERE 
	
		Dr.dim_calendar_id IN (SELECT dim_calendar_id FROM @calendar_ids)
		
		AND ((@day_of_week is null) or (Dr.Rep_DayOfWeek = @day_of_week))
		
	AND
		((@country				 IS NULL) OR (Dr.CountryCar		= @country))
	AND
		((@cms_pool_id			 IS NULL) OR (Dr.Cms_Pool_Id	= @cms_pool_id))
	AND 
		((@cms_location_group_id IS NULL) OR (Dr.Cms_Group_Id	= @cms_location_group_id))
	AND 
		((@ops_region_id		 IS NULL) OR (Dr.Ops_Region_Id	= @ops_region_id))
	AND 
		((@ops_area_id			 IS NULL) OR (Dr.Ops_Area_Id	= @ops_area_id))
	AND 
		((@location				 IS NULL) OR (Dr.LocationCode	= @location))
	AND
		((@car_segment_id		 IS NULL) OR (Dr.Car_Segment_Id	= @car_segment_id))
	AND 
		((@car_class_id			 IS NULL) OR (Dr.Car_Class_Id	= @car_class_id))
	AND 
		((@car_group_id			 IS NULL) OR (Dr.CarGroupId		= @car_group_id))
	AND 
		((@daygroupcode			 IS NULL) OR (Dr.DayGroupCode	IN 
											(SELECT Items FROM dbo.fSplit(@daygroupcode,','))))
	AND 
		((@operstat_name IS NULL) OR (Dr.OperStat IN
												(SELECT Items FROM dbo.fSplit(@operstat_name,','))))
	GROUP BY 
		Dr.Rep_Year , Dr.Rep_Month , Dr.Rep_WeekOfYear , Dr.Rep_Date
	ORDER BY 
		Dr.Rep_Date DESC
	
	INSERT INTO @PAGING (rowId) SELECT rowId FROM @RESULT
	ORDER BY
	CASE WHEN @sortExpression = 'Rep_Date'				THEN Rep_Date END ASC,
	CASE WHEN @sortExpression = 'Rep_Date DESC'			THEN Rep_Date END DESC,
	CASE WHEN @sortExpression = 'Total_Fleet_NR'		THEN Total_Fleet_NR END ASC,
	CASE WHEN @sortExpression = 'Total_Fleet_NR DESC'	THEN Total_Fleet_NR END DESC,
	CASE WHEN @sortExpression = 'CU'					THEN CU END ASC,
	CASE WHEN @sortExpression = 'CU DESC'				THEN CU END DESC,
	CASE WHEN @sortExpression = 'HA'					THEN HA END ASC,
	CASE WHEN @sortExpression = 'HA DESC'				THEN HA END DESC,
	CASE WHEN @sortExpression = 'HL'					THEN HL END ASC,
	CASE WHEN @sortExpression = 'HL DESC'				THEN HL END DESC,	
	CASE WHEN @sortExpression = 'LL'					THEN LL END ASC,
	CASE WHEN @sortExpression = 'LL DESC'				THEN LL END DESC,
	CASE WHEN @sortExpression = 'NC'					THEN NC END ASC,
	CASE WHEN @sortExpression = 'NC DESC'				THEN NC END DESC,
	CASE WHEN @sortExpression = 'PL'					THEN PL END ASC,
	CASE WHEN @sortExpression = 'PL DESC'				THEN PL END DESC,
	CASE WHEN @sortExpression = 'TC'					THEN TC END ASC,
	CASE WHEN @sortExpression = 'TC DESC'				THEN TC END DESC,	
	CASE WHEN @sortExpression = 'SV'					THEN SV END ASC,
	CASE WHEN @sortExpression = 'SV DESC'				THEN SV END DESC,
	CASE WHEN @sortExpression = 'WS'					THEN WS END ASC,
	CASE WHEN @sortExpression = 'WS DESC'				THEN WS END DESC,
	CASE WHEN @sortExpression = 'BD'					THEN BD END ASC,
	CASE WHEN @sortExpression = 'BD DESC'				THEN BD END DESC,
	CASE WHEN @sortExpression = 'MM'					THEN MM END ASC,
	CASE WHEN @sortExpression = 'MM DESC'				THEN MM END DESC,	
	CASE WHEN @sortExpression = 'TW'					THEN TW END ASC,
	CASE WHEN @sortExpression = 'TW DESC'				THEN TW END DESC,
	CASE WHEN @sortExpression = 'TB'					THEN TB END ASC,
	CASE WHEN @sortExpression = 'TB DESC'				THEN TB END DESC,
	CASE WHEN @sortExpression = 'FS'					THEN FS END ASC,
	CASE WHEN @sortExpression = 'FS DESC'				THEN FS END DESC,
	CASE WHEN @sortExpression = 'RL'					THEN RL END ASC,
	CASE WHEN @sortExpression = 'RL DESC'				THEN RL END DESC,
	CASE WHEN @sortExpression = 'RP'					THEN RP END ASC,
	CASE WHEN @sortExpression = 'RP DESC'				THEN RP END DESC,	
	CASE WHEN @sortExpression = 'TN'					THEN TN END ASC,
	CASE WHEN @sortExpression = 'TN DESC'				THEN TN END DESC,
	CASE WHEN @sortExpression = 'RT'					THEN RT END ASC,
	CASE WHEN @sortExpression = 'RT DESC'				THEN RT END DESC,
	CASE WHEN @sortExpression = 'SU'					THEN SU END ASC,
	CASE WHEN @sortExpression = 'SU DESC'				THEN SU END DESC,
	CASE WHEN @sortExpression = 'UNK'					THEN UNK END ASC,
	CASE WHEN @sortExpression = 'UNK DESC'				THEN UNK END DESC
	
	SELECT 
		CS.Rep_Date , CS.Total_Fleet_NR , 
		CS.CU , CS.HA , CS.HL , CS.LL , CS.NC , CS.PL , CS.TC , CS.SV , CS.WS , 
		CS.BD , CS.MM , CS.TW , CS.TB , CS.FS , CS.RL , CS.RP , CS.TN , CS.RT , 
		CS.SU , CS.UNK
	FROM 
		@RESULT CS
	INNER JOIN 
		@PAGING p ON p.rowId = cs.rowId 
	WHERE 
		(p.pageIndex BETWEEN @startRowIndex AND @maximumRows)
	ORDER BY 
		p.pageIndex;
		
	--Select total row count
	SELECT COUNT(*) AS totalCount FROM @RESULT;
	
    
END