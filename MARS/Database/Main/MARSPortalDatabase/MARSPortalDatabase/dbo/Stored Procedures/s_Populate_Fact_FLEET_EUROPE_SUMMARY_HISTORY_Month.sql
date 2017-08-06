
CREATE proc [dbo].[s_Populate_Fact_FLEET_EUROPE_SUMMARY_HISTORY_Month]
@StartDate datetime = null ,
@endDate datetime = null ,
@debug int = 0
as
/*
exec s_Populate_Fact_FLEET_EUROPE_SUMMARY_HISTORY_Month
	@StartDate = '20110101' ,
	@endDate = '20120701'
begin tran
exec s_Populate_Fact_FLEET_EUROPE_SUMMARY_HISTORY_Month
	@StartDate = '20110101' ,
	@endDate = '20110101'
select top 100 * from trace order by trace_id desc
rollback tran

exec s_Populate_Fact_FLEET_EUROPE_SUMMARY_HISTORY_Month @debug = 1
select top 10 * from trace order by trace_id desc

*/
begin try

declare @rowcount int, @inserted int, @updated int
declare @DeletedDate datetime
declare @entity varchar(100)
declare @key1 varchar(100)
declare @data2 varchar(max)
	
	select @entity = OBJECT_NAME(@@PROCID)
	select @entity = coalesce(@entity,'test')

	insert trace (Entity, key1, data1) select @entity, 'start', '@StartDate=' + coalesce(''''+convert(varchar(23),@StartDate,126)+'''', 'null')
													+ ',@endDate=' + coalesce(''''+convert(varchar(23),@endDate,126)+'''', 'null')
	
	if @StartDate is null
		select @StartDate = max(MonthEnd) + 1 from inp.MonthlyData
	if @endDate is null
		select @endDate = max(MonthEnd) + 1 from inp.MonthlyData
	
	select @StartDate = d.FirstDayOfMonth
	from Inp.dim_Calendar d
	where d.Rep_Date = @StartDate
	
	select @endDate = d.LastDayOfMonth
	from Inp.dim_Calendar d
	where d.Rep_Date = @endDate

declare @i1 int, @i2 int
declare @d1 datetime, @d2 datetime
	select @i1 = MIN(dim_calendar_id) from inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY 
	select @i2 = Max(dim_calendar_id) from inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY
	select @d1 = FirstDayOfMonth from inp.dim_Calendar where dim_calendar_id = @i1
	select @d2 = FirstDayOfMonth-1 from inp.dim_Calendar where dim_calendar_id = @i2

	if @d1 < @StartDate
		select @d1 = @StartDate
	if @d2 > @endDate
		select @d2 = @endDate

	insert trace (Entity, key1, data1) select @entity, 'dates', '@StartDate=' + coalesce(''''+convert(varchar(23),@StartDate,126)+'''', 'null')
													+ ',@endDate=' + coalesce(''''+convert(varchar(23),@endDate,126)+'''', 'null')
													+ ',@d1=' + coalesce(''''+convert(varchar(23),@d1,126)+'''', 'null')
													+ ',@d2=' + coalesce(''''+convert(varchar(23),@d2,126)+'''', 'null')
													+ ',@i1=' + coalesce(''''+convert(varchar(20),@i1)+'''', 'null')
													+ ',@i2=' + coalesce(''''+convert(varchar(20),@i2)+'''', 'null')

	if @debug = 1
		return

	select
		dim_Calendar_id = @i1,
		Mth = @d1 ,
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
		ON_RENT 					=SUM(FES.ON_RENT) ,
		
		RT_MIN_SUM					= sum(RT_MIN) ,
		AVAILABLE_FLEET_MIN_SUM		= sum(AVAILABLE_FLEET_MIN) ,
		RT_MAX_SUM					= sum(RT_MAX) ,
		AVAILABLE_FLEET_MAX_SUM		= sum(AVAILABLE_FLEET_MAX)
	into #Fact_FLEET_EUROPE_SUMMARY_HISTORY_Month
	from inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY FES
	where 1=0
	group by COUNTRY, dim_Location_id, car_group, FLEET_CARSALES, FLEET_RAC_TTL, FLEET_RAC_OPS, FLEET_LICENSEE

	select
		dim_Calendar_id = @i1,
		Rep_DayOfWeek = DATEPART(dw,d.Rep_Date) ,
		Mth = @d1 ,
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
		ON_RENT 					=SUM(FES.ON_RENT)
	into #Fact_FLEET_EUROPE_SUMMARY_HISTORY_Month_DOW
	from inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY FES
		join inp.dim_calendar d
			on d.dim_calendar_id = FES.dim_calendar_id
	where 1=0
	group by DATEPART(dw,d.Rep_Date), COUNTRY, dim_Location_id, car_group, FLEET_CARSALES, FLEET_RAC_TTL, FLEET_RAC_OPS, FLEET_LICENSEE


	while @d1 < @d2
	begin
		-- get start and end dates for this loop
		select @i1 = d1.dim_calendar_id
		from inp.dim_Calendar d1
			join inp.dim_Calendar d2
				on d2.FirstDayOfMonth = d1.Rep_Date
		where d2.Rep_Date = @d1
	
		select @i2 = d1.dim_calendar_id
		from inp.dim_Calendar d1
			join inp.dim_Calendar d2
				on d2.LastDayOfMonth = d1.Rep_Date
		where d2.Rep_Date = @d1
		
		-- populate temp table
		truncate table #Fact_FLEET_EUROPE_SUMMARY_HISTORY_Month
		
		insert #Fact_FLEET_EUROPE_SUMMARY_HISTORY_Month
		select
			dim_Calendar_id = @i1,
			Mth = @d1 ,
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
			ON_RENT 					=SUM(FES.ON_RENT) ,
			
			RT_MIN_SUM					= sum(RT_MIN) ,
			AVAILABLE_FLEET_MIN_SUM		= sum(AVAILABLE_FLEET_MIN) ,
			RT_MAX_SUM					= sum(RT_MAX) ,
			AVAILABLE_FLEET_MAX_SUM		= sum(AVAILABLE_FLEET_MAX)
		from inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY FES
		where FES.dim_calendar_id between @i1 and @i2
		group by COUNTRY, dim_Location_id, car_group, FLEET_CARSALES, FLEET_RAC_TTL, FLEET_RAC_OPS, FLEET_LICENSEE

		select @rowcount = @@ROWCOUNT
		insert trace (Entity, key1, key2, data1) 
		select @entity, 'temp', CONVERT(varchar(8),@d1,112), 'inserted='+convert(varchar(20),@rowcount)
		
		begin tran
			-- delete any existing data
			delete inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY_Month
			from inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY_Month f
			where f.dim_calendar_id between @i1 and @i2
				
			-- do not insert into trace inside a transaction
			select @updated = @@ROWCOUNT, @DeletedDate = GETDATE()
			
			-- insert new data
			insert inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY_Month
				(
				  dim_Calendar_id
				, Mth
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
				, Mth
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
			from #Fact_FLEET_EUROPE_SUMMARY_HISTORY_Month
			
			select @rowcount = @@ROWCOUNT
		
		commit tran

		insert trace (Entity, key1, key2, data1) 
		select @entity, 'Month', CONVERT(varchar(8),@d1,112), 'inserted='+convert(varchar(20),@rowcount)
																+ ', deleted=' + convert(varchar(20),@updated)
																+ ', @DeletedDate=' + coalesce(''''+convert(varchar(23),@DeletedDate,126)+'''', 'null')
		
		truncate table #Fact_FLEET_EUROPE_SUMMARY_HISTORY_Month_DOW
		
		insert #Fact_FLEET_EUROPE_SUMMARY_HISTORY_Month_DOW
		select
			dim_Calendar_id = @i1,
			Rep_DayOfWeek = DATEPART(dw,d.Rep_Date) ,
			Mth = @d1 ,
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
			ON_RENT 					=SUM(FES.ON_RENT)
		from inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY FES
			join inp.dim_calendar d
				on d.dim_calendar_id = FES.dim_calendar_id
		where FES.dim_calendar_id between @i1 and @i2
		group by DATEPART(dw,d.Rep_Date), COUNTRY, dim_Location_id, car_group, FLEET_CARSALES, FLEET_RAC_TTL, FLEET_RAC_OPS, FLEET_LICENSEE

		select @rowcount = @@ROWCOUNT
		insert trace (Entity, key1, key2, data1) 
		select @entity, 'temp dow', CONVERT(varchar(8),@d1,112), 'inserted='+convert(varchar(20),@rowcount)

		begin tran
			-- delete any existing data
			delete inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY_Month_DOW
			from inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY_Month_DOW f
			where f.dim_calendar_id between @i1 and @i2
			
			-- do not inserrt into trace inside a transaction
			select @updated = @@ROWCOUNT, @DeletedDate = GETDATE()
			
			-- insert new data
			insert inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY_Month_DOW
				(
				  dim_Calendar_id
				, Rep_DayOfWeek
				, Mth
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
				)
			select
				  dim_Calendar_id
				, Rep_DayOfWeek
				, Mth
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
			from #Fact_FLEET_EUROPE_SUMMARY_HISTORY_Month_DOW
			
			select @rowcount = @@ROWCOUNT
		
		commit tran

		insert trace (Entity, key1, key2, data1) 
		select @entity, 'Month_DOW', CONVERT(varchar(8),@d1,112), 'inserted='+convert(varchar(20),@rowcount)
																+ ', deleted=' + convert(varchar(20),@updated)
																+ ', @DeletedDate=' + coalesce(''''+convert(varchar(23),@DeletedDate,126)+'''', 'null')


		select @d1 = DATEADD(mm,1,@d1)
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