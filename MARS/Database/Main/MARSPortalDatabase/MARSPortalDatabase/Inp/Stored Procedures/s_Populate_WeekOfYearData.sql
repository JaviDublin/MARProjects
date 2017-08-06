﻿CREATE proc inp.s_Populate_WeekOfYearData
as
/*
select top 100 *
from inp.WeekOfYearData
*/
set nocount on
begin try
declare @rowcount int, @inserted int, @updated int
declare @entity varchar(100)
declare @key1 varchar(100)
declare @data2 varchar(max)

	select @entity = OBJECT_NAME(@@PROCID)
	select @entity = coalesce(@entity,'test')

	insert trace (entity, key1, key2)
	select @entity, @key1, 'start'

	truncate table inp.WeekOfYearData
	
	;with d as
	(select FirstDayOfWeek, LastDayOfWeek, min_dim_Calendar_id = min(dim_Calendar_id), max_dim_Calendar_id = max(dim_Calendar_id)  from inp.dim_Calendar group by FirstDayOfWeek, LastDayOfWeek)
	--select * from d
	insert inp.WeekOfYearData (PeriodStart, PeriodEnd, min_dim_Calendar_id, max_dim_Calendar_id)
	select FirstDayOfWeek, LastDayOfWeek, min_dim_Calendar_id, max_dim_Calendar_id
	from d
	where exists (select * from inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY_WeekOfYear f1 where d.min_dim_Calendar_id = f1.dim_Calendar_id)

	select @rowcount = @@rowcount

	insert trace (entity, key1, key2, data1)
	select @entity, @key1, 'end', 'inserted=' + coalesce(convert(varchar(20),@rowcount),'null')

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