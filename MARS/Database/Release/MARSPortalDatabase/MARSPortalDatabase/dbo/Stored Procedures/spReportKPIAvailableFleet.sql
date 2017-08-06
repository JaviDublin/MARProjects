
CREATE PROCEDURE [dbo].[spReportKPIAvailableFleet]

	@country				VARCHAR(10) = NULL	,
	@cms_pool_id			INT			= NULL	,
	@cms_location_group_id	int			= NULL	, 
	@ops_region_id			INT			= NULL	,
	@ops_area_id			INT			= NULL	,	
	@location				VARCHAR(50) = NULL	,	
	@fleet_name				VARCHAR(50) = NULL	,
	@car_segment_id			INT			= NULL	,
	@car_class_id			INT			= NULL	,
	@car_group_id			INT			= NULL	,	
	@start_date				DATETIME			,
	@end_date				DATETIME			,
	@grouping_criteria		VARCHAR(10)			,
	@displayed_value		VARCHAR(10) --Ignored in this report as we display percentage value only


AS
/*
exec spReportKPIAvailableFleet
		@start_date = '20120227'
		, @end_date = '20120410'
		, @country = 'GE'
		, @grouping_criteria = 'WEEK'
		, @displayed_value = 'NUMERIC'
		, @location= 'GERGB60'
*/
	
	
BEGIN TRY
		
	SET NOCOUNT ON;
	
