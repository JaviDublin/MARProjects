CREATE proc inp.s_Populate_Data_Aggregate
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

	;with cte1 as
	(
	select distinct
		Type ,
		min_dim_Calendar_id = dim_Calendar_id_start ,
		max_dim_Calendar_id = dim_Calendar_id_end
	from Inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY_Aggregate a
	) ,
	cte as
	(
	select a.* ,
		PeriodStart = d1.Rep_Date ,
		PeriodEnd = d2.Rep_Date
	from cte1 a
		join Inp.dim_Calendar d1
			on d1.dim_Calendar_id = a.min_dim_Calendar_id
		join Inp.dim_Calendar d2
			on d2.dim_Calendar_id = a.max_dim_Calendar_id
	)	
	merge inp.Data_Aggregate t
		using cte t2
			on t.Type = t2.Type
			and t.min_dim_Calendar_id = t2.min_dim_Calendar_id
	when matched
	and	(	t.max_dim_Calendar_id <> t2.max_dim_Calendar_id
		or	t.PeriodStart <> t2.PeriodStart
		or	t.PeriodEnd <> t2.PeriodEnd
		)
		then update set
			max_dim_Calendar_id = t2.max_dim_Calendar_id ,
			PeriodStart = t2.PeriodStart ,
			PeriodEnd = t2.PeriodEnd ,
			z_updated = getdate()
	when not matched by target then insert
			(
				Type ,
				min_dim_Calendar_id ,
				max_dim_Calendar_id ,
				PeriodStart ,
				PeriodEnd ,
				z_inserted ,
				z_updated
			)
		values
			(
				t2.Type ,
				t2.min_dim_Calendar_id ,
				t2.max_dim_Calendar_id ,
				t2.PeriodStart ,
				t2.PeriodEnd ,
				getdate() ,
				getdate()
			)
	when not matched by source
		then delete;
	
	select @rowcount = @@rowcount

	insert trace (entity, key1, key2, data1)
	select @entity, @key1, 'end', 'updated=' + coalesce(convert(varchar(20),@rowcount),'null')

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