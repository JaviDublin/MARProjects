-- =============================================
-- Author:		Javier
-- Create date: October 2012
-- Description:	Mars - Non Rev - Reports - Ageing Gridview
-- =============================================
CREATE PROCEDURE [General].[Select_NonRev_Grid_Start]
	
	@groupby				VARCHAR(20)		= NULL,
	@country				VARCHAR(10)		= NULL,
	@cms_pool_id			INT				= NULL,
	@cms_location_group_id	INT				= NULL, 
	@ops_region_id			INT				= NULL,
	@ops_area_id			INT				= NULL,	
	@location				VARCHAR(50)		= NULL,
	@fleet_name				VARCHAR(50)		= NULL,
	@car_segment_id			INT				= NULL,
	@car_class_id			INT				= NULL,
	@car_group_id			INT				= NULL,	
	@daygroupcode			VARCHAR(255)	= NULL,
	@sortExpression			VARCHAR(50)		= NULL,
	@startRowIndex			INT				= NULL,
	@maximumRows			INT				= NULL
	
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	
	DECLARE @calendar_id INT = (SELECT MAX(dim_calendar_id) FROM [General].[Fact_NonRevLog_Summary])
	
	DECLARE @fleetname VARCHAR(20)
	
	IF @fleet_name IS NULL 				BEGIN SET @fleetname = 'all' END
	IF @fleet_name = 'CARSALES'			BEGIN SET @fleetname = 'cs' END
	IF @fleet_name = 'RAC OPS'			BEGIN SET @fleetname = 'op' END
	IF @fleet_name = 'RAC TTL'			BEGIN SET @fleetname = 'tt' END
	IF @fleet_name = 'ADVANTAGE'		BEGIN SET @fleetname = 'ad' END
	IF @fleet_name = 'HERTZ ON DEMAND'	BEGIN SET @fleetname = 'hd' END
	IF @fleet_name = 'LICENSEE'			BEGIN SET @fleetname = 'lc' END
	
	
	DECLARE @AGEING TABLE
	(
		RowId INT IDENTITY , GroupCode_Stats VARCHAR(20) , GroupCode_Kci VARCHAR(20) , TotalFleet INT,
		TotalVehicles INT , PctNonRev NUMERIC(5,2), PctTotal NUMERIC(5,2),
		DayGroup1 INT , DayGroup2 INT , DayGroup3 INT , DayGroup4 INT , DayGroup5 INT , 
		DayGroup6 INT , DayGroup7 INT , DayGroup8 INT , DayGroup9 INT
	)
	
	INSERT INTO @AGEING
	(
		GroupCode_Stats , GroupCode_Kci , TotalFleet , TotalVehicles , 
		DayGroup1 , DayGroup2 , DayGroup3 , DayGroup4 , DayGroup5 , 
		DayGroup6 , DayGroup7 , DayGroup8 , DayGroup9
	)
	
	SELECT 
		nrls.OperationalStatusCode	AS [GroupCode_Stats], 
		os.KCICode					AS [GroupCode_Kci], 
	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.total_fleet_tt) + SUM(nrls.total_fleet_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.total_fleet_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.total_fleet_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.total_fleet_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.total_fleet_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.total_fleet_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.total_fleet_op)
	END AS [TotalFleet], 
	
	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.non_rev_fleet_tt) + SUM(nrls.non_rev_fleet_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.non_rev_fleet_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.non_rev_fleet_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.non_rev_fleet_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.non_rev_fleet_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.non_rev_fleet_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.non_rev_fleet_op)
	END AS [TotalVehicles],

	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.daygroup1_tt) + SUM(nrls.daygroup1_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.daygroup1_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.daygroup1_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.daygroup1_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.daygroup1_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.daygroup1_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.daygroup1_op)
	END		
	AS [Daygroup1],
	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.daygroup2_tt) + SUM(nrls.daygroup2_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.daygroup2_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.daygroup2_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.daygroup2_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.daygroup2_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.daygroup2_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.daygroup2_op)
	END AS [Daygroup2],
			
	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.daygroup3_tt) + SUM(nrls.daygroup3_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.daygroup3_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.daygroup3_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.daygroup3_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.daygroup3_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.daygroup3_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.daygroup3_op)
	END
	AS [Daygroup3],
	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.daygroup4_tt) + SUM(nrls.daygroup4_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.daygroup4_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.daygroup4_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.daygroup4_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.daygroup4_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.daygroup4_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.daygroup4_op)
	END
	AS [Daygroup4],
	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.daygroup5_tt) + SUM(nrls.daygroup5_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.daygroup5_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.daygroup5_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.daygroup5_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.daygroup5_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.daygroup5_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.daygroup5_op)
	END
	AS [Daygroup5],
	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.daygroup6_tt) + SUM(nrls.daygroup6_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.daygroup6_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.daygroup6_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.daygroup6_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.daygroup6_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.daygroup6_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.daygroup6_op)
	END
	AS [Daygroup6],
	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.daygroup7_tt) + SUM(nrls.daygroup7_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.daygroup7_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.daygroup7_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.daygroup7_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.daygroup7_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.daygroup7_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.daygroup7_op)
	END
	AS [Daygroup7],
	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.daygroup8_tt) + SUM(nrls.daygroup8_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.daygroup8_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.daygroup8_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.daygroup8_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.daygroup8_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.daygroup8_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.daygroup8_op)
	END
	AS [Daygroup8],
	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.daygroup9_tt) + SUM(nrls.daygroup9_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.daygroup9_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.daygroup9_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.daygroup9_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.daygroup9_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.daygroup9_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.daygroup9_op)
	END
	AS [Daygroup9]
		
	FROM 
		[General].[Fact_NonRevLog_Summary] nrls
	INNER JOIN [Settings].Operational_Status os ON nrls.OperationalStatusCode = os.OperationalStatusCode
	WHERE 
		nrls.dim_calendar_id = @calendar_id
	AND ((@country is null) or (nrls.country_car = @country))
	AND ((@cms_pool_id is null) or (nrls.location_pool_id = @cms_pool_id))
	AND ((@cms_location_group_id is null) or (nrls.location_group_id = @cms_location_group_id))
	AND ((@ops_region_id is null) or (nrls.location_ops_region_id = @ops_region_id))
	AND ((@ops_area_id is null) or (nrls.location_ops_area_id = @ops_area_id))
	AND ((@location is null) or (nrls.location = @location))
	AND ((@car_segment_id is null) or (nrls.car_segment_id = @car_segment_id))
	AND ((@car_class_id is null) or (nrls.car_class_id = @car_class_id))
	AND ((@car_group_id is null) or (nrls.car_group_id = @car_group_id))
	GROUP BY 
		nrls.OperationalStatusCode , os.KCICode
	
	DECLARE @totaFleet INT , @totalFleetNonRev INT
	
	SET @totaFleet			= (SELECT SUM(TotalFleet) FROM @AGEING)
	
	SET @totalFleetNonRev	= (SELECT SUM(TotalVehicles) FROM @AGEING)
	
	UPDATE @AGEING SET PctTotal = CONVERT(NUMERIC(5,2), TotalVehicles * 100 / @totaFleet)
	
	DECLARE @TABLE_RESULTS TABLE
	(
		rowId		INT IDENTITY (1,1) NOT NULL,
		GroupCode	VARCHAR(20) , TotalVehicles VARCHAR(20) , TotalFleet VARCHAR(20),
		PctNonRev   VARCHAR(20)	, PctTotal	VARCHAR(20)	, 
		DayGroup1	VARCHAR(20)	, DayGroup2 VARCHAR(20) , DayGroup3	VARCHAR(20)	, 
		DayGroup4	VARCHAR(20) , DayGroup5 VARCHAR(20) , DayGroup6	VARCHAR(20)	, 
		DayGroup7	VARCHAR(20) , DayGroup8	VARCHAR(20)	, DayGroup9 VARCHAR(20)
	)
	
	DECLARE @PAGING TABLE
	(
		pageIndex		INT IDENTITY (1,1) NOT NULL,
		rowId			INT NULL
	)
	
	IF @groupby IS NULL or @groupby = 'KCI'
	BEGIN
		INSERT INTO @TABLE_RESULTS
		(
			GroupCode , TotalFleet , TotalVehicles , PctNonRev , PctTotal , DayGroup1 , DayGroup2 , DayGroup3 ,
			DayGroup4 , DayGroup5 , DayGroup6 , DayGroup7 , DayGroup8 , DayGroup9
		)
		
		SELECT 
			AG.GroupCode_Kci							AS [GroupCode], 
			dbo.fnAddCommaToInt(@totaFleet)				AS [TotalFleet] , 
			dbo.fnAddCommaToInt(sum(AG.TotalVehicles))	AS [TotalVehicles] , 
			NULL , NULL ,
			dbo.fnAddCommaToInt(sum(AG.DayGroup1))		AS [DayGroup1], 
			dbo.fnAddCommaToInt(sum(AG.DayGroup2))		AS [DayGroup2], 
			dbo.fnAddCommaToInt(sum(AG.DayGroup3))		AS [DayGroup3], 
			dbo.fnAddCommaToInt(sum(AG.DayGroup4))		AS [DayGroup4], 
			dbo.fnAddCommaToInt(sum(AG.DayGroup5))		AS [DayGroup5], 
			dbo.fnAddCommaToInt(sum(AG.DayGroup6))		AS [DayGroup6], 
			dbo.fnAddCommaToInt(sum(AG.DayGroup7))		AS [DayGroup7], 
			dbo.fnAddCommaToInt(sum(AG.DayGroup8))		AS [DayGroup8], 
			dbo.fnAddCommaToInt(sum(AG.DayGroup9))		AS [DayGroup9]
		FROM @AGEING AG
		GROUP BY 
			AG.GroupCode_Kci
		ORDER BY 
				CONVERT(VARCHAR,avg(AG.PctNonRev)) desc
				
				
		
		UPDATE @TABLE_RESULTS SET
		PctTotal  = CONVERT(VARCHAR,CONVERT(NUMERIC(5,2),
		CAST( convert(float,replace(TotalVehicles,',','')) AS FLOAT) * 100.00 / 
			CAST(@totaFleet AS FLOAT))) + ' %' , 
		PctNonRev = CONVERT(VARCHAR,CONVERT(NUMERIC(5,2),
		CAST( convert(float,replace(TotalVehicles,',','')) AS FLOAT) * 100.00 / 
			CAST(@totalFleetNonRev AS FLOAT))) + ' %'
			
	END
	ELSE IF @groupby = 'STAT'
	BEGIN
		INSERT INTO @TABLE_RESULTS
		(
			GroupCode , TotalFleet , TotalVehicles , PctNonRev , PctTotal , DayGroup1 , DayGroup2 , DayGroup3 ,
			DayGroup4 , DayGroup5 , DayGroup6 , DayGroup7 , DayGroup8 , DayGroup9
		)
		
		SELECT 
			AG.GroupCode_Stats						AS [GroupCode], 
			dbo.fnAddCommaToInt(@totaFleet)			AS [TotalFleet] ,
			dbo.fnAddCommaToInt(AG.TotalVehicles)	AS [TotalVehicles] , 
					
			CONVERT(VARCHAR,CONVERT(NUMERIC(5,2),CAST(AG.TotalVehicles AS FLOAT) * 100.00 / 
			CAST(@totaFleet AS FLOAT))) + ' %' 
			
			AS [PctNonRev],
			
			CONVERT(VARCHAR,CONVERT(NUMERIC(5,2),CAST(AG.TotalVehicles AS FLOAT) * 100.00 / 
			CAST(@totalFleetNonRev AS FLOAT))) + ' %'
			
			AS [PctTotal],

			
			dbo.fnAddCommaToInt(AG.DayGroup1)		AS [DayGroup1], 
			dbo.fnAddCommaToInt(AG.DayGroup2)		AS [DayGroup2], 
			dbo.fnAddCommaToInt(AG.DayGroup3)		AS [DayGroup3], 
			dbo.fnAddCommaToInt(AG.DayGroup4)		AS [DayGroup4], 
			dbo.fnAddCommaToInt(AG.DayGroup5)		AS [DayGroup5], 
			dbo.fnAddCommaToInt(AG.DayGroup6)		AS [DayGroup6], 
			dbo.fnAddCommaToInt(AG.DayGroup7)		AS [DayGroup7], 
			dbo.fnAddCommaToInt(AG.DayGroup8)		AS [DayGroup8], 
			dbo.fnAddCommaToInt(AG.DayGroup9)		AS [DayGroup9]
		FROM @AGEING AG
		ORDER BY 
			CONVERT(VARCHAR,AG.PctNonRev) desc , AG.GroupCode_Stats
			
	END
			
	INSERT INTO @PAGING (rowId) SELECT rowId FROM @TABLE_RESULTS
	ORDER BY
	CASE WHEN @sortExpression = 'GroupCode'				THEN GroupCode END ASC,
	CASE WHEN @sortExpression = 'GroupCode DESC'		THEN GroupCode END DESC,
	CASE WHEN @sortExpression = 'TotalVehicles'			THEN CONVERT(INT,REPLACE(TotalVehicles,',',''))  END ASC,
	CASE WHEN @sortExpression = 'TotalVehicles DESC'	THEN CONVERT(INT,REPLACE(TotalVehicles,',',''))  END DESC,
	CASE WHEN @sortExpression = 'PctNonRev'				THEN CONVERT(FLOAT,REPLACE(PctNonRev,'%','')) END ASC,
	CASE WHEN @sortExpression = 'PctNonRev DESC'		THEN CONVERT(FLOAT,REPLACE(PctNonRev,'%','')) END DESC,
	CASE WHEN @sortExpression = 'PctTotal'				THEN CONVERT(FLOAT,REPLACE(PctTotal,'%','')) END ASC,
	CASE WHEN @sortExpression = 'PctTotal DESC'			THEN CONVERT(FLOAT,REPLACE(PctTotal,'%','')) END DESC,	
	CASE WHEN @sortExpression = 'DayGroup1'				THEN CONVERT(INT,REPLACE(DayGroup1,',',''))  END ASC,
	CASE WHEN @sortExpression = 'DayGroup1 DESC'		THEN CONVERT(INT,REPLACE(DayGroup1,',',''))  END DESC,
	CASE WHEN @sortExpression = 'DayGroup2'				THEN CONVERT(INT,REPLACE(DayGroup2,',',''))  END ASC,
	CASE WHEN @sortExpression = 'DayGroup2 DESC'		THEN CONVERT(INT,REPLACE(DayGroup2,',',''))  END DESC,
	CASE WHEN @sortExpression = 'DayGroup3'				THEN CONVERT(INT,REPLACE(DayGroup3,',',''))  END ASC,
	CASE WHEN @sortExpression = 'DayGroup3 DESC'		THEN CONVERT(INT,REPLACE(DayGroup3,',',''))  END DESC,
	CASE WHEN @sortExpression = 'DayGroup4'				THEN CONVERT(INT,REPLACE(DayGroup4,',',''))  END ASC,
	CASE WHEN @sortExpression = 'DayGroup4 DESC'		THEN CONVERT(INT,REPLACE(DayGroup4,',',''))  END DESC,
	CASE WHEN @sortExpression = 'DayGroup5'				THEN CONVERT(INT,REPLACE(DayGroup5,',',''))  END ASC,
	CASE WHEN @sortExpression = 'DayGroup5 DESC'		THEN CONVERT(INT,REPLACE(DayGroup5,',',''))  END DESC,
	CASE WHEN @sortExpression = 'DayGroup6'				THEN CONVERT(INT,REPLACE(DayGroup6,',',''))  END ASC,
	CASE WHEN @sortExpression = 'DayGroup6 DESC'		THEN CONVERT(INT,REPLACE(DayGroup6,',',''))  END DESC,
	CASE WHEN @sortExpression = 'DayGroup7'				THEN CONVERT(INT,REPLACE(DayGroup7,',',''))  END ASC,
	CASE WHEN @sortExpression = 'DayGroup7 DESC'		THEN CONVERT(INT,REPLACE(DayGroup7,',',''))  END DESC,
	CASE WHEN @sortExpression = 'DayGroup8'				THEN CONVERT(INT,REPLACE(DayGroup8,',',''))  END ASC,
	CASE WHEN @sortExpression = 'DayGroup8 DESC'		THEN CONVERT(INT,REPLACE(DayGroup8,',',''))  END DESC,
	CASE WHEN @sortExpression = 'DayGroup9'				THEN CONVERT(INT,REPLACE(DayGroup9,',',''))  END ASC,
	CASE WHEN @sortExpression = 'DayGroup9 DESC'		THEN CONVERT(INT,REPLACE(DayGroup9,',',''))  END DESC
	
	,CASE WHEN @sortExpression IS NULL THEN 
		CONVERT(FLOAT,REPLACE(PctNonRev,'%','')) END DESC ,
		CONVERT(INT,REPLACE(TotalVehicles,',','')) DESC

	SELECT 
		CS.GroupCode , CS.TotalFleet , CS.TotalVehicles , CS.PctNonRev , CS.PctTotal ,
		CS.DayGroup1 , CS.DayGroup2 , CS.DayGroup3 ,
		CS.DayGroup4 , CS.DayGroup5 , CS.DayGroup6 ,
		CS.DayGroup7 , CS.DayGroup8 , CS.DayGroup9 
	FROM 
		@TABLE_RESULTS CS
	INNER JOIN 
		@PAGING p ON p.rowId = cs.rowId 
	WHERE 
		(p.pageIndex BETWEEN @startRowIndex AND @maximumRows)
	ORDER BY 
		p.pageIndex;
		
	--Select total row count
	SELECT COUNT(*) AS totalCount FROM @TABLE_RESULTS;
	
	
	 
END