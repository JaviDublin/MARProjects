-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Select_NonRev_Grid_Comparison]

		@reportType				VARCHAR(10)		= NULL,
		@logic					VARCHAR(5)		= NULL, 
		@customStartDate		VARCHAR(30)		= NULL,
		@customEndDate			VARCHAR(30)		= NULL,
		@dateComparison			VARCHAR(30)		= NULL,
		@dateRange				VARCHAR(50)		= NULL,
		@dateRangeValue		    VARCHAR(20)		= NULL,
		@day_of_week			INT				= NULL,		
		@country				VARCHAR(10)		= NULL,
		@cms_pool_id			INT				= NULL,
		@cms_location_group_id	INT				= NULL, 
		@ops_region_id			INT				= NULL,
		@ops_area_id			INT				= NULL,	
		@location				VARCHAR(50)		= NULL,
		@car_segment_id			INT				= NULL,
		@car_class_id			INT				= NULL,
		@car_group_id			INT				= NULL,
		@operstat_name			VARCHAR(300)	= NULL,
		@daygroupcode			VARCHAR(255)	= NULL,
		@totalFleetType			VARCHAR(50)		= NULL,
		@sortExpression			VARCHAR(50)		= NULL,
		@startRowIndex			INT				= NULL,
		@maximumRows			INT				= NULL,
		@fleet_name				VARCHAR(20)		= NULL
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	IF @totalFleetType IS NULL BEGIN SET @totalFleetType = 'TOTAL' END
	
	-- Region : Set the Dates
	--=========================================================================================
	
	DECLARE @calendar_ids TABLE (dim_calendar_id INT)
	
	DECLARE @start_calendar_id INT, @end_calendar_id INT
	 
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
	
	SET @start_calendar_id = (SELECT dim_Calendar_id FROM Inp.dim_Calendar WHERE Rep_Date = @theDateStart)
	
	SET @end_calendar_id   = (SELECT dim_Calendar_id FROM Inp.dim_Calendar WHERE Rep_Date = DATEADD(D,0,DATEDIFF(D,0,@theDateEnd)))
	

	INSERT INTO @calendar_ids (dim_calendar_id)
	SELECT 
		dim_Calendar_id 
	FROM 
		Inp.dim_Calendar 
	WHERE 
		dim_Calendar_id BETWEEN @start_calendar_id AND @end_calendar_id
				
	-- Region : Set the Fleet Name Values
	--=========================================================================================
	DECLARE @fleetname VARCHAR(20)
	
	IF @fleet_name IS NULL 				BEGIN SET @fleetname = 'all' END
	IF @fleet_name = 'CARSALES'			BEGIN SET @fleetname = 'cs' END
	IF @fleet_name = 'RAC OPS'			BEGIN SET @fleetname = 'op' END
	IF @fleet_name = 'RAC TTL'			BEGIN SET @fleetname = 'tt' END
	IF @fleet_name = 'ADVANTAGE'		BEGIN SET @fleetname = 'ad' END
	IF @fleet_name = 'HERTZ ON DEMAND'	BEGIN SET @fleetname = 'hd' END
	IF @fleet_name = 'LICENSEE'			BEGIN SET @fleetname = 'lc' END
	
	
	-- Region : Set the Group Levels
	--=========================================================================================
	
	IF @reportType	IS NULL BEGIN SET @reportType	= 'Site'	END
	IF @logic		IS NULL BEGIN SET @logic		= 'CMS'		END
	
	DECLARE  @grouplevel VARCHAR(50), @level_id INT
	
	DECLARE @levels TABLE (level_id INT , level_name VARCHAR(50))
	INSERT INTO @levels VALUES (1 , 'country')
	INSERT INTO @levels VALUES (2 , 'pool')
	INSERT INTO @levels VALUES (3 , 'group')
	INSERT INTO @levels VALUES (4 , 'area')
	INSERT INTO @levels VALUES (5 , 'region')
	INSERT INTO @levels VALUES (6 , 'location')
	INSERT INTO @levels VALUES (7 , 'cargroup')
	INSERT INTO @levels VALUES (8 , 'carsegment')
	INSERT INTO @levels VALUES (9 , 'carclass')

	IF @reportType = 'Site'
	BEGIN
		IF @country IS NULL 
		BEGIN 
			SET @level_id = 1 -- Country
		END 
		ELSE
		BEGIN
			IF @logic = 'CMS'
			BEGIN
				IF @cms_pool_id IS NULL   
				BEGIN 
					SET @level_id = 2 -- cms pool   
				END 
				ELSE
				BEGIN
					IF @cms_location_group_id IS NULL 
					BEGIN 
						SET @level_id = 3 -- location group
					END 
					ELSE
					BEGIN
						SET @level_id = 6 -- location
					END
				END
			END
			ELSE IF @logic = 'OPS'
			BEGIN
				IF @ops_region_id IS NULL 
				BEGIN 
					SET @level_id = 5 -- region 
				END 
				ELSE
				BEGIN
					IF @ops_area_id IS NULL 
					BEGIN 
						SET @level_id = 4 -- area
					END 
					ELSE
					BEGIN
						SET @level_id = 6 -- location
					END
				END
			END
		END
	END
	ELSE IF @reportType = 'Fleet'
	BEGIN
		IF @car_segment_id IS NULL 
		BEGIN 
			SET @level_id = 8 -- car segment 
		END 
		ELSE
		BEGIN
			IF @car_class_id IS NULL 
			BEGIN 
				SET @level_id = 9 -- car class 
			END 
			ELSE
			BEGIN
				SET @level_id = 7 -- car group
			END 
		END
	END
	
	SET @grouplevel = (SELECT level_name FROM @levels WHERE level_id = @level_id)
	
	-- Region : Table Report (Data)
	--=========================================================================================
	-- Settings
	------------------------------------
	
	DECLARE @rowcount INT = (SELECT COUNT(p.dim_calendar_id) FROM (
	SELECT dim_calendar_id from [General].Fact_NonRevLog_Summary where dim_calendar_id in 
	(select dim_calendar_id from @calendar_ids) group by dim_calendar_id) p)
	
	DECLARE @min_date INT = (SELECT MIN(dim_calendar_id) FROM @calendar_ids)
	
	DECLARE @max_date INT = (SELECT MAX(dim_calendar_id) FROM @calendar_ids)

	-- Table Report
	------------------------------------
	DECLARE @table_report TABLE 
	(
		position SMALLINT, grouplevel VARCHAR(100) , TotalFleet INT , TotalFleetNR INT,
		daygroup1 INT , daygroup2 INT , daygroup3 INT , daygroup4 INT , daygroup5 INT ,
		daygroup6 INT , daygroup7 INT , daygroup8 INT , daygroup9 INT, commentsfilled INT
	)

	INSERT INTO @table_report 
	(
		position  , grouplevel , TotalFleet , TotalFleetNR,
		daygroup1 , daygroup2  , daygroup3  , daygroup4   ,
		daygroup5 , daygroup6  , daygroup7  , daygroup8   , 
		daygroup9 , commentsfilled
	)

	SELECT 
		2								AS [Position],
		t.GroupLevel					AS [GroupLevel], 
		AVG(t.TotalFleet)/@rowcount		AS [TotalFleet], 
		
		AVG(t.TotalFleetNR)/@rowcount	
								AS [TotalFleetNR],
		AVG(t.Daygroup1)/@rowcount		AS [DayGroup1],
		AVG(t.Daygroup2)/@rowcount		AS [DayGroup2],
		AVG(t.Daygroup3)/@rowcount		AS [DayGroup3],
		AVG(t.Daygroup4)/@rowcount		AS [DayGroup4],
		AVG(t.Daygroup5)/@rowcount		AS [DayGroup5],
		AVG(t.Daygroup6)/@rowcount		AS [DayGroup6],
		AVG(t.Daygroup7)/@rowcount		AS [DayGroup7],
		AVG(t.Daygroup8)/@rowcount		AS [DayGroup8],
		AVG(t.Daygroup9)/@rowcount		AS [DayGroup9],
		AVG(t.CommentsFilled/@rowcount) AS [CommentsFilled]
	FROM 
