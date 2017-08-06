
CREATE PROCEDURE [dbo].[spReportHistoricalTrend] 
	
	@country				VARCHAR(10) = NULL,
	@cms_pool_id			INT			= NULL,
	@cms_location_group_id	int			= NULL, 
	@ops_region_id			INT			= NULL,
	@ops_area_id			INT			= NULL,	
	@location				VARCHAR(50) = NULL,	
	@fleet_name				VARCHAR(50) = NULL,
	@car_segment_id			INT			= NULL,
	@car_class_id			INT			= NULL,
	@car_group_id			INT			= NULL,	
	@start_date				DATETIME		  ,
	@end_date				DATETIME		  ,
	@day_of_week			INT			= NULL,
	@grouping_criteria		VARCHAR(10)	= NULL,
	@displayed_value		VARCHAR(10)	= NULL,
	@OnRentType				int			= null
	
AS
BEGIN TRY
	
	SET NOCOUNT ON; 
	
-- Add Trace
--====================================================================
	DECLARE @rowcount INT, @inserted INT, @updated INT
	DECLARE @entity VARCHAR(100)
	DECLARE @key1	VARCHAR(100)
	DECLARE @data2	VARCHAR(max)
		
	SELECT @data2	=	'@country=' + coalesce(@country,'null')
						+ ',@cms_pool_id=' + coalesce(convert(varchar(20),@cms_pool_id),'null')
						+ ',@cms_location_group_id=' + coalesce(convert(varchar(20),@cms_location_group_id),'null')
						+ ',@ops_region_id=' + coalesce(convert(varchar(20),@ops_region_id),'null')
						+ ',@ops_area_id=' + coalesce(convert(varchar(20),@ops_area_id),'null')
						+ ',@location=' + coalesce('''' + @location+ '''','null')
						+ ',@fleet_name=' + coalesce('''' + @fleet_name+ '''','null')
						+ ',@car_segment_id=' + coalesce(convert(varchar(20),@car_segment_id),'null')
						+ ',@car_class_id=' + coalesce(convert(varchar(20),@car_class_id),'null')
						+ ',@car_group_id=' + coalesce(convert(varchar(20),@car_group_id),'null')
						+ ',@start_date=' + '''' + coalesce(convert(varchar(19),@start_date,126)+'''','null')
						+ ',@end_date=' + '''' + coalesce(convert(varchar(19),@end_date,126)+'''','null')
						+ ',@day_of_week=' + coalesce(convert(varchar(20),@day_of_week),'null')

	SELECT @entity = OBJECT_NAME(@@PROCID)
	SELECT @entity = COALESCE(@entity,'test')
	SELECT @key1 = CONVERT(varchar(20),@@spid)
	INSERT Trace (entity, key1, key2, data2)
	SELECT @entity, @key1, 'start', @data2

-- Set the grouping criteria
--=====================================================================================

	IF DATEDIFF(DAY,@start_date,@end_date) <= 30
	BEGIN
		IF DATEDIFF(DAY,@start_date,GETDATE()) <= 1
		BEGIN
			SET @grouping_criteria = 'HOUR'
		END
		ELSE
		BEGIN
			SET @grouping_criteria = 'DAY'
		END
	END
	ELSE IF DATEDIFF(DAY,@start_date,@end_date) > 30 AND DATEDIFF(DAY,@start_date,@end_date) <= 90
	BEGIN
		SET @grouping_criteria = 'WEEK'
		--SET @start_date = DATEADD(SECOND,59,DATEADD(MINUTE,59,DATEADD(HOUR,23,@start_date)))
		--SET @end_date   = DATEADD(SECOND,-59,DATEADD(MINUTE,-59,DATEADD(HOUR,-23,@end_date)))
	END
	ELSE
	BEGIN
		SET @grouping_criteria = 'MONTH'
		--SET @start_date = DATEADD(SECOND,59,DATEADD(MINUTE,59,DATEADD(HOUR,23,@start_date)))
		--SET @end_date   = DATEADD(SECOND,-59,DATEADD(MINUTE,-59,DATEADD(HOUR,-23,@end_date)))
	END
	
-- Set the displayed value
--=====================================================================================

	IF @displayed_value IS NULL SET @displayed_value = 'NUMERIC'

-- Create the table
--=====================================================================================
	CREATE TABLE #TMP_TREND
		(
			dim_calendar_id		INT,
			NumDays				INT ,			
			rowtype				VARCHAR(1) ,
			REP_YEAR			VARCHAR(4)	,
			REP_MONTH			VARCHAR(2)	,
			REP_WEEK_OF_YEAR	VARCHAR(2)	,
			REP_DATE			DATETIME	,	
			TOTAL_FLEET			INT,
			CARSALES			INT,		
			CU					INT,
			HA					INT,
			HL					INT,
			LL					INT,
			NC					INT,
			PL					INT,
			TC					INT,
			SV					INT,
			WS					INT,
			OPERATIONAL_FLEET	INT,
			BD					INT,
			MM					INT,
			TW					INT,
			TB					INT,		
			FS					INT,
			RL					INT,
			RP					INT,
			TN					INT,
			AVAILABLE_FLEET		INT,
			RT					INT,
			SU					INT,
			GOLD				INT,
			PREDELIVERY			INT,
			OVERDUE				INT,
			ON_RENT				INT		
		)

-- Car Groups Filter
--==================================================================

	DECLARE @CarGroupFlag int = 0
	SELECT @CarGroupFlag = 1
	WHERE COALESCE(@car_segment_id, @car_class_id, @car_group_id) is not null

	DECLARE @CarGroups TABLE (CarGroup VARCHAR(10))
	
	IF @CarGroupFlag = 1
	BEGIN
		INSERT @CarGroups
		SELECT 
			DISTINCT cg.car_group 
		FROM 
			CAR_GROUPS cg
		JOIN CAR_CLASSES  AS cc ON cg.car_class_id   = cc.car_class_id
		JOIN CAR_SEGMENTS AS cs ON cc.car_segment_id = cs.car_segment_id
		WHERE
			((@country IS NULL) OR (cs.country = @country))
		AND
			((@car_segment_id IS NULL) OR (cs.car_segment_id = @car_segment_id))
		AND
			((@car_class_id IS NULL) OR (cc.car_class_id = @car_class_id))
		AND
			((@car_group_id IS NULL) OR (cg.car_group_id = @car_group_id))
			
			IF (SELECT COUNT(*) FROM @CarGroups) = 0 
			BEGIN 
				INSERT @CarGroups SELECT NULL 
			END
	END		
	ELSE
	BEGIN
		INSERT @CarGroups SELECT NULL
	END
	
-- Locations Filter
--====================================================================
		

	DECLARE @LocationFlag int = 0
	SELECT @LocationFlag = 1
	WHERE COALESCE(@cms_pool_id, @cms_location_group_id, @ops_region_id, @ops_area_id, 
		CASE WHEN @location IS NOT NULL THEN 1 ELSE NULL END) IS NOT NULL

		
	DECLARE @Locations TABLE (dim_Location_id INT,Location VARCHAR(50))
	IF @LocationFlag = 1 
	BEGIN 
		INSERT @Locations
		SELECT 
			loc.dim_Location_id , loc.location
		FROM 
			Inp.dim_Location loc
		JOIN 
			CMS_LOCATION_GROUPS AS lg ON loc.cms_location_group_id = lg.cms_location_group_id
		JOIN
			CMS_POOLS AS cp ON lg.cms_pool_id = cp.cms_pool_id
		JOIN 
			OPS_AREAS AS ar ON loc.ops_area_id = ar.ops_area_id
		JOIN 
			OPS_REGIONS AS rg ON ar.ops_region_id = rg.ops_region_id
		WHERE
		--	((@country IS NULL) OR (loc.country = @country)) -- removed for testing 
		--AND 
			((@cms_pool_id IS NULL) OR (cp.cms_pool_id = @cms_pool_id))
		AND
			((@cms_location_group_id IS NULL) OR (lg.cms_location_group_id = @cms_location_group_id))
		AND
			((@ops_region_id IS NULL) OR (rg.ops_region_id  = @ops_region_id))
		AND
			((@ops_area_id IS NULL) OR (ar.ops_area_id = @ops_area_id))
		AND
			((@location IS NULL) OR (loc.location = @location))

	END
	ELSE
	BEGIN
		INSERT @Locations 
		SELECT 
			DISTINCT loc.dim_Location_id  , loc.location
		FROM 
			Inp.dim_Location loc
		--WHERE
		--	((@country IS NULL) OR (loc.country = @country)) -- removed for testing
	END
	
-- Calendar Filter
--====================================================================

	DECLARE @StartFullPeriod	DATETIME
	DECLARE @EndFullPeriod		DATETIME
	DECLARE @StartDateEnd		DATETIME
	DECLARE @EndDateStart		DATETIME
	
	DECLARE @periods TABLE 
	(dim_calendar_id INT, dim_calendar_id_end INT, PeriodStart DATETIME, PeriodEnd DATETIME, type VARCHAR(1))
	
	IF @day_of_week is null
	BEGIN

		select	@StartFullPeriod = MIN(PeriodStart) from @periods		
		select	@EndFullPeriod = max(PeriodEnd) from @periods	
		select 	@StartFullPeriod = coalesce(@StartFullPeriod,'25000101')
		select 	@EndFullPeriod = coalesce(@EndFullPeriod,'19000101')

		insert	@periods
		select	m.min_dim_calendar_id, m.max_dim_calendar_id, m.PeriodStart, m.PeriodEnd, convert(varchar(20),Type)
		from	inp.Data_Aggregate m
		where	(	m.PeriodStart between @start_date and @end_date
				and m.PeriodEnd between @start_date and @end_date
				and	(	m.PeriodEnd < @StartFullPeriod
					or	m.PeriodStart > @EndFullPeriod
					)
				)
		and		m.Type = 1



--		INSERT	@periods
--		SELECT	m.min_dim_calendar_id, m.max_dim_calendar_id, m.PeriodStart, m.PeriodEnd, 'W'
--		FROM	inp.WeekOfYearData m
--		WHERE	m.PeriodStart >= @start_date
--		AND		m.PeriodEnd <= @end_date
	END
	ELSE
	BEGIN
		INSERT	@periods
		SELECT	m.min_dim_calendar_id, m.max_dim_calendar_id, m.MonthStart, m.MonthEnd, 'M'
		FROM	inp.MonthlyData m
		WHERE	m.MonthStart >= @start_date
		AND		m.MonthEnd <= @end_date
	END

	SELECT	@StartFullPeriod = min(PeriodStart) ,
			@EndFullPeriod = max(PeriodEnd)
	FROM @periods
		
	IF @StartFullPeriod > @EndFullPeriod or @StartFullPeriod is null or @EndFullPeriod is null
	BEGIN
		SELECT	@StartDateEnd = @end_date ,
				@EndDateStart = null
	END
	ELSE
	BEGIN
		SELECT	@StartDateEnd = @StartFullPeriod - 1 ,
				@EndDateStart = @EndFullPeriod + 1
	END
	
	INSERT Trace (entity, key1, Key2, Data1)
	SELECT @entity, @key1, 'Periods',	'start=' + COALESCE(CONVERT(VARCHAR(19),@start_date,126),'null')
							+	',@StartDateEnd=' + COALESCE(CONVERT(VARCHAR(19),@StartDateEnd,126),'null')
							+	',@EndDateStart=' + COALESCE(CONVERT(VARCHAR(19),@EndDateStart,126),'null')
							+ 	',end=' + COALESCE(CONVERT(VARCHAR(19),@end_date,126),'null')
							+ 	',StartPeriod=' + COALESCE(CONVERT(VARCHAR(19),@StartFullPeriod,126),'null')
							+ 	',EndPeriod=' + COALESCE(CONVERT(VARCHAR(19),@EndFullPeriod,126),'null')

	
-- Insert Data in the temp. table
--=====================================================================================
	
	IF @grouping_criteria = 'HOUR'
	BEGIN
		INSERT INTO #TMP_TREND
		(
			dim_calendar_id, NumDays , rowtype, REP_YEAR , REP_MONTH , REP_WEEK_OF_YEAR , REP_DATE ,
			TOTAL_FLEET, CARSALES, CU, HA, HL, LL, NC, PL, TC, 
			SV, WS, OPERATIONAL_FLEET, BD, MM, TW, TB, FS, RL, 
			RP, TN, AVAILABLE_FLEET, RT, SU, GOLD, PREDELIVERY, 
			OVERDUE, ON_RENT
		)
		
		SELECT 
			NULL , 1 , 'H',
			FES.REP_YEAR, 
			FES.REP_MONTH, 
			FES.REP_WEEK_OF_YEAR, 
			FES.IMPORTDATE AS [REP_DATE],
			SUM(FES.TOTAL_FLEET), SUM(FES.CARSALES), SUM(FES.CU), SUM(FES.HA), SUM(FES.HL), 
			SUM(FES.LL), SUM(FES.NC), SUM(FES.PL), SUM(FES.TC), SUM(FES.SV), SUM(FES.WS), 
			SUM(FES.OPERATIONAL_FLEET), SUM(FES.BD), SUM(FES.MM), SUM(FES.TW), SUM(FES.TB), 
			SUM(FES.FS), SUM(FES.RL), SUM(FES.RP), SUM(FES.TN), SUM(FES.AVAILABLE_FLEET), 
			SUM(FES.RT), SUM(FES.SU), SUM(FES.GOLD), SUM(FES.PREDELIVERY), SUM(FES.OVERDUE), 
			SUM(FES.ON_RENT)  
		FROM 
			FLEET_EUROPE_STATS FES
		JOIN @Locations l
				ON l.Location = FES.LOCATION
		JOIN @CarGroups cg
				ON cg.CarGroup = FES.Car_Group
					OR cg.CarGroup IS NULL
		WHERE
			(FES.COUNTRY = @country OR @country IS NULL) -- Country
		AND 
		((	@fleet_name = 'CARSALES'
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
			FES.REP_YEAR, FES.REP_MONTH, FES.REP_WEEK_OF_YEAR, 
			FES.IMPORTDATE
		
		SELECT @rowcount = @@rowcount
		INSERT trace (entity, key1, key2, data1)
		SELECT @entity, @key1, 'Insert Hours', 'updated=' + COALESCE(CONVERT(VARCHAR(20),@rowcount),'null')		
	END
	ELSE IF @grouping_criteria = 'DAY'
	BEGIN
		INSERT INTO #TMP_TREND
		(
			dim_calendar_id, NumDays , rowtype, REP_YEAR , REP_MONTH , REP_WEEK_OF_YEAR , REP_DATE ,
			TOTAL_FLEET, CARSALES, CU, HA, HL, LL, NC, PL, TC, 
			SV, WS, OPERATIONAL_FLEET, BD, MM, TW, TB, FS, RL, 
			RP, TN, AVAILABLE_FLEET, RT, SU, GOLD, PREDELIVERY, 
			OVERDUE, ON_RENT
		)		

		SELECT 
			FES.dim_calendar_id, 
			NumDays = 1 ,
			rowtype = 'D' ,
			d.REP_YEAR, d.REP_MONTH, d.Rep_WeekOfYear , DATEADD(d, 0, DATEDIFF(d, 0, d.REP_DATE)),
			SUM(FES.TOTAL_FLEET), 
			SUM(FES.CARSALES), 
			SUM(FES.CU), 
			SUM(FES.HA), 
			SUM(FES.HL), 
			SUM(FES.LL), 
			SUM(FES.NC), 
			SUM(FES.PL), 
			SUM(FES.TC), 
			SUM(FES.SV), 
			SUM(FES.WS), 
			SUM(FES.OPERATIONAL_FLEET), 
			SUM(FES.BD), 
			SUM(FES.MM), 
			SUM(FES.TW), 
			SUM(FES.TB), 
			SUM(FES.FS), 
			SUM(FES.RL), 
			SUM(FES.RP), 
			SUM(FES.TN), 
			SUM(FES.AVAILABLE_FLEET), 
			SUM(FES.RT), 
			SUM(FES.SU), 
			SUM(FES.GOLD), 
			SUM(FES.PREDELIVERY), 
			SUM(FES.OVERDUE), 
			SUM(case @OnRentType  
						when '2' then FES.ON_RENT_MAX 
						when '3'  then FES.ON_RENT_MAX_ABSOLUTE 
						else FES.ON_RENT end  )
		FROM 
			Inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY FES
		JOIN [Inp].dim_calendar d
			ON d.dim_calendar_id = FES.dim_calendar_id
				AND	(	d.Rep_DayOfWeek = @day_of_week 
					OR	@day_of_week IS NULL
					)
		JOIN @Locations l
			ON l.dim_Location_id = FES.dim_Location_id
		JOIN @CarGroups cg
			ON cg.CarGroup = FES.Car_Group
				OR cg.CarGroup IS NULL
		WHERE
			(d.Rep_Date between @start_date AND @end_date)
		AND 
			(FES.COUNTRY = @country OR @country IS NULL) -- Country
		AND 
			((	@fleet_name = 'CARSALES'
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
	END	
	ELSE IF @grouping_criteria = 'WEEK' OR @grouping_criteria = 'MONTH' 
	BEGIN
	
		INSERT INTO #TMP_TREND
		(
			dim_calendar_id, NumDays , rowtype, REP_YEAR , REP_MONTH , REP_WEEK_OF_YEAR , REP_DATE ,
			TOTAL_FLEET, CARSALES, CU, HA, HL, LL, NC, PL, TC, 
			SV, WS, OPERATIONAL_FLEET, BD, MM, TW, TB, FS, RL, 
			RP, TN, AVAILABLE_FLEET, RT, SU, GOLD, PREDELIVERY, 
			OVERDUE, ON_RENT
		)		

		SELECT 
			FES.dim_calendar_id, 
			NumDays = 1 ,
			rowtype = 'D' ,
			d.REP_YEAR, d.REP_MONTH, d.Rep_WeekOfYear , DATEADD(d, 0, DATEDIFF(d, 0, d.REP_DATE)),
			SUM(FES.TOTAL_FLEET), 
			SUM(FES.CARSALES), 
			SUM(FES.CU), 
			SUM(FES.HA), 
			SUM(FES.HL), 
			SUM(FES.LL), 
			SUM(FES.NC), 
			SUM(FES.PL), 
			SUM(FES.TC), 
			SUM(FES.SV), 
			SUM(FES.WS), 
			SUM(FES.OPERATIONAL_FLEET), 
			SUM(FES.BD), 
			SUM(FES.MM), 
			SUM(FES.TW), 
			SUM(FES.TB), 
			SUM(FES.FS), 
			SUM(FES.RL), 
			SUM(FES.RP), 
			SUM(FES.TN), 
			SUM(FES.AVAILABLE_FLEET), 
			SUM(FES.RT), 
			SUM(FES.SU), 
			SUM(FES.GOLD), 
			SUM(FES.PREDELIVERY), 
			SUM(FES.OVERDUE), 
			SUM(case @OnRentType  
						when '2' then FES.ON_RENT_MAX 
						when '3'  then FES.ON_RENT_MAX_ABSOLUTE 
						else FES.ON_RENT end  )
		FROM 
			inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY FES
		JOIN [Inp].dim_calendar d
			ON d.dim_calendar_id = FES.dim_calendar_id
				AND	(	d.Rep_DayOfWeek = @day_of_week 
					OR	@day_of_week IS NULL
					)
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
		
		if @day_of_week is null
		begin		
			INSERT INTO #TMP_TREND
			(
				dim_calendar_id, NumDays , rowtype, REP_YEAR , REP_MONTH , REP_WEEK_OF_YEAR , REP_DATE ,
				TOTAL_FLEET, CARSALES, CU, HA, HL, LL, NC, PL, TC, 
				SV, WS, OPERATIONAL_FLEET, BD, MM, TW, TB, FS, RL, 
				RP, TN, AVAILABLE_FLEET, RT, SU, GOLD, PREDELIVERY, 
				OVERDUE, ON_RENT
			)		
			SELECT 
				FES.dim_calendar_id_start, 
				NumDays = MAX(NumDays) ,
				rowtype = 'W' ,
				d.REP_YEAR, d.REP_MONTH, d.Rep_WeekOfYear , DATEADD(d, 0, DATEDIFF(d, 0, d.REP_DATE)),			
				SUM(FES.TOTAL_FLEET)/MAX(NumDays), 
				SUM(FES.CARSALES)/MAX(NumDays), 
				SUM(FES.CU)/MAX(NumDays), 
				SUM(FES.HA)/MAX(NumDays), 
				SUM(FES.HL)/MAX(NumDays), 
				SUM(FES.LL)/MAX(NumDays), 
				SUM(FES.NC)/MAX(NumDays), 
				SUM(FES.PL)/MAX(NumDays), 
				SUM(FES.TC)/MAX(NumDays), 
				SUM(FES.SV)/MAX(NumDays), 
				SUM(FES.WS)/MAX(NumDays), 
				SUM(FES.OPERATIONAL_FLEET)/MAX(NumDays), 
				SUM(FES.BD)/MAX(NumDays), 
				SUM(FES.MM)/MAX(NumDays), 
				SUM(FES.TW)/MAX(NumDays), 
				SUM(FES.TB)/MAX(NumDays), 
				SUM(FES.FS)/MAX(NumDays), 
				SUM(FES.RL)/MAX(NumDays), 
				SUM(FES.RP)/MAX(NumDays), 
				SUM(FES.TN)/MAX(NumDays), 
				SUM(FES.AVAILABLE_FLEET)/MAX(NumDays), 
				SUM(FES.RT)/MAX(NumDays), 
				SUM(FES.SU)/MAX(NumDays), 
				SUM(FES.GOLD)/MAX(NumDays), 
				SUM(FES.PREDELIVERY)/MAX(NumDays), 
				SUM(FES.OVERDUE)/MAX(NumDays), 
				SUM(FES.ON_RENT)/MAX(NumDays) 
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
			INSERT Trace (entity, key1, key2, data1)
			SELECT @entity, @key1, 'Insert Weeks', 'updated=' + coalesce(convert(varchar(20),@rowcount),'null')		
		end
		else
		begin
			INSERT INTO #TMP_TREND
			(
				dim_calendar_id, NumDays , rowtype, REP_YEAR , REP_MONTH , REP_WEEK_OF_YEAR , REP_DATE ,
				TOTAL_FLEET, CARSALES, CU, HA, HL, LL, NC, PL, TC, 
				SV, WS, OPERATIONAL_FLEET, BD, MM, TW, TB, FS, RL, 
				RP, TN, AVAILABLE_FLEET, RT, SU, GOLD, PREDELIVERY, 
				OVERDUE, ON_RENT
			)		
			SELECT 
					FES.dim_calendar_id, 
					NumDays = MAX(NumDays) ,
					rowtype = 'W' ,
					max(d.REP_YEAR), max(d.REP_MONTH), max(d.Rep_WeekOfYear) , DATEADD(d, 0, DATEDIFF(d, 0, max(d.REP_DATE))),			
					SUM(FES.TOTAL_FLEET), 
					SUM(FES.CARSALES), 
					SUM(FES.CU), 
					SUM(FES.HA), 
					SUM(FES.HL), 
					SUM(FES.LL), 
					SUM(FES.NC), 
					SUM(FES.PL), 
					SUM(FES.TC), 
					SUM(FES.SV), 
					SUM(FES.WS), 
					SUM(FES.OPERATIONAL_FLEET), 
					SUM(FES.BD), 
					SUM(FES.MM), 
					SUM(FES.TW), 
					SUM(FES.TB), 
					SUM(FES.FS), 
					SUM(FES.RL), 
					SUM(FES.RP), 
					SUM(FES.TN), 
					SUM(FES.AVAILABLE_FLEET), 
					SUM(FES.RT), 
					SUM(FES.SU), 
					SUM(FES.GOLD), 
					SUM(FES.PREDELIVERY), 
					SUM(FES.OVERDUE), 
					SUM(FES.ON_RENT) 
				FROM 
					inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY_Month_DOW AS FES
						join @periods m
							on FES.dim_calendar_id = m.dim_calendar_id
						join inp.dim_calendar d
							on d.dim_Calendar_id = m.dim_Calendar_id
						join @Locations l
							on l.dim_Location_id = FES.dim_Location_id
						join @CarGroups cg
							on cg.CarGroup = FES.Car_Group
							or cg.CarGroup is null
				WHERE
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
						)--Fleet
					AND FES.Rep_DayOfWeek = @day_of_week
				GROUP BY 
					FES.dim_calendar_id

				select @rowcount = @@rowcount
				insert trace (entity, key1, key2, data1)
				select @entity, @key1, 'DOW', 'updated=' + coalesce(convert(varchar(20),@rowcount),'null')
		end
			
	END
	
	
-- Select Data
--=====================================================================================

	DECLARE @numdays FLOAT
	
	;WITH cte1 AS
	(
			SELECT numdays = MAX(NumDays) FROM #TMP_TREND GROUP BY dim_calendar_id
	) ,
	cte as
	(
			SELECT numdays = 1.0*SUM(numdays) FROM cte1
	)
	SELECT @numdays = numdays FROM cte
	

	IF (@displayed_value = 'NUMERIC')
	BEGIN
		IF @grouping_criteria = 'HOUR'
		BEGIN
			SELECT 
				REP_YEAR, REP_MONTH, REP_WEEK_OF_YEAR, 
				CONVERT(VARCHAR(8),REP_DATE,108) AS [REP_DATE] ,
				TOTAL_FLEET AS [TOTAL_FLEET], 
				CARSALES AS [CARSALES], 
				CU AS [CU], 
				HA AS [HA], 
				HL AS [HL], 
				LL AS [LL], 
				NC AS [NC], 
				PL AS [PL], 
				TC AS [TC], 
				SV AS [SV], 
				WS AS [WS], 
				OPERATIONAL_FLEET AS [OPERATIONAL_FLEET], 
				BD AS [BD], 
				MM AS [MM], 
				TW AS [TW], 
				TB AS [TB], 
				WS AS [WS], 
				FS AS [FS], 
				RL AS [RL], 
				RP AS [RP], 
				TN AS [TN], 
				AVAILABLE_FLEET AS [AVAILABLE_FLEET], 
				RT AS [RT], 
				SU AS [SU], 
				GOLD AS [GOLD], 
				PREDELIVERY AS [PREDELIVERY], 
				OVERDUE AS [OVERDUE], 
				ON_RENT AS [ON_RENT]
			FROM 
				#TMP_TREND 
			ORDER BY 
				CONVERT(VARCHAR(8),REP_DATE,108)
		END
		ELSE IF @grouping_criteria = 'DAY'
		BEGIN
			SELECT 
				REP_YEAR, REP_MONTH, REP_WEEK_OF_YEAR, REP_DATE ,
				TOTAL_FLEET AS [TOTAL_FLEET], 
				CARSALES AS [CARSALES], 
				CU AS [CU], 
				HA AS [HA], 
				HL AS [HL], 
				LL AS [LL], 
				NC AS [NC], 
				PL AS [PL], 
				TC AS [TC], 
				SV AS [SV], 
				WS AS [WS], 
				OPERATIONAL_FLEET AS [OPERATIONAL_FLEET], 
				BD AS [BD], 
				MM AS [MM], 
				TW AS [TW], 
				TB AS [TB], 
				WS AS [WS], 
				FS AS [FS], 
				RL AS [RL], 
				RP AS [RP], 
				TN AS [TN], 
				AVAILABLE_FLEET AS [AVAILABLE_FLEET], 
				RT AS [RT], 
				SU AS [SU], 
				GOLD AS [GOLD], 
				PREDELIVERY AS [PREDELIVERY], 
				OVERDUE AS [OVERDUE], 
				ON_RENT AS [ON_RENT]
			FROM 
				#TMP_TREND 
			ORDER BY 
				REP_DATE
		END
		ELSE IF @grouping_criteria = 'WEEK'
		BEGIN
			SELECT 	
				REP_YEAR, 
				SUBSTRING(CONVERT(VARCHAR(8),dim_calendar_id),5,2) AS 'REP_MONTH', 
				DATENAME(m, STR(SUBSTRING(CONVERT(VARCHAR(8),dim_calendar_id),5,2)) + '/1/2012') + ' - week ' +
				REP_WEEK_OF_YEAR AS [REP_WEEK_OF_YEAR], 
				NULL AS [REP_DATE], 
				AVG(TOTAL_FLEET) AS [TOTAL_FLEET], 
				AVG(CARSALES) AS [CARSALES], 
				AVG(CU) AS [CU], 
				AVG(HA) AS [HA], 
				AVG(HL) AS [HL], 
				AVG(LL) AS [LL], 
				AVG(NC) AS [NC], 
				AVG(PL) AS [PL], 
				AVG(TC) AS [TC], 
				AVG(SV) AS [SV], 
				AVG(WS) AS [WS], 
				AVG(OPERATIONAL_FLEET) AS [OPERATIONAL_FLEET], 
				AVG(BD) AS [BD], 
				AVG(MM) AS [MM], 
				AVG(TW) AS [TW], 
				AVG(TB) AS [TB], 
				AVG(WS) AS [WS], 
				AVG(FS) AS [FS], 
				AVG(RL) AS [RL], 
				AVG(RP) AS [RP], 
				AVG(TN) AS [TN], 
				AVG(AVAILABLE_FLEET) AS [AVAILABLE_FLEET], 
				AVG(RT) AS [RT], 
				AVG(SU) AS [SU], 
				AVG(GOLD) AS [GOLD], 
				AVG(PREDELIVERY) AS [PREDELIVERY], 
				AVG(OVERDUE) AS [OVERDUE], 
				AVG(ON_RENT) AS [ON_RENT]
			FROM 
				#TMP_TREND 
			GROUP BY 
				REP_YEAR, REP_WEEK_OF_YEAR , SUBSTRING(CONVERT(VARCHAR(8),dim_calendar_id),5,2)
			ORDER BY 
				REP_YEAR, CONVERT(INT,REP_WEEK_OF_YEAR) ASC
		END
		ELSE IF @grouping_criteria = 'MONTH'
		BEGIN			
			SELECT 
				REP_YEAR, 
				DATENAME(m, STR(REP_MONTH) + '/1/2012') + ' ' + SUBSTRING(REP_YEAR,3,2) AS [REP_MONTH], 
				DATENAME(m, STR(REP_MONTH) + '/1/2012') + ' ' + SUBSTRING(REP_YEAR,3,2) AS [REP_WEEK_OF_YEAR] ,
				CONVERT(DATETIME,'01/' + REP_MONTH + '/' + REP_YEAR,103) AS [REP_DATE],	
				AVG(TOTAL_FLEET) AS [TOTAL_FLEET], 
				AVG(CARSALES) AS [CARSALES], 
				AVG(CU) AS [CU], 
				AVG(HA) AS [HA], 
				AVG(HL) AS [HL], 
				AVG(LL) AS [LL], 
				AVG(NC) AS [NC], 
				AVG(PL) AS [PL], 
				AVG(TC) AS [TC], 
				AVG(SV) AS [SV], 
				AVG(WS) AS [WS], 
				AVG(OPERATIONAL_FLEET) AS [OPERATIONAL_FLEET], 
				AVG(BD) AS [BD], 
				AVG(MM) AS [MM], 
				AVG(TW) AS [TW], 
				AVG(TB) AS [TB], 
				AVG(WS) AS [WS], 
				AVG(FS) AS [FS], 
				AVG(RL) AS [RL], 
				AVG(RP) AS [RP], 
				AVG(TN) AS [TN], 
				AVG(AVAILABLE_FLEET) AS [AVAILABLE_FLEET], 
				AVG(RT) AS [RT], 
				AVG(SU) AS [SU], 
				AVG(GOLD) AS [GOLD], 
				AVG(PREDELIVERY) AS [PREDELIVERY], 
				AVG(OVERDUE) AS [OVERDUE], 
				AVG(ON_RENT) AS [ON_RENT]			
				--CONVERT(NUMERIC(12,0),SUM(TOTAL_FLEET)/@numdays) AS [TOTAL_FLEET],
				--CONVERT(NUMERIC(12,0),SUM(CARSALES)/@numdays) AS [CARSALES], 
				--CONVERT(NUMERIC(12,0),SUM(CU)/@numdays) AS [CU], 
				--CONVERT(NUMERIC(12,0),SUM(HA)/@numdays) AS [HA], 
				--CONVERT(NUMERIC(12,0),SUM(HL)/@numdays) AS [HL], 
				--CONVERT(NUMERIC(12,0),SUM(LL)/@numdays) AS [LL], 
				--CONVERT(NUMERIC(12,0),SUM(NC)/@numdays) AS [NC], 
				--CONVERT(NUMERIC(12,0),SUM(PL)/@numdays) AS [PL], 
				--CONVERT(NUMERIC(12,0),SUM(TC)/@numdays) AS [TC], 
				--CONVERT(NUMERIC(12,0),SUM(SV)/@numdays) AS [SV], 
				--CONVERT(NUMERIC(12,0),SUM(WS)/@numdays) AS [WS], 
				--CONVERT(NUMERIC(12,0),SUM(OPERATIONAL_FLEET)/@numdays) AS [OPERATIONAL_FLEET], 
				--CONVERT(NUMERIC(12,0),SUM(BD)/@numdays) AS [BD], 
				--CONVERT(NUMERIC(12,0),SUM(MM)/@numdays) AS [MM], 
				--CONVERT(NUMERIC(12,0),SUM(TW)/@numdays) AS [TW], 
				--CONVERT(NUMERIC(12,0),SUM(TB)/@numdays) AS [TB], 
				--CONVERT(NUMERIC(12,0),SUM(FS)/@numdays) AS [FS], 
				--CONVERT(NUMERIC(12,0),SUM(RL)/@numdays) AS [RL], 
				--CONVERT(NUMERIC(12,0),SUM(RP)/@numdays) AS [RP], 
				--CONVERT(NUMERIC(12,0),SUM(TN)/@numdays) AS [TN], 
				--CONVERT(NUMERIC(12,0),SUM(AVAILABLE_FLEET)/@numdays) AS [AVAILABLE_FLEET], 
				--CONVERT(NUMERIC(12,0),SUM(RT)/@numdays) AS [RT], 
				--CONVERT(NUMERIC(12,0),SUM(SU)/@numdays) AS [SU], 
				--CONVERT(NUMERIC(12,0),SUM(GOLD)/@numdays) AS [GOLD], 
				--CONVERT(NUMERIC(12,0),SUM(PREDELIVERY)/@numdays) AS [PREDELIVERY], 
				--CONVERT(NUMERIC(12,0),SUM(OVERDUE)/@numdays) AS [OVERDUE], 
				--CONVERT(NUMERIC(12,0),SUM(ON_RENT)/@numdays) AS [ON_RENT]
			FROM 
				#TMP_TREND 
			GROUP BY 
				REP_YEAR, REP_MONTH 
			ORDER BY 
				CONVERT(INT,REP_YEAR) ASC, CONVERT(INT,REP_MONTH) ASC
		END
	END
	ELSE
	BEGIN
		IF @grouping_criteria = 'HOUR'
		BEGIN
			SELECT 
				REP_YEAR , REP_MONTH , REP_WEEK_OF_YEAR, 
				CONVERT(VARCHAR(8),REP_DATE,108) AS [REP_DATE] ,
				100.0 AS [TOTAL_FLEET], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (CARSALES*100.0)/TOTAL_FLEET END AS [CARSALES], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (CU*100.0)/TOTAL_FLEET END AS [CU], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (HA*100.0)/TOTAL_FLEET END [HA], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (HL*100.0)/TOTAL_FLEET END AS [HL], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (LL*100.0)/TOTAL_FLEET END AS [LL], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (NC*100.0)/TOTAL_FLEET END AS [NC], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (PL*100.0)/TOTAL_FLEET END AS [PL], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (TC*100.0)/TOTAL_FLEET END AS [TC], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (SV*100.0)/TOTAL_FLEET END AS [SV], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (WS*100.0)/TOTAL_FLEET END AS [WS], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (OPERATIONAL_FLEET*100.0)/TOTAL_FLEET END AS [OPERATIONAL_FLEET], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (BD*100.0)/TOTAL_FLEET END AS [BD], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (MM*100.0)/TOTAL_FLEET END AS [MM], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (TW*100.0)/TOTAL_FLEET END AS [TW], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (TB*100.0)/TOTAL_FLEET END AS [TB], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (FS*100.0)/TOTAL_FLEET END AS [FS], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (RL*100.0)/TOTAL_FLEET END AS [RL], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (RP*100.0)/TOTAL_FLEET END AS [RP], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (TN*100.0)/TOTAL_FLEET END AS [TN], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (AVAILABLE_FLEET*100.0)/TOTAL_FLEET END AS [AVAILABLE_FLEET], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (RT*100.0)/TOTAL_FLEET END AS [RT], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (SU*100.0)/TOTAL_FLEET END AS [SU], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (GOLD*100.0)/TOTAL_FLEET END AS [GOLD], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (PREDELIVERY*100.0)/TOTAL_FLEET END AS [PREDELIVERY], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (OVERDUE*100.0)/TOTAL_FLEET END AS [OVERDUE], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (ON_RENT*100.0)/TOTAL_FLEET END AS [ON_RENT] 
			FROM 
				#TMP_TREND 
			ORDER BY 
				CONVERT(VARCHAR(8),REP_DATE,108)
		END
		ELSE IF @grouping_criteria = 'DAY'
		BEGIN
			SELECT 
				REP_YEAR , REP_MONTH , REP_WEEK_OF_YEAR, REP_DATE ,
				100.0 AS [TOTAL_FLEET], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (CARSALES*100.0)/TOTAL_FLEET END AS [CARSALES], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (CU*100.0)/TOTAL_FLEET END AS [CU], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (HA*100.0)/TOTAL_FLEET END [HA], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (HL*100.0)/TOTAL_FLEET END AS [HL], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (LL*100.0)/TOTAL_FLEET END AS [LL], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (NC*100.0)/TOTAL_FLEET END AS [NC], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (PL*100.0)/TOTAL_FLEET END AS [PL], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (TC*100.0)/TOTAL_FLEET END AS [TC], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (SV*100.0)/TOTAL_FLEET END AS [SV], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (WS*100.0)/TOTAL_FLEET END AS [WS], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (OPERATIONAL_FLEET*100.0)/TOTAL_FLEET END AS [OPERATIONAL_FLEET], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (BD*100.0)/TOTAL_FLEET END AS [BD], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (MM*100.0)/TOTAL_FLEET END AS [MM], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (TW*100.0)/TOTAL_FLEET END AS [TW], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (TB*100.0)/TOTAL_FLEET END AS [TB], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (FS*100.0)/TOTAL_FLEET END AS [FS], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (RL*100.0)/TOTAL_FLEET END AS [RL], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (RP*100.0)/TOTAL_FLEET END AS [RP], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (TN*100.0)/TOTAL_FLEET END AS [TN], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (AVAILABLE_FLEET*100.0)/TOTAL_FLEET END AS [AVAILABLE_FLEET], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (RT*100.0)/TOTAL_FLEET END AS [RT], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (SU*100.0)/TOTAL_FLEET END AS [SU], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (GOLD*100.0)/TOTAL_FLEET END AS [GOLD], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (PREDELIVERY*100.0)/TOTAL_FLEET END AS [PREDELIVERY], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (OVERDUE*100.0)/TOTAL_FLEET END AS [OVERDUE], 
				CASE WHEN (TOTAL_FLEET = 0) THEN 0 ELSE (ON_RENT*100.0)/TOTAL_FLEET END AS [ON_RENT] 
			FROM 
				#TMP_TREND 
			ORDER BY 
				REP_DATE
		END
		ELSE IF @grouping_criteria = 'WEEK'
		BEGIN
			SELECT 
				REP_YEAR, 
				SUBSTRING(CONVERT(VARCHAR(8),dim_calendar_id),5,2) AS [REP_MONTH], 
				DATENAME(m, STR(SUBSTRING(CONVERT(VARCHAR(8),dim_calendar_id),5,2)) + '/1/2012') + ' - week ' +
				REP_WEEK_OF_YEAR AS [REP_WEEK_OF_YEAR], 
				NULL AS [REP_DATE],
				100.0 AS [TOTAL_FLEET], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(CARSALES)*100.0)/AVG(TOTAL_FLEET) END AS [CARSALES], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(CU)*100.0)/AVG(TOTAL_FLEET) END AS [CU], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(HA)*100.0)/AVG(TOTAL_FLEET) END [HA], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(HL)*100.0)/AVG(TOTAL_FLEET) END AS [HL], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(LL)*100.0)/AVG(TOTAL_FLEET) END AS [LL], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(NC)*100.0)/AVG(TOTAL_FLEET) END AS [NC], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(PL)*100.0)/AVG(TOTAL_FLEET) END AS [PL], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(TC)*100.0)/AVG(TOTAL_FLEET) END AS [TC], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(SV)*100.0)/AVG(TOTAL_FLEET) END AS [SV], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(WS)*100.0)/AVG(TOTAL_FLEET) END AS [WS], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(OPERATIONAL_FLEET)*100.0)/AVG(TOTAL_FLEET) END AS [OPERATIONAL_FLEET], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(BD)*100.0)/AVG(TOTAL_FLEET) END AS [BD], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(MM)*100.0)/AVG(TOTAL_FLEET) END AS [MM], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(TW)*100.0)/AVG(TOTAL_FLEET) END AS [TW], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(TB)*100.0)/AVG(TOTAL_FLEET) END AS [TB], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(FS)*100.0)/AVG(TOTAL_FLEET) END AS [FS], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(RL)*100.0)/AVG(TOTAL_FLEET) END AS [RL], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(RP)*100.0)/AVG(TOTAL_FLEET) END AS [RP], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(TN)*100.0)/AVG(TOTAL_FLEET) END AS [TN], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(AVAILABLE_FLEET)*100.0)/AVG(TOTAL_FLEET) END AS [AVAILABLE_FLEET], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(RT)*100.0)/AVG(TOTAL_FLEET) END AS [RT], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(SU)*100.0)/AVG(TOTAL_FLEET) END AS [SU], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(GOLD)*100.0)/AVG(TOTAL_FLEET) END AS [GOLD], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(PREDELIVERY)*100.0)/AVG(TOTAL_FLEET) END AS [PREDELIVERY], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(OVERDUE)*100.0)/AVG(TOTAL_FLEET) END AS [OVERDUE], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(ON_RENT)*100.0)/AVG(TOTAL_FLEET) END AS [ON_RENT] 
			FROM 
				#TMP_TREND
			GROUP BY 
				REP_YEAR, REP_WEEK_OF_YEAR , SUBSTRING(CONVERT(VARCHAR(8),dim_calendar_id),5,2)
			ORDER BY 
				REP_YEAR, CONVERT(INT,REP_WEEK_OF_YEAR) ASC
		END
		ELSE IF @grouping_criteria = 'MONTH'
		BEGIN
			SELECT 
				REP_YEAR, 
				DATENAME(m, STR(REP_MONTH) + '/1/2012') + ' ' + SUBSTRING(REP_YEAR,3,2) AS [REP_MONTH],
				DATENAME(m, STR(REP_MONTH) + '/1/2012') + ' ' + SUBSTRING(REP_YEAR,3,2) AS [REP_WEEK_OF_YEAR] , 
				CONVERT(DATETIME,'01/' + REP_MONTH + '/' + REP_YEAR,103) AS [REP_DATE],
				100.0 AS [TOTAL_FLEET], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(CARSALES)*100.0)/AVG(TOTAL_FLEET) END AS [CARSALES], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(CU)*100.0)/AVG(TOTAL_FLEET) END AS [CU], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(HA)*100.0)/AVG(TOTAL_FLEET) END [HA], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(HL)*100.0)/AVG(TOTAL_FLEET) END AS [HL], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(LL)*100.0)/AVG(TOTAL_FLEET) END AS [LL], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(NC)*100.0)/AVG(TOTAL_FLEET) END AS [NC], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(PL)*100.0)/AVG(TOTAL_FLEET) END AS [PL], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(TC)*100.0)/AVG(TOTAL_FLEET) END AS [TC], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(SV)*100.0)/AVG(TOTAL_FLEET) END AS [SV], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(WS)*100.0)/AVG(TOTAL_FLEET) END AS [WS], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(OPERATIONAL_FLEET)*100.0)/AVG(TOTAL_FLEET) END AS [OPERATIONAL_FLEET], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(BD)*100.0)/AVG(TOTAL_FLEET) END AS [BD], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(MM)*100.0)/AVG(TOTAL_FLEET) END AS [MM], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(TW)*100.0)/AVG(TOTAL_FLEET) END AS [TW], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(TB)*100.0)/AVG(TOTAL_FLEET) END AS [TB], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(FS)*100.0)/AVG(TOTAL_FLEET) END AS [FS], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(RL)*100.0)/AVG(TOTAL_FLEET) END AS [RL], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(RP)*100.0)/AVG(TOTAL_FLEET) END AS [RP], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(TN)*100.0)/AVG(TOTAL_FLEET) END AS [TN], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(AVAILABLE_FLEET)*100.0)/AVG(TOTAL_FLEET) END AS [AVAILABLE_FLEET], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(RT)*100.0)/AVG(TOTAL_FLEET) END AS [RT], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(SU)*100.0)/AVG(TOTAL_FLEET) END AS [SU], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(GOLD)*100.0)/AVG(TOTAL_FLEET) END AS [GOLD], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(PREDELIVERY)*100.0)/AVG(TOTAL_FLEET) END AS [PREDELIVERY], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(OVERDUE)*100.0)/AVG(TOTAL_FLEET) END AS [OVERDUE], 
				CASE WHEN (AVG(TOTAL_FLEET) = 0) THEN 0 ELSE (AVG(ON_RENT)*100.0)/AVG(TOTAL_FLEET) END AS [ON_RENT] 
			FROM 
				#TMP_TREND
			GROUP BY 
				REP_YEAR, REP_MONTH 
			ORDER BY 
				CONVERT(INT,REP_YEAR) ASC, CONVERT(INT,REP_MONTH) ASC
		END
	END


-- Drop the table
--=====================================================================================
	
	
	
	SELECT @rowcount = @@rowcount	

	DROP TABLE #TMP_TREND

	--INSERT Trace (entity, key1, key2, data1)
	--SELECT @entity, @key1, 'end', 'returned=' + coalesce(convert(varchar(20),@rowcount),'null')
	--							+ ',NumDays=' + coalesce(convert(varchar(10),@numdays),'null')
 
END TRY
BEGIN CATCH
	
	DECLARE @ErrDesc VARCHAR(MAX)
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