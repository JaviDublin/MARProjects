CREATE proc [bcs].[s_ProcessControl]
@ProcessBatchName	varchar(100) ,
@MaxLoop			int = 100 ,
@RunCmd				int = 1
as
/*
exec s_ProcessControl 'Mars_Daily'
*/
begin try
	declare @StartDate datetime
	declare @BatchProcessHistory_id int
	declare	@rc int
	
	-- Get the Processbatch id
	declare	@ProcessBatch_id int
	select	@ProcessBatch_id = ProcessBatch_id
	from	[bcs].ProcessBatch
	where	ProcessBatchName = @ProcessBatchName
	
	if @ProcessBatch_id is null
	begin
		raiserror ('No such batch %s', 16, -1, @ProcessBatchName)
		return
	end
	
	declare @BatchControl_id int ,
			@FileProcess_id int ,
			@bcData1 varchar(100) ,
			@bcData2 varchar(100) ,
			@bcData3 varchar(100) ,
			@bcData4 varchar(100)
			
	declare	@ProcessControlSeq int
	declare	@Status varchar(100)
	declare @NextRunTime datetime
	declare @RetryCount int

	-- get all steps for this batch
	select	*
	into	#ProcessControl
	from	[bcs].ProcessControl
	where	ProcessBatch_id = @ProcessBatch_id

	-- process each step in turn starting from the previous step
	select @Status = 'Success'
	while @Status = 'Success' and @MaxLoop > 0
	begin
		-- get a batch
		select	@BatchControl_id = null ,
				@Status = null
		select top 1
				@BatchControl_id	= BatchControl_id ,
				@FileProcess_id		= FileProcess_id ,
				@bcData1			= Data1 ,
				@bcData2			= Data2 ,
				@bcData3			= Data3 ,
				@bcData4			= Data4 ,
				@ProcessControlSeq	= PrevProcessControlSeq ,
				@Status				= Status ,
				@NextRunTime		= coalesce(NextRunTime,'19000101') ,
				@RetryCount			= RetryCount
		from [bcs].BatchControl
		where	ProcessBatch_id = @ProcessBatch_id
		and		status <> 'Complete'
		and		status <> 'Suspend'
		order by BatchControl_id
		
		if @BatchControl_id is not null
		begin
			if @Status not in ('Success', 'Retry')
			begin
				raiserror ('Cannot run batch - last status = %s', 16, -1, @Status)
				return
			end
			if @NextRunTime > getdate() and @Status = 'Retry'
			begin
				return	-- wait for retry timeout
			end
			
			-- ok run the batch
			declare @ProcessType varchar(20) ,
					@Processname varchar(50) ,
					@Data1 varchar(1000) ,
					@Data2 varchar(1000) ,
					@Data3 varchar(max) ,
					@ExecuteUser varchar(200) ,
					@RetryWaitTime datetime ,
					@RetryCountPC int
			
			declare @sql nvarchar(1000)
			
			-- process batch - run each step in turn starting after the previous step
			while @ProcessControlSeq < (select MAX(ProcessControlSeq) from #ProcessControl) and @MaxLoop > 0
			begin
				select @MaxLoop = @MaxLoop - 1
				
				-- get next batch entry
				select top 1 
						@ProcessControlSeq = ProcessControlSeq ,
						@Processname = Processname ,
						@ProcessType = ProcessType ,
						@Data1 = Data1 ,
						@Data2 = Data2 ,
						@Data3 = Data3 ,
						@ExecuteUser = ExecuteUser ,
						@RetryWaitTime = RetryWaitTime ,
						@RetryCountPC = RetryCount
				from #ProcessControl
				where ProcessControlSeq > @ProcessControlSeq
				order by ProcessControlSeq
				
				-- Add entry to BatchHistory set to running
				-- fail batch if still running
				if exists	(	select * 
								from [bcs].BatchHistory
								where BatchControl_id = @BatchControl_id
								and	ProcessBatch_id = @ProcessBatch_id
								and	Status = 'Running'
							)
				begin
					update	[bcs].BatchHistory
					set		[Status] = 'Failed'
					where	ProcessBatch_id = @ProcessBatch_id
					and		Status = 'Running'

					raiserror('Batch failed to complete %d', 16, -1, @ProcessBatch_id)

					return
				end
				
				if not exists	(	select * from BatchHistory
									where BatchControl_id = @BatchControl_id
									and ProcessBatch_id = @ProcessBatch_id
								)
				begin
					insert BatchHistory (BatchControl_id, ProcessBatch_id, Status)
					select @BatchControl_id, @ProcessBatch_id, 'Running'
				end
				else
				begin
					update BatchHistory
					set	Status = 'Running'
					where BatchControl_id = @BatchControl_id
					and ProcessBatch_id = @ProcessBatch_id
				end
				
				-- processs batch entry
				if	@ProcessType = 'StoredProc'
				begin
					select	@sql = 'exec ' + @Data2
					if @Data3 is not null
					begin
						if @bcData1 is not null
							select	@Data3 = REPLACE(@Data3, '^Data1^', @bcData1)
						if @bcData2 is not null
							select	@Data3 = REPLACE(@Data3, '^Data2^', @bcData2)
						if @bcData3 is not null
							select	@Data3 = REPLACE(@Data3, '^Data3^', @bcData3)
						if @bcData4 is not null
							select	@Data3 = REPLACE(@Data3, '^Data4^', @bcData4)
						if @FileProcess_id is not null
							select	@Data3 = REPLACE(@Data3, '<FileProcess_id>', convert(varchar(20),@FileProcess_id))

						select	@sql = @sql + ' ' + @Data3
					end
					
					insert	BatchProcessHistory (ProcessBatch_id, BatchControl_id, ProcessControlSeq, Processname, StartDate, EndDate, Status, Data1)
					select	@ProcessBatch_id, @BatchControl_id, @ProcessControlSeq, @Processname, GETDATE(), '19000101', 'Running', @sql
					select @BatchProcessHistory_id = SCOPE_IDENTITY()
if @RunCmd = 1
					exec (@sql)
					
					update	[bcs].BatchProcessHistory
					set		EndDate = GETDATE() ,
							Status = 'Success'
					where	BatchProcessHistory_id = @BatchProcessHistory_id
			
if @RunCmd = 1
					update	[bcs].BatchControl
					set		status = 'Success' ,
							RetryCount = null ,
							NextRunTime = '19000101' ,
							PrevProcessControlSeq = @ProcessControlSeq 
					where	BatchControl_id = @BatchControl_id

					update [bcs].BatchHistory
					set	Status = 'Success'
					where BatchControl_id = @BatchControl_id
					and ProcessBatch_id = @ProcessBatch_id
				end
				if @ProcessType = 'SSIS'
				begin
					declare @cmd varchar(8000)
					select @cmd = 'C:\"Program Files"\"Microsoft SQL Server"\100\DTS\Binn\DTEXEC.exe'
					select @cmd = @cmd + ' /f "I:\SSIS packages repository\' + @Data2 + '"'
					
					if @Data3 is not null
					begin
						if @bcData1 is not null
							select @Data3 = REPLACE(@Data3, '^Data1^', @bcData1)
						if @bcData2 is not null
							select @Data3 = REPLACE(@Data3, '^Data2^', @bcData2)
						if @bcData3 is not null
							select @Data3 = REPLACE(@Data3, '^Data3^', @bcData3)
						if @bcData4 is not null
							select @Data3 = REPLACE(@Data3, '^Data4^', @bcData4)
						select @Data3 = REPLACE(@Data3, '<', ' /set \package.variables[User::')
						select @Data3 = REPLACE(@Data3, '>', '].Value;\"')
						select @Data3 = REPLACE(@Data3, ',', '\"')
						select @Data3 = @Data3 + '\"'
						select @cmd = @cmd + @Data3
					end
					
					insert	[bcs].BatchProcessHistory 
						(ProcessBatch_id, BatchControl_id, ProcessControlSeq, Processname, StartDate, 
						EndDate, [Status] , Data1)
					select	
						@ProcessBatch_id, @BatchControl_id, @ProcessControlSeq, @Processname, 
						GETDATE(), '19000101', 'Running', @cmd + coalesce(' --EXECUTE AS LOGIN = '+@ExecuteUser, '')
					select @BatchProcessHistory_id = SCOPE_IDENTITY()
					
if @RunCmd = 1
begin
					if @ExecuteUser is not null
						EXECUTE AS LOGIN = @ExecuteUser
					declare @SSIS_Log table (seq int identity, data varchar(max))
					insert @SSIS_Log (data)
					exec @rc = master..xp_cmdshell @cmd
					if @rc <> 0
					begin
						revert
						
						-- log ssis error
						insert bcs.SSIS_Log (BatchProcessHistory_id, seq, data, z_inserted)
						select @BatchProcessHistory_id, seq, data, GETDATE()
						from @SSIS_Log

						raiserror('failed to exec cmd rc=%d', 16, -1, @rc)
						return
					end
					revert
end			
					update	[bcs].BatchProcessHistory
					set		EndDate = GETDATE() ,
							Status = 'Success'
					where	BatchProcessHistory_id = @BatchProcessHistory_id
					
if @RunCmd = 1
					update	[bcs].BatchControl
					set		status = 'Success' ,
							RetryCount = null ,
							NextRunTime = '19000101' ,
							PrevProcessControlSeq = @ProcessControlSeq 
					where	BatchControl_id = @BatchControl_id

					update [bcs].BatchHistory
					set	Status = 'Success'
					where BatchControl_id = @BatchControl_id
					and ProcessBatch_id = @ProcessBatch_id
				end
			end
		end
		-- update for end of batch
		if @ProcessControlSeq = (select MAX(ProcessControlSeq) from #ProcessControl)
		begin
			update	[bcs].FileProcess
			set		Status = 'Complete'
			where	FileProcess_id = @FileProcess_id
			
			update	[bcs].BatchControl
			set		status = 'Complete'
			where	BatchControl_id = @BatchControl_id
			
			update [bcs].BatchHistory
			set	Status = 'Complete'
			where BatchControl_id = @BatchControl_id
			and ProcessBatch_id = @ProcessBatch_id
		end
	end	
end try
begin catch
declare @ErrDesc varchar(max)
declare @ErrProc varchar(128)
declare @ErrLine varchar(20)
declare @Retry varchar(20) = ''

	select 	@ErrProc = ERROR_PROCEDURE() ,
				@ErrLine = ERROR_LINE() ,
				@ErrDesc = ERROR_MESSAGE()
	select	@ErrProc = coalesce(@ErrProc,'<null>') ,
			@ErrLine = coalesce(@ErrLine,'<null>') ,
			@ErrDesc = coalesce(@ErrDesc,'<null>')

	if @@TRANCOUNT >0
		rollback tran		-- just in case

	if @RetryCount is null and @RetryCountPC > 0
		select @RetryCount = @RetryCountPC

	if @RetryCount > 0 and @RetryCountPC > 0
	begin
		update	[bcs].BatchControl
		set		status = 'Retry' ,
				NextRunTime = GETDATE() + @RetryWaitTime ,
				RetryCount = @RetryCount - 1
		where	BatchControl_id = @BatchControl_id

		update	[bcs].BatchHistory	-- cannot leave as running
		set		[Status] = 'Failed'
		where	ProcessBatch_id = @ProcessBatch_id
		and		Status = 'Running'

		select @Retry = 'Retry'
	end	
	else
	begin
		update	[bcs].BatchControl
		set		status = 'Failed'
		where	BatchControl_id = @BatchControl_id

		update	[bcs].BatchHistory	-- cannot leave as running
		set		[Status] = 'Failed'
		where	ProcessBatch_id = @ProcessBatch_id
		and		Status = 'Running'
	end
	
	update	[bcs].BatchProcessHistory
	set		EndDate = GETDATE() ,
			Status = 'Failed'
	where	BatchProcessHistory_id = @BatchProcessHistory_id
	
	insert	Trace (Entity, key1, data1, data2)
	select	Entity = 's_ProcessControl', key1 = 'failure' + @Retry,
				data1 = '<ErrProc=' + @ErrProc + '>'
							+ '<ErrLine=' + @ErrLine + '>'
							+ '<ErrDesc=' + @ErrDesc + '>' ,
				data2 = '<@BatchControl_id=' + coalesce(convert(varchar(20),@BatchControl_id),'null') + '>'
							+ '<@ProcessBatch_id=' + coalesce(convert(varchar(20),@ProcessBatch_id),'null') + '>'
	raiserror('Failed %s', 16, -1, @ErrDesc)
end catch