(
		SELECT
			CASE 
				WHEN @grouplevel = 'country'	THEN fnrls.country_loc 
				WHEN @grouplevel = 'pool'		THEN fnrls.location_pool
				WHEN @grouplevel = 'group'		THEN fnrls.location_group
				WHEN @grouplevel = 'region'		THEN fnrls.location_ops_region
				WHEN @grouplevel = 'area'		THEN fnrls.location_ops_area
				WHEN @grouplevel = 'location'	THEN fnrls.location
				WHEN @grouplevel = 'cargroup'	THEN fnrls.car_group
				WHEN @grouplevel = 'carclass'	THEN fnrls.car_class
				WHEN @grouplevel = 'carsegment'	THEN fnrls.car_segment
			END 
			AS [GroupLevel],
			
			CASE WHEN @totalFleetType = 'TOTAL'
			THEN
				CASE
				WHEN @fleetname = 'all' THEN SUM(fnrls.total_fleet_tt) + SUM(fnrls.total_fleet_cs)
				WHEN @fleetname = 'cs'  THEN SUM(fnrls.total_fleet_cs)
				WHEN @fleetname = 'lc'  THEN SUM(fnrls.total_fleet_lc)
				WHEN @fleetname = 'ad'  THEN SUM(fnrls.total_fleet_ad)
				WHEN @fleetname = 'hd'  THEN SUM(fnrls.total_fleet_hd)
				WHEN @fleetname = 'tt'  THEN SUM(fnrls.total_fleet_tt)
				WHEN @fleetname = 'op'  THEN SUM(fnrls.total_fleet_op)
				END
			ELSE
				CASE
				WHEN @fleetname = 'all' THEN SUM(fnrls.operational_fleet_tt) + SUM(fnrls.operational_fleet_cs)
				WHEN @fleetname = 'cs'  THEN SUM(fnrls.operational_fleet_cs)
				WHEN @fleetname = 'lc'  THEN SUM(fnrls.operational_fleet_lc)
				WHEN @fleetname = 'ad'  THEN SUM(fnrls.operational_fleet_ad)
				WHEN @fleetname = 'hd'  THEN SUM(fnrls.operational_fleet_hd)
				WHEN @fleetname = 'tt'  THEN SUM(fnrls.operational_fleet_tt)
				WHEN @fleetname = 'op'  THEN SUM(fnrls.operational_fleet_op)
				END
			END AS [TotalFleet],
			
			CASE
				WHEN @fleetname = 'all' THEN SUM(fnrls.non_rev_fleet_tt) + SUM(fnrls.non_rev_fleet_cs)
				WHEN @fleetname = 'cs'  THEN SUM(fnrls.non_rev_fleet_cs)
				WHEN @fleetname = 'lc'  THEN SUM(fnrls.non_rev_fleet_lc)
				WHEN @fleetname = 'ad'  THEN SUM(fnrls.non_rev_fleet_ad)
				WHEN @fleetname = 'hd'  THEN SUM(fnrls.non_rev_fleet_hd)
				WHEN @fleetname = 'tt'  THEN SUM(fnrls.non_rev_fleet_tt)
				WHEN @fleetname = 'op'  THEN SUM(fnrls.non_rev_fleet_op)
			END
			AS [TotalFleetNR],
			CASE
				WHEN @fleetname = 'all' THEN SUM(fnrls.daygroup1_tt) + SUM(fnrls.daygroup1_cs)
				WHEN @fleetname = 'cs'  THEN SUM(fnrls.daygroup1_cs)
				WHEN @fleetname = 'lc'  THEN SUM(fnrls.daygroup1_lc)
				WHEN @fleetname = 'ad'  THEN SUM(fnrls.daygroup1_ad)
				WHEN @fleetname = 'hd'  THEN SUM(fnrls.daygroup1_hd)
				WHEN @fleetname = 'tt'  THEN SUM(fnrls.daygroup1_tt)
				WHEN @fleetname = 'op'  THEN SUM(fnrls.daygroup1_op)
			END
			AS [Daygroup1],
			CASE
				WHEN @fleetname = 'all' THEN SUM(fnrls.daygroup2_tt) + SUM(fnrls.daygroup2_cs)
				WHEN @fleetname = 'cs'  THEN SUM(fnrls.daygroup2_cs)
				WHEN @fleetname = 'lc'  THEN SUM(fnrls.daygroup2_lc)
				WHEN @fleetname = 'ad'  THEN SUM(fnrls.daygroup2_ad)
				WHEN @fleetname = 'hd'  THEN SUM(fnrls.daygroup2_hd)
				WHEN @fleetname = 'tt'  THEN SUM(fnrls.daygroup2_tt)
				WHEN @fleetname = 'op'  THEN SUM(fnrls.daygroup2_op)
			END
			AS [Daygroup2],
			
			CASE
				WHEN @fleetname = 'all' THEN SUM(fnrls.daygroup3_tt) + SUM(fnrls.daygroup3_cs)
				WHEN @fleetname = 'cs'  THEN SUM(fnrls.daygroup3_cs)
				WHEN @fleetname = 'lc'  THEN SUM(fnrls.daygroup3_lc)
				WHEN @fleetname = 'ad'  THEN SUM(fnrls.daygroup3_ad)
				WHEN @fleetname = 'hd'  THEN SUM(fnrls.daygroup3_hd)
				WHEN @fleetname = 'tt'  THEN SUM(fnrls.daygroup3_tt)
				WHEN @fleetname = 'op'  THEN SUM(fnrls.daygroup3_op)
			END
			AS [Daygroup3],
			CASE
				WHEN @fleetname = 'all' THEN SUM(fnrls.daygroup4_tt) + SUM(fnrls.daygroup4_cs)
				WHEN @fleetname = 'cs'  THEN SUM(fnrls.daygroup4_cs)
				WHEN @fleetname = 'lc'  THEN SUM(fnrls.daygroup4_lc)
				WHEN @fleetname = 'ad'  THEN SUM(fnrls.daygroup4_ad)
				WHEN @fleetname = 'hd'  THEN SUM(fnrls.daygroup4_hd)
				WHEN @fleetname = 'tt'  THEN SUM(fnrls.daygroup4_tt)
				WHEN @fleetname = 'op'  THEN SUM(fnrls.daygroup4_op)
			END
			AS [Daygroup4],
			CASE
				WHEN @fleetname = 'all' THEN SUM(fnrls.daygroup5_tt) + SUM(fnrls.daygroup5_cs)
				WHEN @fleetname = 'cs'  THEN SUM(fnrls.daygroup5_cs)
				WHEN @fleetname = 'lc'  THEN SUM(fnrls.daygroup5_lc)
				WHEN @fleetname = 'ad'  THEN SUM(fnrls.daygroup5_ad)
				WHEN @fleetname = 'hd'  THEN SUM(fnrls.daygroup5_hd)
				WHEN @fleetname = 'tt'  THEN SUM(fnrls.daygroup5_tt)
				WHEN @fleetname = 'op'  THEN SUM(fnrls.daygroup5_op)
			END
			AS [Daygroup5],
			CASE
				WHEN @fleetname = 'all' THEN SUM(fnrls.daygroup6_tt) + SUM(fnrls.daygroup6_cs)
				WHEN @fleetname = 'cs'  THEN SUM(fnrls.daygroup6_cs)
				WHEN @fleetname = 'lc'  THEN SUM(fnrls.daygroup6_lc)
				WHEN @fleetname = 'ad'  THEN SUM(fnrls.daygroup6_ad)
				WHEN @fleetname = 'hd'  THEN SUM(fnrls.daygroup6_hd)
				WHEN @fleetname = 'tt'  THEN SUM(fnrls.daygroup6_tt)
				WHEN @fleetname = 'op'  THEN SUM(fnrls.daygroup6_op)
			END
			AS [Daygroup6],
			CASE
				WHEN @fleetname = 'all' THEN SUM(fnrls.daygroup7_tt) + SUM(fnrls.daygroup7_cs)
				WHEN @fleetname = 'cs'  THEN SUM(fnrls.daygroup7_cs)
				WHEN @fleetname = 'lc'  THEN SUM(fnrls.daygroup7_lc)
				WHEN @fleetname = 'ad'  THEN SUM(fnrls.daygroup7_ad)
				WHEN @fleetname = 'hd'  THEN SUM(fnrls.daygroup7_hd)
				WHEN @fleetname = 'tt'  THEN SUM(fnrls.daygroup7_tt)
				WHEN @fleetname = 'op'  THEN SUM(fnrls.daygroup7_op)
			END
			AS [Daygroup7],
			CASE
				WHEN @fleetname = 'all' THEN SUM(fnrls.daygroup8_tt) + SUM(fnrls.daygroup8_cs)
				WHEN @fleetname = 'cs'  THEN SUM(fnrls.daygroup8_cs)
				WHEN @fleetname = 'lc'  THEN SUM(fnrls.daygroup8_lc)
				WHEN @fleetname = 'ad'  THEN SUM(fnrls.daygroup8_ad)
				WHEN @fleetname = 'hd'  THEN SUM(fnrls.daygroup8_hd)
				WHEN @fleetname = 'tt'  THEN SUM(fnrls.daygroup8_tt)
				WHEN @fleetname = 'op'  THEN SUM(fnrls.daygroup8_op)
			END
			AS [Daygroup8],
			CASE
				WHEN @fleetname = 'all' THEN SUM(fnrls.daygroup9_tt) + SUM(fnrls.daygroup9_cs)
				WHEN @fleetname = 'cs'  THEN SUM(fnrls.daygroup9_cs)
				WHEN @fleetname = 'lc'  THEN SUM(fnrls.daygroup9_lc)
				WHEN @fleetname = 'ad'  THEN SUM(fnrls.daygroup9_ad)
				WHEN @fleetname = 'hd'  THEN SUM(fnrls.daygroup9_hd)
				WHEN @fleetname = 'tt'  THEN SUM(fnrls.daygroup9_tt)
				WHEN @fleetname = 'op'  THEN SUM(fnrls.daygroup9_op)
			END
			AS [Daygroup9] ,
			
			SUM(ISNULL(cf.CommentsFilled,0)) as [CommentsFilled]
	
	FROM 
		[General].[Fact_NonRevLog_Summary] fnrls

	LEFT JOIN
		(
			SELECT 
				Lstwwd		, 
				OperStat	,  
				CountryCar	, 
				CountryLoc	, 
				KCICode		,  
				CarGroup	, 
				COUNT(*)	AS [CommentsFilled]
			FROM 
				[General].[Fact_NonRevLog]
			WHERE  
			(
				CONVERT(INT,CONVERT(VARCHAR(8),EndDate,112)) >= @min_date
			OR 
				(CONVERT(INT,CONVERT(VARCHAR(8),StartDate,112)) between @min_date and @max_date)
			)
			AND
				RemarkId > 1
		GROUP BY  
			Lstwwd , OperStat ,  CountryCar , CountryLoc , KCICode ,  CarGroup
		) 
			cf ON 
	
			fnrls.location		= cf.Lstwwd 
		AND fnrls.country_car	= cf.CountryCar 
		AND fnrls.country_loc	= cf.CountryLoc
		AND fnrls.car_group		= cf.CarGroup

	WHERE 
		fnrls.country_car IN (SELECT country FROM COUNTRIES WHERE active = 1)
	AND 
		((@country IS NULL) OR(fnrls.country_loc = @country))
	AND 
		fnrls.dim_calendar_id IN (SELECT dim_calendar_id FROM @calendar_ids)
	AND 
		((@cms_pool_id IS NULL) OR (fnrls.location_pool_id = @cms_pool_id))
	AND 
		((@cms_location_group_id IS NULL) OR (fnrls.location_group_id = @cms_location_group_id))
	AND 
		((@ops_region_id IS NULL) OR (fnrls.location_ops_region_id = @ops_region_id))
	AND 
		((@ops_area_id IS NULL) OR (fnrls.location_ops_area_id = @ops_area_id))
	AND 
		((@location IS NULL) OR (fnrls.location = @location))		
	AND 
		((@car_segment_id IS NULL) OR (fnrls.car_segment_id = @car_segment_id))
	AND 
		((@car_class_id IS NULL) OR (fnrls.car_class_id = @car_class_id))
	AND 
		((@car_group_id IS NULL) OR (fnrls.car_group_id = @car_group_id))
	AND 
		((@operstat_name		 IS NULL) OR (fnrls.OperationalStatusCode IN 
											(SELECT Items FROM dbo.fSplit(@operstat_name,','))))
	AND
		((@day_of_week			 IS NULL) OR (fnrls.Rep_DayOfWeek	= @day_of_week))


	GROUP BY
		CASE 
			WHEN @grouplevel = 'country'	THEN fnrls.country_loc 
			WHEN @grouplevel = 'pool'		THEN fnrls.location_pool
			WHEN @grouplevel = 'group'		THEN fnrls.location_group
			WHEN @grouplevel = 'region'		THEN fnrls.location_ops_region
			WHEN @grouplevel = 'area'		THEN fnrls.location_ops_area
			WHEN @grouplevel = 'location'	THEN fnrls.location
			WHEN @grouplevel = 'cargroup'	THEN fnrls.car_group
			WHEN @grouplevel = 'carclass'	THEN fnrls.car_class
			WHEN @grouplevel = 'carsegment'	THEN fnrls.car_segment
		END 
	) t
	GROUP BY 
		t.GroupLevel
	ORDER BY 
		t.GroupLevel
	
	DELETE FROM @table_report WHERE position = 2 AND grouplevel IS NULL
	
	
	--select * from @table_report
	
	-- Region : Table #Output
	--=========================================================================================
	CREATE TABLE #table_output
	(
		position SMALLINT , grouplevel VARCHAR(100) , TotalFleet INT , TotalFleetNR INT,
		daygroup1 INT , daygroup2 INT , daygroup3 INT , daygroup4 INT , daygroup5 INT ,
		daygroup6 INT , daygroup7 INT , daygroup8 INT , daygroup9 INT, commentsfilled INT
	)
	
	-- Insert Row TOTAL
	------------------------------------
	INSERT INTO #table_output 
	(
		position , grouplevel , TotalFleet , TotalFleetNR,
		daygroup1 , daygroup2 , daygroup3 , daygroup4 , daygroup5 , daygroup6 ,
		daygroup7 , daygroup8 , daygroup9 , commentsfilled
	)
	
	SELECT 
		1 , 
		'TOTAL' ,  
		ISNULL(SUM(TotalFleet),0)	, 
		ISNULL(SUM(TotalFleetNR),0) ,
		ISNULL(SUM(daygroup1),0)	, 
		ISNULL(SUM(daygroup2),0)	,
		ISNULL(SUM(daygroup3),0)	,
		ISNULL(SUM(daygroup4),0)	,
		ISNULL(SUM(daygroup5),0)	,
		ISNULL(SUM(daygroup6),0)	,
		ISNULL(SUM(daygroup7),0)	,
		ISNULL(SUM(daygroup8),0)	,
		ISNULL(SUM(daygroup9),0)	, 
		ISNULL(SUM(commentsfilled),0)
	FROM 
		@table_report

	-- Insert Row GROUP LEVELS
	------------------------------------

	INSERT INTO #table_output 
	(
		position , grouplevel , TotalFleet , TotalFleetNR,
		daygroup1 , daygroup2 , daygroup3 , daygroup4 , daygroup5 , daygroup6 ,
		daygroup7 , daygroup8 , daygroup9 , commentsfilled
	)

	SELECT 
		position  , grouplevel , TotalFleet , TotalFleetNR ,
		daygroup1 , daygroup2  , daygroup3  , daygroup4    , 
		daygroup5 , daygroup6  , daygroup7  , daygroup8    , 
		daygroup9 , commentsfilled 
	FROM 
		@table_report
		
	--select * from #table_output
	
	-- Region : Grid View Data
	--=========================================================================================
	DECLARE @PAGING TABLE
	(
		pageIndex		INT IDENTITY (1,1) NOT NULL,
		rowId			INT NULL
	)
	
	DECLARE @RESULT TABLE 
	(
		rowId				INT IDENTITY (1,1) NOT NULL,
		GroupLevel			VARCHAR(100),
		Fleet				INT,
		NonRev				INT,
		Remarks				INT,
		PctNonRev			VARCHAR(20),
		PctFleet			VARCHAR(20)
	)

	DECLARE @table_columnstring TABLE (id INT IDENTITY,columnname VARCHAR(20))
	
	INSERT INTO @table_columnstring (columnname)
	SELECT 
		CASE
			WHEN @fleetname = 'all' THEN LOWER(DayGroupName) 
		ELSE	
			LOWER(DayGroupName) + '_' + @fleetname
		END
		AS [columns]
	FROM [Settings].[NonRev_Day_Groups]
	WHERE 
		((@daygroupcode is null) OR (DayGroupCode in (SELECT Items FROM dbo.fSplit(@daygroupcode,','))))


	DECLARE @counter INT = 1
	DECLARE @columnstring VARCHAR(255) , @COLUMN varchar(20)
	WHILE @counter <= (SELECT MAX(id) FROM @table_columnstring)	
	BEGIN
		SET @column = (SELECT columnname FROM @table_columnstring WHERE id = @counter)
		SET @columnstring = ISNULL(@columnstring,'') + @column + ' + '
		SET @counter = @counter +1 
	END

	--SET @columnstring = SUBSTRING(@columnstring, 1, (LEN(@columnstring) - 1))
	
	--SET @columnstring = 
	--SUBSTRING(@columnstring, 1,
	--case when CHARINDEX('_',@columnstring) = 0 then LEN(@columnstring) else CHARINDEX('_',@columnstring)- 1 end)
	
	set @columnstring = replace(@columnstring,'_all','')
	set @columnstring = replace(@columnstring,'_cs','')
	set @columnstring = replace(@columnstring,'_lc','')
	set @columnstring = replace(@columnstring,'_ad','')
	set @columnstring = replace(@columnstring,'_hd','')
	set @columnstring = replace(@columnstring,'_tt','')
	set @columnstring = replace(@columnstring,'_op','')
	

	DECLARE @sql_command VARCHAR(max)
	SET @sql_command = ' select grouplevel as [GroupLevel], TotalFleet as [TotalFleet], '
	SET @sql_command = @sql_command + @columnstring + ' as [TotalFleetNR],'
	SET @sql_command = @sql_command + ' commentsFilled  '
	SET @sql_command = @sql_command + ' from  #table_output'
	
	SET @sql_command = REPLACE(@sql_command,'daygroup9 +','daygroup9')
	SET @sql_command = REPLACE(@sql_command,'+ as',' as')
	SET @sql_command = REPLACE(@sql_command,'+  as',' as')
	SET @sql_command = REPLACE(@sql_command,'+)',')')

	INSERT INTO @RESULT	
	(GroupLevel , Fleet , NonRev , Remarks)
	
	EXEC  (@sql_command)

	DROP TABLE #table_output
	
	--select * from @RESULT
		
	DECLARE @totaFleet INT , @totalFleetNonRev INT
	
	SET @totaFleet			= (SELECT SUM(Fleet) FROM @RESULT WHERE GroupLevel <> 'TOTAL')
	
	SET @totalFleetNonRev	= (SELECT SUM(NonRev) FROM @RESULT WHERE GroupLevel <> 'TOTAL')
	
	IF @totalFleetNonRev = 0 SET @totalFleetNonRev = 1
	
	--UPDATE @RESULT SET
	--	PctFleet  = CONVERT(VARCHAR,CONVERT(NUMERIC(5,2),
	--	CAST( convert(float,replace(Fleet,',','')) AS FLOAT) * 100.00 / 
	--		CAST(@totaFleet AS FLOAT))) + ' %' , 
		
	--	PctNonRev = CONVERT(VARCHAR,CONVERT(NUMERIC(5,2),
	--	CAST( convert(float,replace(Fleet,',','')) AS FLOAT) * 100.00 / 
	--		CAST(@totalFleetNonRev AS FLOAT))) + ' %'
			
	UPDATE @RESULT SET
		PctFleet  = CONVERT(VARCHAR,CONVERT(NUMERIC(5,2),
		CAST( convert(float,replace(NonRev,',','')) AS FLOAT) * 100.00 / 
			CAST(case when Fleet = 0 then 1 else Fleet end AS FLOAT))) + ' %' , 
		
		PctNonRev = CONVERT(VARCHAR,CONVERT(NUMERIC(5,2),
		CAST( convert(float,replace(Remarks,',','')) AS FLOAT) * 100.00 / 
			CAST(@totalFleetNonRev AS FLOAT))) + ' %'
	


	IF @sortExpression IS NULL SET @sortExpression = 'Fleet DESC'

	INSERT INTO @PAGING (rowId) SELECT rowId FROM @RESULT
	ORDER BY
	CASE WHEN @sortExpression = 'GroupLevel'		THEN GroupLevel END ASC,
	CASE WHEN @sortExpression = 'GroupLevel DESC'	THEN GroupLevel END DESC,
	CASE WHEN @sortExpression = 'Fleet'				THEN Fleet END ASC,
	CASE WHEN @sortExpression = 'Fleet DESC'		THEN Fleet END DESC,
	CASE WHEN @sortExpression = 'NonRev'			THEN NonRev END ASC,
	CASE WHEN @sortExpression = 'NonRev DESC'		THEN NonRev END DESC,
	CASE WHEN @sortExpression = 'Remarks'			THEN Remarks END ASC,
	CASE WHEN @sortExpression = 'Remarks DESC'		THEN Remarks END DESC,
	CASE WHEN @sortExpression = 'PctNonRev'			THEN PctNonRev END ASC,
	CASE WHEN @sortExpression = 'PctNonRev DESC'	THEN PctNonRev END DESC,	
	CASE WHEN @sortExpression = 'PctFleet'			THEN PctFleet END ASC,
	CASE WHEN @sortExpression = 'PctFleet DESC'		THEN PctFleet END DESC



	SELECT 
		CS.GroupLevel ,
		CS.Fleet , CS.NonRev , CS.Remarks, CS.PctNonRev ,
		CS.PctFleet 
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