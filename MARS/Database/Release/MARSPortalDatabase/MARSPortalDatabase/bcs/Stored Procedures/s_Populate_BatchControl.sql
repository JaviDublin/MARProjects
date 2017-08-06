CREATE proc [bcs].[s_Populate_BatchControl]
@ProcessBatchName	varchar(100)
as
	declare @ProcessBatch_id int
	select @ProcessBatch_id = ProcessBatch_id from ProcessBatch where ProcessBatchName = @ProcessBatchName

	if @ProcessBatchName in ('Mars_All')
	begin
		select @ProcessBatch_id = ProcessBatch_id from ProcessBatch where ProcessBatchName = @ProcessBatchName

		-- populate with next filename
		insert	BatchControl (ProcessBatch_id, PrevProcessControlSeq, FileProcess_id, status, Data1, Data2, Data3, Data4)
		select	
				--top 1
				ProcessBatch_id				= @ProcessBatch_id ,
				PrevProcessControlSeq		= 0 ,
				FileProcess_id				= FileProcess_id ,
				status						= 'Success' ,
				Data1						= fp.FileName ,
				Data2						= case when Entity = 'Mars_Daily'
														then '\\hescft02\MarsFISdata\Temp\Carrent Daily\'
														else '\\hescft02\MarsFISdata\Temp\Carrent Hourly\'
													end ,
				Data3						= case when Entity = 'Mars_Daily' then 'Daily' else 'Hourly' end ,
				Data4						= null
		from	bcs.FileProcess fp
		where	Entity in ('Mars_Daily','Mars_Hourly')
		and		Status = 'Ready'  -- FileDownloaded
		and		not exists	(	select	*
								from	bcs.BatchControl
								where	ProcessBatch_id = @ProcessBatch_id
								and		Data1 = fp.FileName
							)
		order by FileName
	end

	if 'CMS_Forecast' = @ProcessBatchName
	begin
		insert	BatchControl (ProcessBatch_id, PrevProcessControlSeq, FileProcess_id, status, Data1, Data2, Data3, Data4)
		select	
				--top 1
				ProcessBatch_id				= @ProcessBatch_id ,
				PrevProcessControlSeq		= 0 ,
				FileProcess_id				= FileProcess_id ,
				status						= 'Success' ,
				Data1						= fp.FileName ,
				Data2						= '\\hescft02\MarsFISdata\Temp\CMS Forecast\' ,
				Data3						= '' ,
				Data4						= REPLACE(fp.FileName,'.gz','')
		from	bcs.FileProcess fp
		where	Entity = 'CMS_Forecast'
		and		Status = 'Ready'  -- FileDownloaded
		and		not exists	(	select	*
								from	bcs.BatchControl
								where	ProcessBatch_id = @ProcessBatch_id
								and		Data1 = fp.FileName
							)
		order by FileName
	end
	
	
	if 'BPServer_Fleet' = @ProcessBatchName
	begin
		insert into bcs.FileProcess
			(Entity , FileName , Status)
		values 
			(@ProcessBatchName , CONVERT(varchar(8),GETDATE(),112), 'Ready')
	
		insert	BatchControl 
			(ProcessBatch_id, PrevProcessControlSeq, FileProcess_id, status, Data1, Data2, Data3, Data4)
		select	
				--top 1
				ProcessBatch_id				= @ProcessBatch_id ,
				PrevProcessControlSeq		= 0 ,
				FileProcess_id				= FileProcess_id ,
				status						= 'Success' ,
				Data1						= CONVERT(varchar(8),GETDATE(),112) ,
				Data2						= NULL ,
				Data3						= NULL ,
				Data4						= NULL
		from	bcs.FileProcess fp
		where	Entity = 'BPServer_Fleet'
		and		Status = 'Ready'  -- FileDownloaded
		and		not exists	(	select	*
								from	bcs.BatchControl
								where	ProcessBatch_id = @ProcessBatch_id
								and		Data1 = fp.FileName
							)
		order by FileName
	end