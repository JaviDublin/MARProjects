
CREATE proc bcs.s_rpt_Batches
@date datetime = '25000101'
as
	;with cte1 as
	(
	select top 100 *
	from bcs.BatchControl
	order by BatchControl_id desc
	) ,
	cte2 as	
	(
	select cte1.BatchControl_id, startdate = min(bph.StartDate), EndDate =  max(bph.EndDate)
	from bcs.BatchProcessHistory bph
		join cte1
			on cte1.BatchControl_id = bph.BatchControl_id
	group by cte1.BatchControl_id 
	)
	select	pb.ProcessBatchName ,
			cte1.BatchControl_id ,
			StartDate = convert(varchar(10),cte2.startdate,121) + ' ' + convert(varchar(8),cte2.startdate,108) ,
			EndDate = convert(varchar(10),cte2.EndDate,121) + ' ' + convert(varchar(8),cte2.EndDate,108) ,
			cte1.status,
			fp.FileName 
	from cte1
		left join cte2
			on cte1.BatchControl_id = cte2.BatchControl_id
		join bcs.ProcessBatch pb
			on cte1.ProcessBatch_id = pb.ProcessBatch_id
		join bcs.FileProcess fp
			on cte1.FileProcess_id = fp.FileProcess_id
	order by cte1.BatchControl_id desc