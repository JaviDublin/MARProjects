
create proc s_Populate_Fact_FLEET_EUROPE_SUMMARY_HISTORY_Aggregate_Week
@StartDate datetime = null ,
@endDate datetime = null ,
@debug		int = 0
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

exec s_Populate_Fact_FLEET_EUROPE_SUMMARY_HISTORY_Aggregate_Week @debug = 1
select top 10 * from trace order by trace_id desc
*/

declare @rowcount int, @inserted int, @updated int
declare @DeletedDate datetime
declare @entity varchar(100)
declare @key1 varchar(100)
declare @data2 varchar(max)
	
	select @entity = OBJECT_NAME(@@PROCID)
	select @entity = coalesce(@entity,'test')

	insert trace (Entity, key1, data1) select @entity, 'start', '@StartDate=' + coalesce(''''+convert(varchar(23),@StartDate,126)+'''', 'null')
													+ ',@endDate=' + coalesce(''''+convert(varchar(23),@endDate,126)+'''', 'null')
	
declare @i1 int, @i2 int
declare @d1 datetime, @d2 datetime
declare @Year int, @Week int
declare @dim_Calendar_id_start int
declare @dim_Calendar_id_end int

	select @StartDate = convert(varchar(8),@StartDate,112)
	select @endDate = convert(varchar(8),@endDate,112)
	
	if @StartDate is null
	begin
		-- get second last date from table
		;with cte as
		(	select min_dim_Calendar_id, seq = ROW_NUMBER() over (order by min_dim_Calendar_id desc)
			from Inp.Data_Aggregate
			where Type = 1
		)
		select @dim_Calendar_id_start = min_dim_Calendar_id
		from cte
		where seq = 2
		
		if @dim_Calendar_id_start is null
		begin
			select @dim_Calendar_id_start = min(dim_Calendar_id)
			from inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY
		end
		
		select	@StartDate = rep_date
		from	inp.dim_Calendar
		where	dim_Calendar_id = @dim_Calendar_id_start
	end
	
	if @endDate is null
	begin
		select	@dim_Calendar_id_end = convert(varchar(8),GETDATE()-1,112) ,
				@endDate = convert(varchar(8),GETDATE()-1,112)
	end
	
	-- align end date with source table
	select @i1 = MAX(dim_calendar_id)
	from inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY
	
	select @d2 = FirstDayOfWeek-1		-- end of last full week in table
	from inp.dim_Calendar
	where dim_Calendar_id = @i1
	
	if @d2 < @endDate
	begin
		select	@endDate = @d2 ,
				@dim_Calendar_id_end = convert(varchar(8),@d2,112)
	end
	
	-- Align dates with calendar weeks
	select @StartDate = FirstDayOfWeek
	from inp.dim_Calendar
	where Rep_Date = @StartDate
	
	select @endDate = LastDayOfWeek
	from inp.dim_Calendar
	where Rep_Date = @endDate
	
	select @dim_Calendar_id_start = dim_Calendar_id
	from inp.dim_Calendar
	where Rep_Date = @StartDate
	
	select @dim_Calendar_id_end = dim_Calendar_id
	from inp.dim_Calendar
	where Rep_Date = @endDate
	
	insert trace (Entity, key1, data1) select @entity, 'values',
													'@StartDate=' + coalesce(''''+convert(varchar(23),@StartDate,126)+'''', 'null')
													+ ',@endDate=' + coalesce(''''+convert(varchar(23),@endDate,126)+'''', 'null')
													+ ',@dim_Calendar_id_start=' + convert(varchar(20),@dim_Calendar_id_start)
													+ ',@dim_Calendar_id_end=' + convert(varchar(20),@dim_Calendar_id_end)

	if @debug = 1
		return	

	CREATE TABLE #Fact_FLEET_EUROPE_SUMMARY_HISTORY_Aggregate
		(
		dim_Calendar_id_start [int] not NULL,
		dim_Calendar_id_end [int] not NULL,
		Type int not null ,						-- 1 = 4 week
		NumDays int not null ,
		[COUNTRY] [varchar](10) not null ,
		car_group varchar(10) null ,
		[dim_Location_id] [int] not NULL,
		[FLEET_CARSALES] [bit] not NULL,
		[FLEET_RAC_TTL] [bit] not NULL,
		[FLEET_RAC_OPS] [bit] not NULL ,
		FLEET_LICENSEE bit not null ,
		
		[TOTAL_FLEET] int not NULL,
		[CARSALES] int not NULL,
		CARHOLD_H int not NULL,
		CARHOLD_L int not NULL,
		[CU] int not NULL,
		[HA] int not NULL,
		[HL] int not NULL,
		[LL] int not NULL,
		[NC] int not NULL,
		[PL] int not NULL,
		[TC] int not NULL,
		[SV] int not NULL,
		[WS] int not NULL,
		WS_NONRAC int not null ,	
		[OPERATIONAL_FLEET] int not NULL,
		[BD] int not NULL,
		[MM] int not NULL,
		[TW] int not NULL,
		[TB] int not NULL,
		WS_RAC int not null ,
		AVAILABLE_TB int not null ,
		[FS] int not NULL,
		[RL] int not NULL,
		[RP] int not NULL,
		[TN] int not NULL,
		[AVAILABLE_FLEET] int not NULL,
		[RT] int not NULL,
		[SU] int not NULL,
		[GOLD] int not NULL,
		[PREDELIVERY] int not NULL,
		[OVERDUE] int not NULL,
		[ON_RENT] int not NULL,

		RT_MIN_SUM int not null ,
		AVAILABLE_FLEET_MIN_SUM int not null ,

		RT_MAX_SUM int not null ,
		AVAILABLE_FLEET_MAX_SUM int not null
		)

	select @d1 = @StartDate
	
	select @d2 = LastDayOfWeek
	from Inp.dim_Calendar
	where Rep_Date = @d1
	
	while @d2 <= @endDate
	begin
		select	@i1 = dim_Calendar_id
		from Inp.dim_Calendar
		where Rep_Date = @d1

		select @i2 = dim_Calendar_id
		from Inp.dim_Calendar
		where Rep_Date = @d2
		
		truncate table #Fact_FLEET_EUROPE_SUMMARY_HISTORY_Aggregate
		
		insert #Fact_FLEET_EUROPE_SUMMARY_HISTORY_Aggregate
		select
			dim_Calendar_id_start = @i1 ,
			dim_Calendar_id_start = @i2 ,
			Type = 1 ,
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
		group by COUNTRY, dim_Location_id, car_group, FLEET_CARSALES, FLEET_RAC_TTL, FLEET_RAC_OPS, FLEET_LICENSEE
		
		select @rowcount = @@ROWCOUNT
		insert trace (Entity, key1, key2, data1) 
		select @entity, 'temp', CONVERT(varchar(8),@d1,112), 'inserted='+convert(varchar(20),@rowcount)
		
		begin tran
			-- delete any existing data
			delete inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY_Aggregate
			from inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY_Aggregate f
			where f.dim_Calendar_id_start between @i1 and @i2
			and f.Type = 1
				
			-- do not insert into trace inside a transaction
			select @updated = @@ROWCOUNT, @DeletedDate = GETDATE()
			
			-- insert new data
			insert inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY_Aggregate
				(
				  dim_Calendar_id_start
				, dim_Calendar_id_end
				, Type
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
				  dim_Calendar_id_start
				, dim_Calendar_id_end
				, Type
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
			from #Fact_FLEET_EUROPE_SUMMARY_HISTORY_Aggregate
			
			select @rowcount = @@ROWCOUNT
		
		commit tran

		insert trace (Entity, key1, key2, data1) 
		select @entity, 'pop', CONVERT(varchar(8),@d1,112), 'inserted='+convert(varchar(20),@rowcount)
																+ ', deleted=' + convert(varchar(20),@updated)
																+ ', @DeletedDate=' + coalesce(''''+convert(varchar(23),@DeletedDate,126)+'''', 'null')

		select @d1 = rep_date+1 from inp.dim_calendar where dim_Calendar_id = @i2
		select @d2 = LastDayOfWeek from Inp.dim_Calendar where Rep_Date = @d1

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

	if @@TRANCOUNT > 0
		rollback tran
	
	insert	Trace (Entity, key1, data1, data2)
	select	Entity = @entity, key1 = 'Failure',
		data1 = '<ErrProc=' + @ErrProc + '>'
			+ '<ErrLine=' + @ErrLine + '>'
			+ '<ErrDesc=' + @ErrDesc + '>',
		data2 = @key1
	raiserror('Failed %s', 16, -1, @ErrDesc)
end catch