-- Add Trace
--===========================================================
	DECLARE @rowcount INT, @inserted INT, @updated INT
	DECLARE @entity VARCHAR(100)
	DECLARE @key1	VARCHAR(100)
	DECLARE @data2	VARCHAR(max)
	
	SELECT @data2	=	'@country=' + COALESCE(@country,'null')
					+ ',@cms_pool_id=' + COALESCE(CONVERT(VARCHAR(20),@cms_pool_id),'null')
					+ ',@cms_location_group_id=' + COALESCE(CONVERT(VARCHAR(20),@cms_location_group_id),'null')
					+ ',@ops_region_id=' + COALESCE(CONVERT(VARCHAR(20),@ops_region_id),'null')
					+ ',@ops_area_id=' + COALESCE(CONVERT(VARCHAR(20),@ops_area_id),'null')
					+ ',@location=' + COALESCE('''' + @location+ '''','null')
					+ ',@fleet_name=' + COALESCE('''' + @fleet_name+ '''','null')
					+ ',@car_segment_id=' + COALESCE(CONVERT(VARCHAR(20),@car_segment_id),'null')
					+ ',@car_class_id=' + COALESCE(CONVERT(VARCHAR(20),@car_class_id),'null')
					+ ',@car_group_id=' + COALESCE(CONVERT(VARCHAR(20),@car_group_id),'null')
					+ ',@start_date=' + '''' + COALESCE(CONVERT(VARCHAR(19),@start_date,126)+'''','null')
					+ ',@end_date=' + '''' + COALESCE(CONVERT(VARCHAR(19),@end_date,126)+'''','null')
					

	SELECT @entity = OBJECT_NAME(@@PROCID)
	SELECT @entity = coalesce(@entity,'test')
	SELECT @key1 = convert(varchar(20),@@spid)
	INSERT Trace (entity, key1, key2, data2)
	SELECT @entity, @key1, 'start', @data2
	

-- Set the grouping criteria
--=====================================================================================

	IF DATEDIFF(DAY,@start_date,@end_date) <= 30
	BEGIN
		SET @grouping_criteria = 'DAY'
	END
	ELSE IF DATEDIFF(DAY,@start_date,@end_date) > 30 AND DATEDIFF(DAY,@start_date,@end_date) <= 90
	BEGIN
		SET @grouping_criteria = 'WEEK'
	END
	ELSE
	BEGIN
		SET @grouping_criteria = 'MONTH'
	END
	
-- Set the displayed value
--=====================================================================================

	IF @displayed_value IS NULL SET @displayed_value = 'NUMERIC'

-- Locations
--===========================================================
	DECLARE @LocationFlag INT = 0
	SELECT @LocationFlag = 1
	WHERE COALESCE(@cms_pool_id, @cms_location_group_id, @ops_region_id, @ops_area_id, case when @location is not null then 1 else null end) is not null
		
	DECLARE @Locations TABLE (dim_Location_id INT)
	IF @LocationFlag = 1
		INSERT @Locations
		SELECT dim_Location_id
		FROM inp.dim_Location FES
		WHERE 
			((FES.LOCATION IN (SELECT L.location	FROM LOCATIONS AS L INNER JOIN CMS_LOCATION_GROUPS AS CLG ON L.cms_location_group_id = CLG.cms_location_group_id WHERE CLG.cms_pool_id = @cms_pool_id)) OR @cms_pool_id IS NULL) -- CMS_POOLS
		AND ((FES.LOCATION IN (SELECT location		FROM LOCATIONS WHERE cms_location_group_id = @cms_location_group_id)) OR @cms_location_group_id IS NULL) -- CMS_LOCATION_GROUPS
		AND ((FES.LOCATION IN (SELECT L.location	FROM LOCATIONS AS L INNER JOIN OPS_AREAS AS OA ON L.ops_area_id = OA.ops_area_id WHERE OA.ops_region_id = @ops_region_id)) OR @ops_region_id IS NULL) -- OPS_REGIONS
		AND ((FES.LOCATION IN (SELECT location		FROM LOCATIONS WHERE ops_area_id = @ops_area_id)) OR @ops_area_id IS NULL) -- OPS_AREAS
		AND (FES.LOCATION = @location OR @location IS NULL) -- Location
	ELSE
		INSERT @Locations SELECT DISTINCT dim_Location_id FROM inp.dim_Location
		
		
-- Car groups
--===========================================================
	DECLARE @CarGroupFlag INT = 0
	SELECT @CarGroupFlag = 1
	WHERE COALESCE(@car_segment_id, @car_class_id, @car_group_id) is not null
	DECLARE @CarGroups TABLE (CarGroup VARCHAR(10))
	
	IF @CarGroupFlag = 1
		INSERT @CarGroups
		SELECT DISTINCT CAR_GROUP
		FROM CAR_GROUPS FES
		WHERE 
			((FES.CAR_GROUP IN (SELECT CG.car_group FROM CAR_GROUPS AS CG INNER JOIN CAR_CLASSES AS CC ON CG.car_class_id = CC.car_class_id WHERE CC.car_segment_id = @car_segment_id)) OR @car_segment_id IS NULL) --@car_segment_id
		AND ((FES.CAR_GROUP IN (SELECT CG.car_group FROM CAR_GROUPS AS CG WHERE CG.car_class_id = @car_class_id)) OR @car_class_id IS NULL) --@car_class_id
		AND ((FES.CAR_GROUP = (SELECT car_group		FROM CAR_GROUPS WHERE car_group_id = @car_group_id)) OR @car_group_id IS NULL) --@car_group_id
	ELSE
		INSERT @CarGroups SELECT null		
	
-- Dates
--===========================================================

	DECLARE @StartFullPeriod	DATETIME
	DECLARE @EndFullPeriod		DATETIME
	DECLARE @StartDateEnd		DATETIME
	DECLARE @EndDateStart		DATETIME
	
	
	DECLARE @periods TABLE 
		(
			dim_calendar_id INT, dim_calendar_id_end INT, PeriodStart DATETIME, 
			PeriodEnd DATETIME, type VARCHAR(1)
		)
	

	SELECT	@StartFullPeriod = MIN(PeriodStart) from @periods		
	SELECT	@EndFullPeriod = max(PeriodEnd) from @periods	
	SELECT 	@StartFullPeriod = COALESCE(@StartFullPeriod,'25000101')
	SELECT 	@EndFullPeriod = coalesce(@EndFullPeriod,'19000101')

	INSERT	@periods
	SELECT	m.min_dim_calendar_id, m.max_dim_calendar_id, m.PeriodStart, m.PeriodEnd, convert(varchar(20),Type)
	FROM	inp.Data_Aggregate m
	WHERE	(	m.PeriodStart between @start_date and @end_date
			and m.PeriodEnd between @start_date and @end_date
			and	(	m.PeriodEnd < @StartFullPeriod
				or	m.PeriodStart > @EndFullPeriod
				)
			)
	and		m.Type = 1

	SELECT	@StartFullPeriod	= MIN(PeriodStart) ,
			@EndFullPeriod		= MAX(PeriodEnd)
	FROM @periods
	
	IF @StartFullPeriod > @EndFullPeriod or @StartFullPeriod is null or @EndFullPeriod is null
		SELECT	@StartDateEnd = @end_date ,
				@EndDateStart = null
	ELSE
		SELECT	@StartDateEnd = @StartFullPeriod - 1 ,
				@EndDateStart = @EndFullPeriod + 1
	
-- Create Table
--===========================================================	

	CREATE TABLE #TMP_TREND
	(
		dim_calendar_id		INT,
		NumDays				INT ,			
		rowtype				VARCHAR(1) ,
		REP_YEAR			VARCHAR(4),
		REP_MONTH			VARCHAR(2),
		REP_WEEK_OF_YEAR	VARCHAR(2),
		REP_DATE			DATETIME,	
		RT					INT,
		SU					INT,
		GOLD				INT,
		PREDELIVERY			INT,
		OVERDUE				INT,
		TOTAL_FLEET			INT		
	)

-- Insert Data
--===========================================================

	IF @grouping_criteria = 'DAY'
	BEGIN
		INSERT INTO #TMP_TREND
		(
			dim_calendar_id, NumDays , rowtype,
			REP_YEAR, REP_MONTH, REP_WEEK_OF_YEAR, 
			REP_DATE, RT, SU, GOLD, PREDELIVERY, OVERDUE , TOTAL_FLEET
		)
		
		SELECT 
			FES.dim_calendar_id, 
			NumDays = 1 ,
			rowtype = 'D' ,
			d.REP_YEAR, d.REP_MONTH, d.Rep_WeekOfYear , DATEADD(d, 0, DATEDIFF(d, 0, d.REP_DATE)), 
			SUM(RT) AS 'RT', 
			SUM(SU) AS 'SU', 
			SUM(GOLD) AS 'GOLD', 
			SUM(PREDELIVERY) AS 'PREDELIVERY', 
			SUM(OVERDUE) AS 'OVERDUE' ,
			SUM(TOTAL_FLEET) AS [TOTAL_FLEET]
		FROM 
			Inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY AS FES
		JOIN @Locations l
				ON l.dim_Location_id = FES.dim_Location_id	
		JOIN @CarGroups cg
				ON cg.CarGroup = FES.Car_Group OR cg.CarGroup is null
		JOIN inp.dim_Calendar d
				ON d.dim_calendar_id = FES.dim_calendar_id	
		WHERE
			(d.Rep_Date between @start_date AND @end_date)
		AND			
			(FES.COUNTRY = @country OR @country IS NULL) -- Country
		AND (	(	@fleet_name = 'CARSALES'
					AND FES.FLEET_CARSALES > 0
				)
				OR	(	@fleet_name = 'RAC OPS'
					AND FES.FLEET_RAC_OPS > 0
					)
				OR	(	@fleet_name = 'RAC TTL'
					AND FES.FLEET_RAC_TTL > 0
					)
				OR	(	@fleet_name = 'ADVANTAGE'
					AND FES.FLEET_ADV > 0
					)
				OR	(	@fleet_name = 'HERTZ ON DEMAND'
					AND FES.FLEET_HOD > 0
					)
				OR	(	@fleet_name = 'LICENSEE'
					AND FES.FLEET_LICENSEE > 0
					)
				OR	(	@fleet_name IS NULL
					AND	(	FES.FLEET_CARSALES > 0
						OR FES.FLEET_RAC_TTL > 0
						)
					)
				)
		GROUP BY 
			FES.dim_calendar_id ,d.REP_YEAR, d.REP_MONTH, d.Rep_WeekOfYear , d.REP_DATE
		
		SELECT @rowcount = @@rowcount
		INSERT trace (entity, key1, key2, data1)
		SELECT @entity, @key1, 'Insert Days', 'updated=' + COALESCE(CONVERT(VARCHAR(20),@rowcount),'null')		
	END
	ELSE
	BEGIN
		INSERT INTO #TMP_TREND
		(
			dim_calendar_id, NumDays , rowtype,
			REP_YEAR, REP_MONTH, REP_WEEK_OF_YEAR, 
			REP_DATE, RT, SU, GOLD, PREDELIVERY, OVERDUE , TOTAL_FLEET
		)		

		SELECT 
			FES.dim_calendar_id, 
			NumDays = 1 ,
			rowtype = 'D' ,
			d.REP_YEAR, d.REP_MONTH, d.Rep_WeekOfYear , DATEADD(d, 0, DATEDIFF(d, 0, d.REP_DATE)),
			SUM(RT) AS 'RT', 
			SUM(SU) AS 'SU', 
			SUM(GOLD) AS 'GOLD', 
			SUM(PREDELIVERY) AS 'PREDELIVERY', 
			SUM(OVERDUE) AS 'OVERDUE' ,
			SUM(TOTAL_FLEET) AS [TOTAL_FLEET]
		FROM 
			inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY FES
		JOIN [Inp].dim_calendar d
			ON d.dim_calendar_id = FES.dim_calendar_id
		JOIN @Locations l
			ON l.dim_Location_id = FES.dim_Location_id
		JOIN @CarGroups cg
			ON cg.CarGroup = FES.Car_Group
				OR cg.CarGroup IS NULL
		WHERE
			(	d.Rep_Date between @start_date AND @StartDateEnd
			or	d.Rep_Date between @EndDateStart AND @end_date
			)
			AND (FES.COUNTRY = @country OR @country IS NULL) -- Country
			AND (	(	@fleet_name = 'CARSALES'
					AND FES.FLEET_CARSALES > 0
					)
				OR	(	@fleet_name = 'RAC OPS'
					AND FES.FLEET_RAC_OPS > 0
					)
				OR	(	@fleet_name = 'RAC TTL'
					AND FES.FLEET_RAC_TTL > 0
					)
				OR	(	@fleet_name = 'ADVANTAGE'
					AND FES.FLEET_ADV > 0
					)
				OR	(	@fleet_name = 'HERTZ ON DEMAND'
					AND FES.FLEET_HOD > 0
					)
				OR	(	@fleet_name = 'LICENSEE'
					AND FES.FLEET_LICENSEE > 0
					)
				OR	(	@fleet_name IS NULL
					AND	(	FES.FLEET_CARSALES > 0
						OR FES.FLEET_RAC_TTL > 0
						)
					)
				)--Fleet
		GROUP BY 
			FES.dim_calendar_id , d.REP_YEAR, d.REP_MONTH, d.Rep_WeekOfYear , d.REP_DATE
			
		SELECT @rowcount = @@rowcount
		INSERT trace (entity, key1, key2, data1)
		SELECT @entity, @key1, 'Insert Days', 'updated=' + COALESCE(CONVERT(VARCHAR(20),@rowcount),'null')	
			
		INSERT INTO #TMP_TREND
		(
			dim_calendar_id, NumDays , rowtype,
			REP_YEAR, REP_MONTH, REP_WEEK_OF_YEAR, 
			REP_DATE, RT, SU, GOLD, PREDELIVERY, OVERDUE , TOTAL_FLEET
		)		
		SELECT 
			FES.dim_calendar_id_start, 
			NumDays = MAX(NumDays) ,
			rowtype = 'W' ,
			d.REP_YEAR, d.REP_MONTH, d.Rep_WeekOfYear , DATEADD(d, 0, DATEDIFF(d, 0, d.REP_DATE)),	
			SUM(RT)/MAX(NumDays) AS 'RT', 
			SUM(SU)/MAX(NumDays) AS 'SU', 
			SUM(GOLD)/MAX(NumDays) AS 'GOLD', 
			SUM(PREDELIVERY)/MAX(NumDays) AS 'PREDELIVERY', 
			SUM(OVERDUE)/MAX(NumDays) AS 'OVERDUE',
			SUM(TOTAL_FLEET)/MAX(NumDays) AS [TOTAL_FLEET]

		FROM 
			Inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY_Aggregate FES
				join @periods m
					on FES.dim_Calendar_id_start = m.dim_calendar_id
					and m.type = FES.Type
		JOIN [Inp].dim_calendar d
			ON d.dim_calendar_id = FES.dim_calendar_id_start
		JOIN @Locations l
			ON l.dim_Location_id = FES.dim_Location_id
		JOIN @CarGroups cg
			ON cg.CarGroup = FES.Car_Group
				OR cg.CarGroup IS NULL
		WHERE
			(	
				d.Rep_Date between @start_date AND @end_date
			)
			AND (FES.COUNTRY = @country OR @country IS NULL) -- Country
			AND (	(	@fleet_name = 'CARSALES'
					AND FES.FLEET_CARSALES > 0
					)
				OR	(	@fleet_name = 'RAC OPS'
					AND FES.FLEET_RAC_OPS > 0
					)
				OR	(	@fleet_name = 'RAC TTL'
					AND FES.FLEET_RAC_TTL > 0
					)
				OR	(	@fleet_name = 'ADVANTAGE'
					AND FES.FLEET_ADV > 0
					)
				OR	(	@fleet_name = 'HERTZ ON DEMAND'
					AND FES.FLEET_HOD > 0
					)
				OR	(	@fleet_name = 'LICENSEE'
					AND FES.FLEET_LICENSEE > 0
					)
				OR	(	@fleet_name IS NULL
					AND	(	FES.FLEET_CARSALES > 0
						OR FES.FLEET_RAC_TTL > 0
						)
					)
				)--Fleet
		GROUP BY 
			FES.dim_calendar_id_start , d.REP_YEAR, d.REP_MONTH, d.Rep_WeekOfYear , d.REP_DATE 
		
		SELECT @rowcount = @@rowcount
		INSERT trace (entity, key1, key2, data1)
		SELECT @entity, @key1, 'Insert Weeks', 'updated=' + COALESCE(CONVERT(VARCHAR(20),@rowcount),'null')		
	END
		
		
		
	SELECT @rowcount = @@rowcount
	INSERT Trace (entity, key1, key2, data1)
	SELECT @entity, @key1, 'Insert KPI Available Fleet', 'updated=' + COALESCE(CONVERT(VARCHAR(20),@rowcount),'null')		
	
-- Select Data
--===========================================================
	IF (@displayed_value = 'NUMERIC') --Display VALUE
	BEGIN
		IF (@grouping_criteria = 'DAY')
		BEGIN
			SELECT 
				REP_YEAR, 
				REP_MONTH, 
				REP_WEEK_OF_YEAR, 
				REP_DATE, 
				RT, 
				SU, 
				GOLD, 
				PREDELIVERY, 
				OVERDUE 
			FROM 
				#TMP_TREND 
			ORDER BY 
				REP_DATE
		END
		ELSE 
		BEGIN
			IF (@grouping_criteria = 'WEEK')
			BEGIN
				SELECT 
					REP_YEAR, 
					SUBSTRING(CONVERT(VARCHAR(8),dim_calendar_id),5,2) AS 'REP_MONTH',
					DATENAME(m, STR(SUBSTRING(CONVERT(VARCHAR(8),dim_calendar_id),5,2)) + '/1/2012') + ' - week ' +
					REP_WEEK_OF_YEAR AS [REP_WEEK_OF_YEAR],
					AVG(RT) AS [RT], 
					AVG(SU) AS [SU], 
					AVG(GOLD) AS [GOLD], 
					AVG(PREDELIVERY) AS [PREDELIVERY], 
					AVG(OVERDUE) AS [OVERDUE]
				FROM 
					#TMP_TREND 
				GROUP BY 
					REP_YEAR, REP_WEEK_OF_YEAR , SUBSTRING(CONVERT(VARCHAR(8),dim_calendar_id),5,2)
				ORDER BY 
					REP_YEAR, CONVERT(INT,REP_WEEK_OF_YEAR) ASC
			END 
			ELSE
			BEGIN				
				IF (@grouping_criteria = 'MONTH')
				BEGIN
					SELECT 
						REP_YEAR, 
						DATENAME(m, STR(REP_MONTH) + '/1/2012') + ' ' + SUBSTRING(REP_YEAR,3,2) AS [REP_MONTH], 
						DATENAME(m, STR(REP_MONTH) + '/1/2012') + ' ' + SUBSTRING(REP_YEAR,3,2) AS [REP_WEEK_OF_YEAR] ,
						CONVERT(DATETIME,'01/' + REP_MONTH + '/' + REP_YEAR,103) AS [REP_DATE],	
						AVG(RT) AS [RT], 
						AVG(SU) AS [SU], 
						AVG(GOLD) AS [GOLD], 
						AVG(PREDELIVERY) AS [PREDELIVERY], 
						AVG(OVERDUE) AS [OVERDUE]
					FROM 
						#TMP_TREND 
					GROUP BY 
						REP_YEAR, REP_MONTH 
					ORDER BY 
						CONVERT(INT,REP_YEAR) ASC, CONVERT(INT,REP_MONTH) ASC
				END 
			END
		END	
	END
	ELSE
	BEGIN
		IF (@grouping_criteria = 'DAY')
		BEGIN
			SELECT 
				REP_YEAR, 
				REP_MONTH, 
				REP_WEEK_OF_YEAR, 
				REP_DATE, 
				RT * 100.0 / TOTAL_FLEET AS [RT], 
				SU * 100.0 / TOTAL_FLEET AS [SU], 
				GOLD * 100.0 / TOTAL_FLEET AS [GOLD], 
				PREDELIVERY * 100.0 / TOTAL_FLEET AS [PREDELIVERY], 
				OVERDUE * 100.0 / TOTAL_FLEET AS [OVERDUE]
			FROM 
				#TMP_TREND 
			
			--GROUP BY
			--	REP_YEAR, 
			--	REP_MONTH, 
			--	REP_WEEK_OF_YEAR, 
			--	REP_DATE,RT,SU,GOLD,PREDELIVERY,OVERDUE
			ORDER BY 
				REP_DATE
				
		END
		ELSE 
		BEGIN
			IF (@grouping_criteria = 'WEEK')
			BEGIN
				SELECT 
					REP_YEAR, 
					SUBSTRING(CONVERT(VARCHAR(8),dim_calendar_id),5,2) AS 'REP_MONTH',
					DATENAME(m, STR(SUBSTRING(CONVERT(VARCHAR(8),dim_calendar_id),5,2)) + '/1/2012') + ' - week ' +
					REP_WEEK_OF_YEAR AS [REP_WEEK_OF_YEAR],
					AVG(RT) * 100.0 / AVG(TOTAL_FLEET) AS [RT], 
					AVG(SU) * 100.0 / AVG(TOTAL_FLEET) AS [SU], 
					AVG(GOLD) * 100.0 / AVG(TOTAL_FLEET) AS [GOLD], 
					AVG(PREDELIVERY) * 100.0 / AVG(TOTAL_FLEET) AS [PREDELIVERY], 
					AVG(OVERDUE) * 100.0 / AVG(TOTAL_FLEET) AS [OVERDUE]
				FROM 
					#TMP_TREND 
				GROUP BY 
					REP_YEAR, REP_WEEK_OF_YEAR , SUBSTRING(CONVERT(VARCHAR(8),dim_calendar_id),5,2)
				ORDER BY 
					REP_YEAR, CONVERT(INT,REP_WEEK_OF_YEAR) ASC
			END 
			ELSE
			BEGIN				
				IF (@grouping_criteria = 'MONTH')
				BEGIN
					SELECT 
						REP_YEAR, 
						DATENAME(m, STR(REP_MONTH) + '/1/2012') + ' ' + SUBSTRING(REP_YEAR,3,2) AS [REP_MONTH], 
						DATENAME(m, STR(REP_MONTH) + '/1/2012') + ' ' + SUBSTRING(REP_YEAR,3,2) AS [REP_WEEK_OF_YEAR] ,
						CONVERT(DATETIME,'01/' + REP_MONTH + '/' + REP_YEAR,103) AS [REP_DATE],	
						AVG(RT) * 100.0 / AVG(TOTAL_FLEET) AS [RT], 
						AVG(SU) * 100.0 / AVG(TOTAL_FLEET) AS [SU], 
						AVG(GOLD) * 100.0 / AVG(TOTAL_FLEET) AS [GOLD], 
						AVG(PREDELIVERY) * 100.0 / AVG(TOTAL_FLEET) AS [PREDELIVERY], 
						AVG(OVERDUE) * 100.0 / AVG(TOTAL_FLEET) AS [OVERDUE]
					FROM 
						#TMP_TREND 
					GROUP BY 
						REP_YEAR, REP_MONTH 
					ORDER BY 
						CONVERT(INT,REP_YEAR) ASC, CONVERT(INT,REP_MONTH) ASC
				END 
			END
		END	
	END
	
	SELECT @rowcount = @@rowcount	
	
-- Drop the tempory table	
--=======================================================
	DROP TABLE #TMP_TREND		
	
	INSERT Trace (entity, key1, key2, data1)
	SELECT @entity, @key1, 'end', 'returned=' + coalesce(convert(varchar(20),@rowcount),'null')
	
			
	
END TRY
BEGIN CATCH
	DECLARE @ErrDesc VARCHAR(max)
	DECLARE @ErrProc VARCHAR(128)
	DECLARE @ErrLine VARCHAR(20)

	SELECT 	
		@ErrProc = ERROR_PROCEDURE() ,
		@ErrLine = ERROR_LINE() ,
		@ErrDesc = ERROR_MESSAGE()
	
	SELECT	
		@ErrProc = COALESCE(@ErrProc,'') ,
		@ErrLine = COALESCE(@ErrLine,'') ,
		@ErrDesc = COALESCE(@ErrDesc,'')
	
	INSERT	Trace (Entity, key1, data1, data2)
	SELECT	Entity = @entity, key1 = 'Failure',
		data1 = '<ErrProc=' + @ErrProc + '>'
			+ '<ErrLine=' + @ErrLine + '>'
			+ '<ErrDesc=' + @ErrDesc + '>',
		data2 = @key1
	RAISERROR('Failed %s', 16, -1, @ErrDesc)
END CATCH