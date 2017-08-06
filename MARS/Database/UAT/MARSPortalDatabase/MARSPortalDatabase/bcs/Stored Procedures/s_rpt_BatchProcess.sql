create proc [bcs].s_rpt_BatchProcess
as
	select BatchControl_id, ProcessControlSeq, Processname, StartDate, EndDate,
			Duration = convert(decimal(5,2),datediff(ss, StartDate, EndDate)/60.0),
			Data1 = replace(right(Data1, 18),'.tar.gz"','')
	into #BatchProcessHistory
	from [bcs].BatchProcessHistory
	where ProcessBatch_id = 1
	and	Status = 'Success'
	and BatchControl_id >= (select MAX(BatchControl_id) from BatchProcessHistory) - 10
	
declare @LastFile varchar(10)
	select @LastFile = Data1
	from #BatchProcessHistory 
	where BatchControl_id = (select MAX(BatchControl_id) from #BatchProcessHistory)
	and ProcessControlSeq = 1
	
declare @TotalAvDur decimal(5,2)
declare @PrevAvDur decimal(5,2)
declare @Prev_1AvDur decimal(5,2)
	select @TotalAvDur = AVG(Duration) from (select Duration = sum(Duration) from #BatchProcessHistory  where BatchControl_id <> (select MAX(BatchControl_id) from #BatchProcessHistory) group by BatchControl_id) a
	select @PrevAvDur = sum(Duration) from #BatchProcessHistory where BatchControl_id = (select MAX(BatchControl_id) from #BatchProcessHistory)
	select @Prev_1AvDur = sum(Duration) from #BatchProcessHistory where BatchControl_id = (select MAX(BatchControl_id) from #BatchProcessHistory) - 1
	
	select t.ProcessControlSeq, t.Processname, t.avduration_mins, last_file = @LastFile, last_end = tprev.EndDate, last_duration = tprev.duration, prev_1_duration = tprev_1.duration 
	from
	(
	select	ProcessControlSeq = max(ProcessControlSeq), Processname ,
			avduration_mins = convert(decimal(5,2),avg(datediff(ss,StartDate, EndDate))/60.0)
	from	#BatchProcessHistory
	group by Processname
	union all
	select ProcessControlSeq = 999, Processname = 'Summary', avduration_mins = @TotalAvDur
	) t
	left join
		(
		select	Processname, duration = convert(decimal(5,2),datediff(ss,StartDate, EndDate)/60.0), 
				EndDate = CONVERT(varchar(8),DATEADD(hh, 8, EndDate), 112) + ' ' + CONVERT(varchar(8),DATEADD(hh, 8, EndDate), 108)
		from	#BatchProcessHistory
		where	BatchControl_id = (select MAX(BatchControl_id) from #BatchProcessHistory)
		union all
		select Processname = 'Summary', avduration_mins = @PrevAvDur, EndDate = ''
		) tprev
		on tprev.Processname = t.Processname
	left join 
		(
		select	Processname, duration = convert(decimal(5,2),datediff(ss,StartDate, EndDate)/60.0)
		from	#BatchProcessHistory
		where	BatchControl_id = (select MAX(BatchControl_id) from #BatchProcessHistory)-1
		union all
		select Processname = 'Summary', avduration_mins = @Prev_1AvDur
		) tprev_1
		on tprev_1.Processname = t.Processname
	order by ProcessControlSeq desc