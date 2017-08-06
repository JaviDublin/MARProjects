CREATE proc [bcs].[s_FileProcess]
@Entity		varchar(50) ,
@Action		varchar(50) ,		-- InsertFile, GetFilesList, GetFilesToDelete
@Status		varchar(100) ,
@FileName	varchar(1000) = null ,
@Data		varchar(max) = null
as
/*
begin tran
exec s_FileProcess @Entity = 'TestControl', @Action = 'InsertFile', @Status = 'FileToDownload', @FileName='Test1.done'
exec s_FileProcess @Entity = 'TestControl', @Action = 'InsertFile', @Status = 'FileToDownload', @FileName='Test2.done'
exec s_FileProcess @Entity = 'TestControl', @Action = 'GetFilesList', @Status = 'FileToDownload'
exec s_FileProcess @Entity = 'TestControl', @Action = 'InsertFile', @Status = 'FileToDownloaded', @FileName='Test1.tar.gz'
exec s_FileProcess @Entity = 'TestControl', @Action = 'InsertFile', @Status = 'FileToDownloaded', @FileName='Test2.tar.gz'
exec s_FileProcess @Entity = 'TestControl', @Action = 'GetFilesToDelete', @Status = 'FileToDownloaded'
rollback tran
begin tran
exec s_FileProcess @Entity = 'Test', @Action = 'InsertFile', @Status = 'FileToDownload', @FileName='Test1.fin'
exec s_FileProcess @Entity = 'Test', @Action = 'InsertFile', @Status = 'FileToDownload', @FileName='Test2.fin'
exec s_FileProcess @Entity = 'Test', @Action = 'GetFilesList', @Status = 'FileToDownload'
exec s_FileProcess @Entity = 'Test', @Action = 'InsertFile', @Status = 'FileToDownloaded', @FileName='Test1.fin'
exec s_FileProcess @Entity = 'Test', @Action = 'InsertFile', @Status = 'FileToDownloaded', @FileName='Test2.fin'
exec s_FileProcess @Entity = 'Test', @Action = 'GetFilesToDelete', @Status = 'FileToDownloaded'
rollback tran
select top 100 * From fileprocess order by fileprocess_id desc
exec s_FileProcess @Entity = 'DClick_NetworkImpression_3995', @Action = 'GetFilesList', @Status = 'FileToDownload'

*/

	if @Entity like 'DUV%'
		insert Trace (Entity, Key1, data1, data2)
		select 's_FileProcess', @Action, @Entity + ', ' + coalesce(@FileName, ''), @Status

declare	@CurrentStatus varchar(100)
declare	@DataFileName varchar(1000)
declare @ExtractType varchar(20)
declare @ControlFileExt varchar(20)
declare @DataFileExt varchar(20)

	declare @ProcessType table (ProcessName varchar(50), ControlFileExt varchar(20), DataFileExt varchar(20), ExtractType varchar(20))
	insert @ProcessType select 'TestControl1', '.done', '.tar.gz', 'ControlFile'	-- key on control file - file to load has different name
	insert @ProcessType select 'TestControl2', '', '', 'Normal'			-- file to load as in parameter
	insert @ProcessType select 'TestControl3', '', '', 'NotLast'		-- do not load last file
	insert @ProcessType select 'TestControl4', '', '', 'After2Hours'	-- give 2 hours for upload to complete
	insert @ProcessType select 'TestControl4', '', '', 'NotLastDay'		-- do not oad last days files

	insert @ProcessType select 'Classic', '', '', 'Normal'
	insert @ProcessType select 'NewWorld', '', '', 'Normal'

	
	select	@ExtractType = ExtractType ,
			@ControlFileExt = ControlFileExt ,
			@DataFileExt = DataFileExt
	from	@ProcessType
	where	ProcessName = @Entity
	
	if @ExtractType = 'ControlFile'
		select	@DataFileName = REPLACE(@FileName, @ControlFileExt, @DataFileExt)
	
	if @DataFileName is null
		select @DataFileName = @FileName
	
	if @Action = 'InsertFile'
	begin
		select @CurrentStatus = Status
		from [bcs].FileProcess
		where Entity = @Entity
		and FileName = @DataFileName
		
		if @CurrentStatus is null
		begin
			insert [bcs].FileProcess
				(
				Entity ,
				FileName ,
				Status ,
				Data
				)
			select
				@Entity ,
				FileName	=	@DataFileName ,
				@Status ,
				@Data
			--from	(select Entity = @Entity) a
		end
		else if @CurrentStatus <> 'Complete'	-- complete - no going back
		and @Status <> 'FileToDownload'			-- cannot update to FileToDownload
		begin
			update	[bcs].FileProcess
			set		Status = @Status ,
					Data = @Data ,
					z_updated = GETDATE()
			where Entity = @Entity
			and FileName = @DataFileName
		end
	end
	else if @Action = 'GetFilesList'
	begin
		if @ExtractType = 'After2Hours'
		begin
			select	FileName
			from	FileProcess
			where	Entity = @Entity
			and		Status = @Status
			and		z_inserted < dateadd(hh,-2,getdate())
		end
		if @ExtractType = 'NotLast'
		begin
			select	FileName
			from	FileProcess
			where	Entity = @Entity
			and		Status = @Status
			and		FileName <>		(		select	max(FileName)
											from	FileProcess
											where	Entity = @Entity
											and		Status = @Status
									)
		end
		if @ExtractType = 'NotLastDay'		-- DClick_NetworkImpression_3995 only at the moment
		begin
			select	FileName
			from	FileProcess
			where	Entity = @Entity
			and		Status = @Status
			and		right(FileName, 17) <>	(		select	top 1 right(FileName, 17)
													from	[bcs].FileProcess
													where	Entity = @Entity
													and		Status = @Status
													order by FileProcess_id desc
													--order by convert(datetime,left(right(FileName, 17),10),103) desc
											)
		end
		if @ExtractType not in ('NotLast', 'NotLastDay','After2Hours')
		begin
			select	FileName
			from	[bcs].FileProcess
			where	Entity = @Entity
			and		Status = @Status
		end
	end
	else if @Action = 'GetFilesToDelete'
	begin
		select	FileName		=	case	when pt.Type = 'C'
												then REPLACE(fp.FileName, pt.DataFileExt, pt.ControlFileExt)
											else fp.FileName
									end
		from	FileProcess fp
			left join	(	select	ProcessName, Type = 'C', ControlFileExt, DataFileExt from @ProcessType where ProcessName = @Entity
							union all
							select	ProcessName, Type = 'D', ControlFileExt, DataFileExt from @ProcessType where ProcessName = @Entity
						) pt
				on fp.Entity = pt.ProcessName
		where	fp.Entity = @Entity
		and		fp.Status = @Status
	end