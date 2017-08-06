create proc [bcs].s_rpt_FileProcessing
as
	set transaction isolation level read uncommitted
	
	select	FileName = bc.Data1 ,
			bch.ProcessControlSeq ,
			bch.Processname ,
			duration = convert(varchar(12),convert(time,case when bch.Status = 'Running' then getdate() else bch.EndDate end - bch.StartDate)) ,
			bch.Status ,
			StartDate = convert(varchar(8),bch.StartDate,112) + ' ' + CONVERT(varchar(8), convert(time,bch.StartDate)),
			EndDate = convert(varchar(8),bch.EndDate,112) + ' ' + CONVERT(varchar(8), convert(time,bch.EndDate)),
			bch.BatchControl_id
	from [bcs].BatchProcessHistory bch
		left join BatchControl bc
			on bch.BatchControl_id = bc.BatchControl_id
	union all
	select  FileName = max(maxrow.Data1) ,
			ProcessControlSeq = 9999 ,
			Processname = 'Summary (' + convert(varchar(20), max(maxrow.ProcessControlSeq)) + ')' 
							+ coalesce(' rows=' + max(RowsUploaded.RowsUploaded),'') + coalesce(' del=' + convert(varchar(20), convert(int, max(RowsUploaded.RowsUploaded)) - convert(int, max(RowsInserted.RowsInserted))),'') ,
			duration = convert(varchar(12),convert(time,case when max(maxrow.Status) = 'Running' then getdate() else max(maxrow.EndDate) end
								- max(minrow.StartDate))) ,
			Status = max(maxrow.Status) ,
			StartDate = convert(varchar(8),max(minrow.StartDate),112) + ' ' + CONVERT(varchar(8), convert(time,max(minrow.StartDate))) ,
			EndDate = convert(varchar(8),max(maxrow.EndDate),112) + ' ' + CONVERT(varchar(8), convert(time,max(maxrow.EndDate))) ,
			maxrow.BatchControl_id
	from	
			(
			select bc.Data1, bch.BatchControl_id, Status = '', bch.StartDate, EndDate = CONVERT(datetime, '19000101'), bch.ProcessControlSeq
			from [bcs].BatchProcessHistory bch
				left join BatchControl bc
					on bch.BatchControl_id = bc.BatchControl_id
				join (select bch.BatchControl_id, ProcessControlSeq = min(bch.ProcessControlSeq) from BatchProcessHistory bch group by bch.BatchControl_id) minrow
					on bch.BatchControl_id = minrow.BatchControl_id
					and bch.ProcessControlSeq = minrow.ProcessControlSeq
			) minrow
			join
			(
			select bc.Data1, bch.BatchControl_id, Status = bch.Status, StartDate = CONVERT(datetime, '19000101'), bch.EndDate, bch.ProcessControlSeq
			from [bcs].BatchProcessHistory bch
				left join BatchControl bc
				on bch.BatchControl_id = bc.BatchControl_id
			join (select bch.BatchControl_id, ProcessControlSeq = max(bch.ProcessControlSeq) from BatchProcessHistory bch group by bch.BatchControl_id) maxrow
				on bch.BatchControl_id = maxrow.BatchControl_id
				and bch.ProcessControlSeq = maxrow.ProcessControlSeq
			) maxrow
				on minrow.BatchControl_id = maxrow.BatchControl_id
			left join	
					(	select EventDate, RowsUploaded = SUBSTRING(Data1, charindex('RowsInserted=',data1)+13,10)
						from Trace
						where Entity = 'Import_Omniture_hit_data_transactions'
						and key1 = 'End'
					) RowsUploaded
						on RowsUploaded.EventDate between minrow.StartDate and maxrow.EndDate
			left join 
					(
						select EventDate, RowsInserted = replace(data1, 'inserted=', '')
						from Trace
						where Entity = 's_import_fact_OmnitureTransaction'
						and key1 = 'End'
					) RowsInserted
						on RowsInserted.EventDate between minrow.StartDate and maxrow.EndDate
			group by maxrow.BatchControl_id
	order by BatchControl_id desc , ProcessControlSeq desc