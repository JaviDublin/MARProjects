
CREATE proc [dbo].[s_Populate_Fact_FLEET_EUROPE_SUMMARY_HISTORY_WeekOfYear]
@StartDate datetime = null ,
@endDate datetime = null
as
begin try
/*
exec s_Populate_Fact_FLEET_EUROPE_SUMMARY_HISTORY_WeekOfYear
	@StartDate = '20110102' ,
	@endDate = '20120624'
begin tran
exec s_Populate_Fact_FLEET_EUROPE_SUMMARY_HISTORY_WeekOfYear
	@StartDate = '20110102' ,
	@endDate = '20110102'
select top 20 * from trace order by trace_id desc
rollback tran

*/

declare @rowcount int, @inserted int, @updated int
declare @DeletedDate datetime
declare @entity varchar(100)
declare @key1 varchar(100)
declare @data2 varchar(max)
	
	select @entity = OBJECT_NAME(@@PROCID)
	select @entity = coalesce(@entity,'test')

	if @StartDate is null
	begin
		select @StartDate = convert(varchar(8),DATEADD(mm,-1,getdate()),112)

		select @StartDate = d.FirstDayOfMonth
		from Inp.dim_Calendar d
		where d.Rep_Date = @StartDate

		select @StartDate = DATEADD(wk,-1,@StartDate)
	end
	if @endDate is null
	begin
		select @endDate = convert(varchar(8),DATEADD(wk,-1,getdate()),112)
	
		select @endDate = d.LastDayOfMonth
		from Inp.dim_Calendar d
		where d.Rep_Date = @endDate

		select @endDate = DATEADD(wk,-1,@endDate)
	end
	
	select @StartDate = d.FirstDayOfWeek
	from Inp.dim_Calendar d
	where d.Rep_Date = @StartDate
	
	select @endDate = d.LastDayOfWeek
	from Inp.dim_Calendar d
	where d.Rep_Date = @endDate
	
	insert trace (Entity, key1, data1) select @entity, 'start', '@StartDate=' + coalesce(''''+convert(varchar(23),@StartDate,126)+'''', 'null')
													+ ',@endDate=' + coalesce(''''+convert(varchar(23),@endDate,126)+'''', 'null')
	
declare @i1 int, @i2 int
declare @d1 datetime, @d2 datetime
declare @Year int, @Week int

	select @i1 = MIN(dim_calendar_id) from inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY 
	select @i2 = Max(dim_calendar_id) from inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY

	-- get full week periods
	select @i1 = MIN(dim_calendar_id)
	from inp.dim_Calendar
	where Rep_Year * 1000 + Rep_WeekOfYear > (select Rep_Year * 1000 + Rep_WeekOfYear from inp.dim_Calendar where dim_calendar_id = @i1)

	select @i2 = max(dim_calendar_id)
	from inp.dim_Calendar
	where Rep_Year * 1000 + Rep_WeekOfYear < (select Rep_Year * 1000 + Rep_WeekOfYear from inp.dim_Calendar where dim_calendar_id = @i2)

	select @d1 = min(Rep_Date) from inp.dim_Calendar
	where Rep_Year =  (select Rep_Year from inp.dim_Calendar where dim_calendar_id = @i1)
	and Rep_WeekOfYear = (select Rep_WeekOfYear from inp.dim_Calendar where dim_calendar_id = @i1)

	select @d2 = max(Rep_Date) from inp.dim_Calendar
	where Rep_Year = (select Rep_Year from inp.dim_Calendar where dim_calendar_id = @i2)
	and Rep_WeekOfYear = (select Rep_WeekOfYear from inp.dim_Calendar where dim_calendar_id = @i2)

	if @d1 < @StartDate
		select @d1 = @StartDate
	if @d2 > @endDate
		select @d2 = @endDate


	select 
		dim_Calendar_id = @i1,
		Rep_Year = @Year ,
		WeekOfYear = @Week ,
		NumDays = COUNT(distinct FES.dim_calendar_id) ,
		COUNTRY ,
		car_group ,
		dim_Location_id ,		-- Location_id
		[FLEET_CARSALES] ,
		[FLEET_RAC_TTL] ,
		[FLEET_RAC_OPS] ,
		FLEET_LICENSEE ,
		
		TOTAL_FLEET 				=SUM(FES.TOTAL_FLEET), 
		CARSALES 					=SUM(FES.CARSALES), 
		CARHOLD_H 					=SUM(FES.CARHOLD_H), 
		CARHOLD_L 					=SUM(FES.CARHOLD_L), 
		CU 							=SUM(FES.CU), 
		HA 							=SUM(FES.HA), 
		HL 							=SUM(FES.HL), 
		LL 							=SUM(FES.LL), 
		NC 							=SUM(FES.NC), 
		PL 							=SUM(FES.PL), 
		TC 							=SUM(FES.TC), 
		SV 							=SUM(FES.SV), 
		WS 							=SUM(FES.WS), 
		WS_NONRAC					=SUM(FES.WS_NONRAC), 
		OPERATIONAL_FLEET 			=SUM(FES.OPERATIONAL_FLEET), 
		BD 							=SUM(FES.BD), 
		MM 							=SUM(FES.MM), 
		TW 							=SUM(FES.TW), 
		TB 							=SUM(FES.TB), 
		WS_RAC 						=SUM(FES.WS_RAC), 
		AVAILABLE_TB 				=SUM(FES.AVAILABLE_TB), 
		FS 							=SUM(FES.FS), 
		RL 							=SUM(FES.RL), 
		RP 							=SUM(FES.RP), 
		TN 							=SUM(FES.TN), 
		AVAILABLE_FLEET 			=SUM(FES.AVAILABLE_FLEET), 
		RT 							=SUM(FES.RT), 
		SU 							=SUM(FES.SU), 
		GOLD 						=SUM(FES.GOLD), 
		PREDELIVERY 				=SUM(FES.PREDELIVERY), 
		OVERDUE 					=SUM(FES.OVERDUE), 
		ON_RENT 					=SUM(FES.ON_RENT),
		RT_MIN_SUM					= sum(RT_MIN) ,
		AVAILABLE_FLEET_MIN_SUM		= sum(AVAILABLE_FLEET_MIN) ,
		RT_MAX_SUM					= sum(RT_MAX) ,
		AVAILABLE_FLEET_MAX_SUM		= sum(AVAILABLE_FLEET_MAX)
	into #Fact_FLEET_EUROPE_SUMMARY_HISTORY_WeekOfYear
	from inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY FES
	where 1=0
	group by COUNTRY, dim_Location_id, car_group, FLEET_CARSALES, FLEET_RAC_TTL, FLEET_RAC_OPS, FLEET_LICENSEE

	while @d1 < @d2
	begin

		select	@Year = max(Rep_Year) ,
				@Week = max(Rep_WeekOfYear) ,
				@i1 = MIN(dim_calendar_id) ,
				@i2 = MAX(dim_calendar_id)
		from inp.dim_Calendar
		where Rep_Year = (select Rep_Year from inp.dim_Calendar where Rep_Date = @d1)
		and Rep_WeekOfYear = (select Rep_WeekOfYear from inp.dim_Calendar where Rep_Date = @d1)
	
		truncate table #Fact_FLEET_EUROPE_SUMMARY_HISTORY_WeekOfYear
		
		insert #Fact_FLEET_EUROPE_SUMMARY_HISTORY_WeekOfYear
		select
			dim_Calendar_id = @i1,
			Rep_Year = @Year ,
			WeekOfYear = @Week ,
			NumDays = COUNT(distinct FES.dim_calendar_id) ,
			COUNTRY ,
			car_group ,
			dim_Location_id ,		-- Location_id
			[FLEET_CARSALES] ,
			[FLEET_RAC_TTL] ,
			[FLEET_RAC_OPS] ,
			FLEET_LICENSEE ,
			
			TOTAL_FLEET 				=SUM(FES.TOTAL_FLEET), 
			CARSALES 					=SUM(FES.CARSALES), 
			CARHOLD_H 					=SUM(FES.CARHOLD_H), 
			CARHOLD_L 					=SUM(FES.CARHOLD_L), 
			CU 							=SUM(FES.CU), 
			HA 							=SUM(FES.HA), 
			HL 							=SUM(FES.HL), 
			LL 							=SUM(FES.LL), 
			NC 							=SUM(FES.NC), 
			PL 							=SUM(FES.PL), 
			TC 							=SUM(FES.TC), 
			SV 							=SUM(FES.SV), 
			WS 							=SUM(FES.WS), 
			WS_NONRAC					=SUM(FES.WS_NONRAC), 
			OPERATIONAL_FLEET 			=SUM(FES.OPERATIONAL_FLEET), 
			BD 							=SUM(FES.BD), 
			MM 							=SUM(FES.MM), 
			TW 							=SUM(FES.TW), 
			TB 							=SUM(FES.TB), 
			WS_RAC 						=SUM(FES.WS_RAC), 
			AVAILABLE_TB 				=SUM(FES.AVAILABLE_TB), 
			FS 							=SUM(FES.FS), 
			RL 							=SUM(FES.RL), 
			RP 							=SUM(FES.RP), 
			TN 							=SUM(FES.TN), 
			AVAILABLE_FLEET 			=SUM(FES.AVAILABLE_FLEET), 
			RT 							=SUM(FES.RT), 
			SU 							=SUM(FES.SU), 
			GOLD 						=SUM(FES.GOLD), 
			PREDELIVERY 				=SUM(FES.PREDELIVERY), 
			OVERDUE 					=SUM(FES.OVERDUE), 
			ON_RENT 					=SUM(FES.ON_RENT),
			RT_MIN_SUM					= sum(RT_MIN) ,
			AVAILABLE_FLEET_MIN_SUM		= sum(AVAILABLE_FLEET_MIN) ,
			RT_MAX_SUM					= sum(RT_MAX) ,
			AVAILABLE_FLEET_MAX_SUM		= sum(AVAILABLE_FLEET_MAX)
		from inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY FES
		where dim_calendar_id between @i1 and @i2
		group by 
			COUNTRY, dim_Location_id, car_group, FLEET_CARSALES, FLEET_RAC_TTL, FLEET_RAC_OPS, 
			FLEET_LICENSEE
		
		select @rowcount = @@ROWCOUNT
		insert trace (Entity, key1, key2, data1) 
		select @entity, 'temp', CONVERT(varchar(8),@d1,112), 'inserted='+convert(varchar(20),@rowcount)

		begin tran
			-- delete any existing data
			delete inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY_WeekOfYear
			from inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY_WeekOfYear f
			where f.dim_calendar_id between @i1 and @i2
				
			-- do not insert into trace inside a transaction
			select @updated = @@ROWCOUNT, @DeletedDate = GETDATE()
			
			-- insert new data
			insert inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY_WeekOfYear
				(
				  dim_Calendar_id
				, Rep_Year
				, WeekOfYear
				, NumDays
				, COUNTRY
				, car_group
				, dim_Location_id
				, FLEET_CARSALES
				, FLEET_RAC_TTL
				, FLEET_RAC_OPS
				, FLEET_LICENSEE
				
				, TOTAL_FLEET
				, CARSALES
				, CARHOLD_H
				, CARHOLD_L
				, CU
				, HA
				, HL
				, LL
				, NC
				, PL
				, TC
				, SV
				, WS
				, WS_NONRAC
				, OPERATIONAL_FLEET
				, BD
				, MM
				, TW
				, TB
				, WS_RAC
				, AVAILABLE_TB
				, FS
				, RL
				, RP
				, TN
				, AVAILABLE_FLEET
				, RT
				, SU
				, GOLD
				, PREDELIVERY
				, OVERDUE
				, ON_RENT
				, RT_MIN_SUM
				, AVAILABLE_FLEET_MIN_SUM
				, RT_MAX_SUM
				, AVAILABLE_FLEET_MAX_SUM
				)
			select
				  dim_Calendar_id
				, Rep_Year
				, WeekOfYear
				, NumDays
				, COUNTRY
				, car_group
				, dim_Location_id
				, FLEET_CARSALES
				, FLEET_RAC_TTL
				, FLEET_RAC_OPS
				, FLEET_LICENSEE
				
				, TOTAL_FLEET
				, CARSALES
				, CARHOLD_H
				, CARHOLD_L
				, CU
				, HA
				, HL
				, LL
				, NC
				, PL
				, TC
				, SV
				, WS
				, WS_NONRAC
				, OPERATIONAL_FLEET
				, BD
				, MM
				, TW
				, TB
				, WS_RAC
				, AVAILABLE_TB
				, FS
				, RL
				, RP
				, TN
				, AVAILABLE_FLEET
				, RT
				, SU
				, GOLD
				, PREDELIVERY
				, OVERDUE
				, ON_RENT
				, RT_MIN_SUM
				, AVAILABLE_FLEET_MIN_SUM
				, RT_MAX_SUM
				, AVAILABLE_FLEET_MAX_SUM
			from #Fact_FLEET_EUROPE_SUMMARY_HISTORY_WeekOfYear
			
			select @rowcount = @@ROWCOUNT
		
		commit tran

		insert trace (Entity, key1, key2, data1) 
		select @entity, 'pop', CONVERT(varchar(8),@d1,112), 'inserted='+convert(varchar(20),@rowcount)
																+ ', deleted=' + convert(varchar(20),@updated)
																+ ', @DeletedDate=' + coalesce(''''+convert(varchar(23),@DeletedDate,126)+'''', 'null')

		select @d1 = rep_date+1 from inp.dim_calendar where dim_Calendar_id = @i2
	end
	insert trace (Entity, key1) select @entity, 'end'

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