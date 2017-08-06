CREATE procedure [dbo].[spReportFleetStatus] 
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
	@userName				varchar(50) = NULL
AS
/*
exec spReportFleetStatus
	@start_date = '20110320' ,
	@end_date = '20120325' ,
	@country = 'GE'
exec spReportFleetStatus
	@start_date = '20110320' ,
	@end_date = '20120325' ,
	@country = 'GE' ,
	@day_of_week = 5
*/
		
		SET NOCOUNT ON;
	begin try
	/*
	declare
		@country				VARCHAR(10) = 'GE',
		@cms_pool_id			INT			= NULL,
		@cms_location_group_id	int			= NULL, 
		@ops_region_id			INT			= NULL,
		@ops_area_id			INT			= NULL,	
		@location				VARCHAR(50) = NULL,	
		@fleet_name				VARCHAR(50) = NULL,
		@car_segment_id			INT			= NULL,
		@car_class_id			INT			= NULL,
		@car_group_id			INT			= NULL,	
		@start_date				DATETIME	='20110618'	  ,
		@end_date				DATETIME	='20120618'	  ,
		@day_of_week			INT			= NULL
	*/
	
	-- Added by Javi 07/Aug/2012
	--------------------------------------
	declare @counter int
	declare @dailyDate table (dailyDate datetime)
	insert into @dailyDate (dailyDate)
	select z_inserted from bcs.FileProcess where Entity = 'Mars_Daily' and z_inserted between @start_date and @end_date
	set @counter = (select COUNT(*) from @dailyDate)
	if @counter = 0 set @start_date = @start_date - 1
	
	
	
	DECLARE @rowcount int, @inserted int, @updated int
	DECLARE @entity VARCHAR(100)
	DECLARE @key1	VARCHAR(100)
	DECLARE @data2	VARCHAR(max)
		
	select @data2	=	'@country=' + coalesce(@country,'null')
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

		select @entity = OBJECT_NAME(@@PROCID)
		select @entity = coalesce(@entity,'test')
		select @key1 = convert(varchar(20),@@spid)
		--insert trace (entity, key1, key2, data2)
		--select @entity, @key1, 'start', @data2

		-- get start and end months for month table access
		declare @StartFullPeriod datetime
		declare @EndFullPeriod datetime
		declare @StartDateEnd datetime
		declare @EndDateStart datetime

		-- get full periods at aggregation level
		declare @periods table 
			(dim_calendar_id int, dim_calendar_id_end int, PeriodStart datetime, PeriodEnd datetime, type int)
		
		if @day_of_week is null
		begin
			declare @type int
			select @type = 5
			while @type > 1
			begin
				select @type = @type - 1

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
				and		m.Type = @Type
				
				select	@StartFullPeriod = MIN(PeriodStart) from @periods		
				select	@EndFullPeriod = max(PeriodEnd) from @periods	

				select @data2	=	'@Type=' + coalesce(convert(varchar(20),@Type),'null')
									+ ',@StartFullPeriod=' + '''' + coalesce(convert(varchar(19),@StartFullPeriod,126)+'''','null')
									+ ',@EndFullPeriod=' + '''' + coalesce(convert(varchar(19),@EndFullPeriod,126)+'''','null')
				--insert trace (entity, key1, key2, data2)
				--select @entity, @key1, 'Aggregate', @data2
			end
		end
		else
		begin
			insert	@periods
			select	m.min_dim_calendar_id, m.max_dim_calendar_id, m.MonthStart, m.MonthEnd, 999
			from	inp.MonthlyData m
			where	m.MonthStart >= @start_date
			and		m.MonthEnd <= @end_date

		end
		
		select	@StartFullPeriod = min(PeriodStart) ,
				@EndFullPeriod = max(PeriodEnd)
		from @periods
		
		if @StartFullPeriod > @EndFullPeriod or @StartFullPeriod is null or @EndFullPeriod is null
			select	@StartDateEnd = @end_date ,
					@EndDateStart = null
		else
			select	@StartDateEnd = @StartFullPeriod - 1 ,
					@EndDateStart = @EndFullPeriod + 1
		
		--insert trace (entity, key1, Key2, Data1)
		--select @entity, @key1, 'Periods',	'start=' + coalesce(convert(varchar(19),@start_date,126),'null')
		--					+	',@StartDateEnd=' + coalesce(convert(varchar(19),@StartDateEnd,126),'null')
		--					+	',@EndDateStart=' + coalesce(convert(varchar(19),@EndDateStart,126),'null')
		--					+ 	',end=' + coalesce(convert(varchar(19),@end_date,126),'null')
		--					+ 	',StartPeriod=' + coalesce(convert(varchar(19),@StartFullPeriod,126),'null')
		--					+ 	',EndPeriod=' + coalesce(convert(varchar(19),@EndFullPeriod,126),'null')
		
		-- locations
		declare @LocationFlag int = 0
		select @LocationFlag = 1
		where coalesce(@cms_pool_id, @cms_location_group_id, @ops_region_id, @ops_area_id, case when @location is not null then 1 else null end) is not null
		
		declare @Locations table (dim_Location_id int)
		if @LocationFlag = 1
			insert @Locations
			select dim_Location_id
			from inp.dim_Location FES
			where ((FES.LOCATION IN (SELECT L.location	FROM LOCATIONS AS L INNER JOIN CMS_LOCATION_GROUPS AS CLG ON L.cms_location_group_id = CLG.cms_location_group_id WHERE CLG.cms_pool_id = @cms_pool_id)) OR @cms_pool_id IS NULL) -- CMS_POOLS
			AND ((FES.LOCATION IN (SELECT location		FROM LOCATIONS WHERE cms_location_group_id = @cms_location_group_id)) OR @cms_location_group_id IS NULL) -- CMS_LOCATION_GROUPS
			AND ((FES.LOCATION IN (SELECT L.location	FROM LOCATIONS AS L INNER JOIN OPS_AREAS AS OA ON L.ops_area_id = OA.ops_area_id WHERE OA.ops_region_id = @ops_region_id)) OR @ops_region_id IS NULL) -- OPS_REGIONS
			AND ((FES.LOCATION IN (SELECT location		FROM LOCATIONS WHERE ops_area_id = @ops_area_id)) OR @ops_area_id IS NULL) -- OPS_AREAS
			AND (FES.LOCATION = @location OR @location IS NULL) -- Location
		else
			insert @Locations select distinct dim_Location_id from inp.dim_Location
		
		-- Car groups
		declare @CarGroupFlag int = 0
		select @CarGroupFlag = 1
		where coalesce(@car_segment_id, @car_class_id, @car_group_id) is not null

		declare @CarGroups table (CarGroup varchar(10))
		if @CarGroupFlag = 1
			insert @CarGroups
			select distinct CAR_GROUP
			from CAR_GROUPS FES
			where ((FES.CAR_GROUP IN (SELECT CG.car_group FROM CAR_GROUPS AS CG INNER JOIN CAR_CLASSES AS CC ON CG.car_class_id = CC.car_class_id WHERE CC.car_segment_id = @car_segment_id)) OR @car_segment_id IS NULL) --@car_segment_id
			AND ((FES.CAR_GROUP IN (SELECT CG.car_group FROM CAR_GROUPS AS CG WHERE CG.car_class_id = @car_class_id)) OR @car_class_id IS NULL) --@car_class_id
			AND ((FES.CAR_GROUP = (SELECT car_group		FROM CAR_GROUPS WHERE car_group_id = @car_group_id)) OR @car_group_id IS NULL) --@car_group_id
		else
			insert @CarGroups select null

		-- Get day entries to start of first full month
		CREATE TABLE #TMP_TREND
		(
			dim_calendar_id			int,
			NumDays				int ,			
			rowtype				int ,
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

		-- Get entries before and after full periods
		INSERT INTO #TMP_TREND
		(
			dim_calendar_id, NumDays , rowtype,
			TOTAL_FLEET, CARSALES, CU, HA, HL, LL, NC, PL, TC, 
			SV, WS, OPERATIONAL_FLEET, BD, MM, TW, TB, FS, RL, 
			RP, TN, AVAILABLE_FLEET, RT, SU, GOLD, PREDELIVERY, 
			OVERDUE, ON_RENT
		)		

		SELECT 
				FES.dim_calendar_id, 
				NumDays = 1 ,
				rowtype = 0 ,
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
				inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY FES
					join inp.dim_calendar d
						on d.dim_calendar_id = FES.dim_calendar_id
						and	(	d.Rep_DayOfWeek = @day_of_week 
							or	@day_of_week IS NULL
							)
					join @Locations l
						on l.dim_Location_id = FES.dim_Location_id
					join @CarGroups cg
						on cg.CarGroup = FES.Car_Group
						or cg.CarGroup is null
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
							) AND
								NOT(FLEET_ADV = 1)
								AND
								NOT(FLEET_HOD = 1)
								AND
								NOT(FLEET_LICENSEE = 1)
						)
					)--Fleet
			GROUP BY 
				FES.dim_calendar_id
		
		select @rowcount = @@rowcount
		--insert trace (entity, key1, key2, data1)
		--select @entity, @key1, 'Insert Days', 'updated=' + coalesce(convert(varchar(20),@rowcount),'null')
				
		-- get full periods
		if @day_of_week is null
		begin
			INSERT INTO #TMP_TREND
			(
				dim_calendar_id, NumDays , rowtype,
				TOTAL_FLEET, CARSALES, CU, HA, HL, LL, NC, PL, TC, 
				SV, WS, OPERATIONAL_FLEET, BD, MM, TW, TB, FS, RL, 
				RP, TN, AVAILABLE_FLEET, RT, SU, GOLD, PREDELIVERY, 
				OVERDUE, ON_RENT
			)		

			SELECT 
					FES.dim_Calendar_id_start, 
					NumDays = max(NumDays) ,
					rowtype = FES.Type ,
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
					inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY_Aggregate AS FES
						join @periods m
							on FES.dim_Calendar_id_start = m.dim_calendar_id
							and m.type = FES.Type
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
							) AND
								NOT(FLEET_ADV = 1)
								AND
								NOT(FLEET_HOD = 1)
								AND
								NOT(FLEET_LICENSEE = 1)
						)
					)--Fleet
				GROUP BY 
					FES.dim_Calendar_id_start, FES.Type
				
				select @rowcount = @@rowcount
				--insert trace (entity, key1, key2, data1)
				--select @entity, @key1, 'Insert 4 weeks', 'updated=' + coalesce(convert(varchar(20),@rowcount),'null')
		end
		else
		begin
			INSERT INTO #TMP_TREND
			(
				dim_calendar_id, NumDays , rowtype,
				TOTAL_FLEET, CARSALES, CU, HA, HL, LL, NC, PL, TC, 
				SV, WS, OPERATIONAL_FLEET, BD, MM, TW, TB, FS, RL, 
				RP, TN, AVAILABLE_FLEET, RT, SU, GOLD, PREDELIVERY, 
				OVERDUE, ON_RENT
			)		
			
			SELECT 
					FES.dim_calendar_id, 
					NumDays = max(NumDays) ,
					rowtype = 1 ,
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
							) AND
								NOT(FLEET_ADV = 1)
								AND
								NOT(FLEET_HOD = 1)
								AND
								NOT(FLEET_LICENSEE = 1)
						)
					)--Fleet
					AND FES.Rep_DayOfWeek = @day_of_week
				GROUP BY 
					FES.dim_calendar_id

				select @rowcount = @@rowcount
				--insert trace (entity, key1, key2, data1)
				--select @entity, @key1, 'DOW', 'updated=' + coalesce(convert(varchar(20),@rowcount),'null')

		end
		
		declare @numdays float
		
		;with cte1 as
		(
				select numdays = max(NumDays) from #TMP_TREND group by dim_calendar_id
		) ,
		cte as
		(
				select numdays = 1.0*sum(numdays)  from cte1
		)
		select @numdays = numdays from cte

		SELECT 
			--sum(CARSALES)	AS 'Test', 
			convert(numeric(12,0),sum(TOTAL_FLEET)/@numdays)	AS 'TOTAL_FLEET', 
			convert(numeric(12,0),sum(CARSALES)/@numdays)		AS 'CARSALES', 
			convert(numeric(12,0),sum(CU)/@numdays) AS 'CU', 
			convert(numeric(12,0),sum(HA)/@numdays) AS 'HA', 
			convert(numeric(12,0),sum(HL)/@numdays) AS 'HL', 
			convert(numeric(12,0),sum(LL)/@numdays) AS 'LL', 
			convert(numeric(12,0),sum(NC)/@numdays) AS 'NC', 
			convert(numeric(12,0),sum(PL)/@numdays) AS 'PL', 
			convert(numeric(12,0),sum(TC)/@numdays) AS 'TC', 
			convert(numeric(12,0),sum(SV)/@numdays) AS 'SV', 
			convert(numeric(12,0),sum(WS)/@numdays) AS 'WS', 
			convert(numeric(12,0),sum(OPERATIONAL_FLEET)/@numdays) AS 'OPERATIONAL_FLEET', 
			convert(numeric(12,0),sum(BD)/@numdays) AS 'BD', 
			convert(numeric(12,0),sum(MM)/@numdays) AS 'MM', 
			convert(numeric(12,0),sum(TW)/@numdays) AS 'TW', 
			convert(numeric(12,0),sum(TB)/@numdays) AS 'TB', 
			convert(numeric(12,0),sum(FS)/@numdays) AS 'FS', 
			convert(numeric(12,0),sum(RL)/@numdays) AS 'RL', 
			convert(numeric(12,0),sum(RP)/@numdays) AS 'RP', 
			convert(numeric(12,0),sum(TN)/@numdays) AS 'TN', 
			convert(numeric(12,0),sum(AVAILABLE_FLEET)/@numdays) AS 'AVAILABLE_FLEET', 
			convert(numeric(12,0),sum(RT)/@numdays)		AS 'RT', 
			convert(numeric(12,0),sum(SU)/@numdays)		AS 'SU', 
			convert(numeric(12,0),sum(GOLD)/@numdays)	AS 'GOLD', 
			convert(numeric(12,0),sum(PREDELIVERY)/@numdays) AS 'PREDELIVERY', 
			convert(numeric(12,0),sum(OVERDUE)/@numdays) AS 'OVERDUE', 
			convert(numeric(12,0),sum(ON_RENT)/@numdays) AS 'ON_RENT' 
		FROM 
			#TMP_TREND
		select @rowcount = @@rowcount	

		-- Drop the tempory table	
		DROP TABLE #TMP_TREND

		--insert trace (entity, key1, key2, key3, data1)
		--select @entity, @key1, 'end', @userName, 'returned=' + coalesce(convert(varchar(20),@rowcount),'null')
		--								+ ',NumDays=' + coalesce(convert(varchar(10),@numdays),'null')
	end try
	begin catch
	declare @ErrDesc varchar(max)
	declare @ErrProc varchar(128)
	declare @ErrLine varchar(20)

		select 	@ErrProc = ERROR_PROCEDURE() ,
			@ErrLine = ERROR_LINE() ,
			@ErrDesc = ERROR_MESSAGE()
		select	@ErrProc = coalesce(@ErrProc,'') ,
			@ErrLine = coalesce(@ErrLine,'') ,
			@ErrDesc = coalesce(@ErrDesc,'')
		
		insert	Trace (Entity, key1, data1, data2)
		select	Entity = @entity, key1 = 'Failure',
			data1 = '<ErrProc=' + @ErrProc + '>'
				+ '<ErrLine=' + @ErrLine + '>'
				+ '<ErrDesc=' + @ErrDesc + '>',
			data2 = @key1
		raiserror('Failed %s', 16, -1, @ErrDesc)
	end catch