create proc s_Populate_Fact_FLEET_EUROPE_SUMMARY_HISTORY
as
begin try	

insert trace (Entity, key1) select 'pop inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY', 'start'

declare @i1 int, @i2 int
declare @d1 datetime, @d2 datetime
select @d1 = MIN(Rep_Date) from FLEET_EUROPE_SUMMARY_HISTORY 
select @d2 = Max(Rep_Date) from FLEET_EUROPE_SUMMARY_HISTORY

truncate table inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY

if exists (select * from sysindexes where name = 'ix_Fact_FLEET_EUROPE_SUMMARY_HISTORY_01')
	drop index inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY.ix_Fact_FLEET_EUROPE_SUMMARY_HISTORY_01
if exists (select * from sysindexes where name = 'ix_Fact_FLEET_EUROPE_SUMMARY_HISTORY')
	drop index inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY.ix_Fact_FLEET_EUROPE_SUMMARY_HISTORY

while @d1 < @d2
begin
	select @i1 = dim_Calendar_id from inp.dim_calendar where REP_DATE = @d1

	insert inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY
		(
		[dim_Calendar_id] ,
		[COUNTRY] ,
		[dim_Location_id] ,
		car_group ,
		[FLEET_CARSALES] ,
		[FLEET_RAC_TTL] ,
		[FLEET_RAC_OPS] ,
		FLEET_LICENSEE ,
		
		TOTAL_FLEET, 
		CARSALES, 
		CARHOLD_H ,
		CARHOLD_L ,
		CU, 
		HA, 
		HL, 
		LL, 
		NC, 
		PL, 
		TC, 
		SV, 
		WS, 
		WS_NONRAC ,
		OPERATIONAL_FLEET, 
		BD, 
		MM, 
		TW, 
		TB, 
		WS_RAC ,
		AVAILABLE_TB ,
		FS, 
		RL, 
		RP, 
		TN, 
		AVAILABLE_FLEET, 
		RT, 
		SU, 
		GOLD, 
		PREDELIVERY, 
		OVERDUE, 
		ON_RENT	,
		TOTAL_FLEET_MIN ,
		CARSALES_MIN ,
		CARHOLD_H_MIN ,
		CARHOLD_L_MIN ,
		CU_MIN ,
		HA_MIN ,
		HL_MIN ,
		LL_MIN ,
		NC_MIN ,
		PL_MIN ,
		TC_MIN ,
		SV_MIN ,
		WS_MIN ,
		WS_NONRAC_MIN ,
		OPERATIONAL_FLEET_MIN ,
		BD_MIN ,
		MM_MIN ,
		TW_MIN ,
		TB_MIN ,
		WS_RAC_MIN ,
		AVAILABLE_TB_MIN ,
		FS_MIN ,
		RL_MIN ,
		RP_MIN ,
		TN_MIN ,
		AVAILABLE_FLEET_MIN ,
		RT_MIN ,
		SU_MIN ,
		GOLD_MIN ,
		PREDELIVERY_MIN ,
		OVERDUE_MIN ,
		ON_RENT_MIN ,
		TOTAL_FLEET_MAX ,
		CARSALES_MAX ,
		CARHOLD_H_MAX ,
		CARHOLD_L_MAX ,
		CU_MAX ,
		HA_MAX ,
		HL_MAX ,
		LL_MAX ,
		NC_MAX ,
		PL_MAX ,
		TC_MAX ,
		SV_MAX ,
		WS_MAX ,
		WS_NONRAC_MAX ,
		OPERATIONAL_FLEET_MAX ,
		BD_MAX ,
		MM_MAX ,
		TW_MAX ,
		TB_MAX ,
		WS_RAC_MAX ,
		AVAILABLE_TB_MAX ,
		FS_MAX ,
		RL_MAX ,
		RP_MAX ,
		TN_MAX ,
		AVAILABLE_FLEET_MAX ,
		RT_MAX ,
		SU_MAX ,
		GOLD_MAX ,
		PREDELIVERY_MAX ,
		OVERDUE_MAX ,
		ON_RENT_MAX ,
		TOTAL_FLEET_MAX_ABSOLUTE ,
		OPERATIONAL_FLEET_MAX_ABSOLUTE ,
		OVERDUE_MAX_ABSOLUTE ,
		ON_RENT_MAX_ABSOLUTE
		)
	select
		dim_Calendar_id = @i1 ,
		h.COUNTRY ,
		dim_Location_id ,
		car_group ,
		FLEET_CARSALES ,
		FLEET_RAC_TTL ,
		FLEET_RAC_OPS ,
		FLEET_LICENSEE ,
		
		TOTAL_FLEET, 
		CARSALES, 
		CARHOLD_H ,
		CARHOLD_L ,
		CU, 
		HA, 
		HL, 
		LL, 
		NC, 
		PL, 
		TC, 
		SV, 
		WS, 
		WS_NONRAC ,
		OPERATIONAL_FLEET, 
		BD, 
		MM, 
		TW, 
		TB, 
		WS_RAC ,
		AVAILABLE_TB ,
		FS, 
		RL, 
		RP, 
		TN, 
		AVAILABLE_FLEET, 
		RT, 
		SU, 
		GOLD, 
		PREDELIVERY, 
		OVERDUE, 
		ON_RENT ,
		TOTAL_FLEET_MIN ,
		CARSALES_MIN ,
		CARHOLD_H_MIN ,
		CARHOLD_L_MIN ,
		CU_MIN ,
		HA_MIN ,
		HL_MIN ,
		LL_MIN ,
		NC_MIN ,
		PL_MIN ,
		TC_MIN ,
		SV_MIN ,
		WS_MIN ,
		WS_NONRAC_MIN ,
		OPERATIONAL_FLEET_MIN ,
		BD_MIN ,
		MM_MIN ,
		TW_MIN ,
		TB_MIN ,
		WS_RAC_MIN ,
		AVAILABLE_TB_MIN ,
		FS_MIN ,
		RL_MIN ,
		RP_MIN ,
		TN_MIN ,
		AVAILABLE_FLEET_MIN ,
		RT_MIN ,
		SU_MIN ,
		GOLD_MIN ,
		PREDELIVERY_MIN ,
		OVERDUE_MIN ,
		ON_RENT_MIN ,
		TOTAL_FLEET_MAX ,
		CARSALES_MAX ,
		CARHOLD_H_MAX ,
		CARHOLD_L_MAX ,
		CU_MAX ,
		HA_MAX ,
		HL_MAX ,
		LL_MAX ,
		NC_MAX ,
		PL_MAX ,
		TC_MAX ,
		SV_MAX ,
		WS_MAX ,
		WS_NONRAC_MAX ,
		OPERATIONAL_FLEET_MAX ,
		BD_MAX ,
		MM_MAX ,
		TW_MAX ,
		TB_MAX ,
		WS_RAC_MAX ,
		AVAILABLE_TB_MAX ,
		FS_MAX ,
		RL_MAX ,
		RP_MAX ,
		TN_MAX ,
		AVAILABLE_FLEET_MAX ,
		RT_MAX ,
		SU_MAX ,
		GOLD_MAX ,
		PREDELIVERY_MAX ,
		OVERDUE_MAX ,
		ON_RENT_MAX ,
		TOTAL_FLEET_MAX_ABSOLUTE ,
		OPERATIONAL_FLEET_MAX_ABSOLUTE ,
		OVERDUE_MAX_ABSOLUTE ,
		ON_RENT_MAX_ABSOLUTE
		
	from FLEET_EUROPE_SUMMARY_History h
		left join inp.dim_LOCATION l
			on l.LOCATION = h.LOCATION
	where h.Rep_Date = @d1
	
	insert trace (Entity, key1, key2, data1) 
	select 'pop inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY', 'opo', CONVERT(varchar(8),@d1,112), 'inserted='+convert(varchar(20),@@rowcount)
	
	select @d1 = @d1+1
end

insert trace (Entity, key1) select 'pop inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY', 'end'


create clustered index ix_Fact_FLEET_EUROPE_SUMMARY_HISTORY on inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY (dim_Calendar_id)
create index ix_Fact_FLEET_EUROPE_SUMMARY_HISTORY_01 on inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY (COUNTRY)

insert trace (Entity, key1) select 'pop inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY', 'index'

end try
begin catch
select ERROR_MESSAGE()
end catch