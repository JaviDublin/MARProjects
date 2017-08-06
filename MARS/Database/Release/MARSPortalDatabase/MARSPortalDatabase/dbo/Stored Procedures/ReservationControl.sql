CREATE PROCEDURE [dbo].[ReservationControl]

AS
BEGIN

	SET NOCOUNT ON

declare @condition int,@_result int, @_resCount varchar(10),@_rmkCount varchar(10)
,@_timeStamp DateTime,@_ts DateTime

SELECT TOP 1 @condition=ConditionId FROM [ResControl] order by Id desc

select top 1 @_ts=[TimeStamp] FROM [ResControl] order by Id desc
select @_timeStamp=ISNULL(@_ts,getDate())

if @condition>1  return -- not idle return

insert into [ResControl]  ([ConditionId],[Comment],[ConDateTime],[ErrorLogId],[TimeStamp])
VALUES		(2,'Changing to Running',GETDATE(),NULL,@_timeStamp)

insert into [ResControl]  ([ConditionId],[Comment],[ConDateTime],[ErrorLogId],[TimeStamp])
VALUES		(2,'Truncating ReservationStaging',GETDATE(),NULL,@_timeStamp)
truncate table ReservationStaging

insert into [ResControl]  ([ConditionId],[Comment],[ConDateTime],[ErrorLogId],[TimeStamp])
VALUES		(2,'Truncating ResRmkStaging',GETDATE(),NULL,@_timeStamp)
truncate table ResRmkStaging

insert into [ResControl]  ([ConditionId],[Comment],[ConDateTime],[ErrorLogId],[TimeStamp])
VALUES		(2,'Running Import Program',GETDATE(),NULL,@_timeStamp)

EXEC @_result=master.. xp_cmdshell 'I:\ResToTeradata\Pooling.exe'

select @_resCount=COUNT(*) from dbo.reservationStaging
select @_rmkCount=COUNT(*) from dbo.ResRmkStaging

if @_resCount=0 
begin
	exec [dbo].[ReservationUpdate]
	insert into [ResControl]  ([ConditionId],[Comment],[ConDateTime],[ErrorLogId],[TimeStamp])
	VALUES		(1,'Updated CI and CO in Reservation table',GETDATE(),NULL,@_timeStamp)
	return;
end

select top 1 @_timeStamp=[TimeStamp] FROM [ResControl] order by Id desc

insert into [ResControl]  ([ConditionId],[Comment],[ConDateTime],[ErrorLogId],[TimeStamp])
	VALUES		(2,'Reservation field count = '+@_resCount,GETDATE(),NULL,@_timeStamp)
insert into [ResControl]  ([ConditionId],[Comment],[ConDateTime],[ErrorLogId],[TimeStamp])
	VALUES		(2,'Remark field count = '+@_rmkCount,GETDATE(),NULL,@_timeStamp)

if @_result!=0 
begin
	INSERT INTO [dbo].[Reservation_Error_Log]
           ([Message],[DateTime],[ResErrorTypeId])
     VALUES('Import from GDW failed.',GETDATE(),4)
	declare @_errorId int
	select top 1 @_errorId=Id from [dbo].[Reservation_Error_Log] order by Id desc
	INSERT INTO [dbo].[Reservation_Log]
           ([Message],[DateTime],[ErrorLog_Id])
     VALUES('Upload from GDW unsuccesful',GETDATE(),@_errorId)
    insert into [dbo].[ResControl]  ([ConditionId],[Comment],[ConDateTime],[ErrorLogId],[TimeStamp])
	VALUES		(3,'Import program failed',GETDATE(),@_errorId,@_timeStamp)
	return
end

insert into [ResControl]  ([ConditionId],[Comment],[ConDateTime],[ErrorLogId],[TimeStamp])
VALUES		(2,'Finished Import Program with no errors',GETDATE(),NULL,@_timeStamp)

insert into [ResControl]  ([ConditionId],[Comment],[ConDateTime],[ErrorLogId],[TimeStamp])
VALUES		(2,'Running Import stored proc [dbo].[ReservationImport]',GETDATE(),NULL,@_timeStamp)
exec [dbo].[ReservationImport]

-- set the timestamp here for and early import
--declare @_timeStamp datetime='2013-08-06 18:55:37.000'
insert into [ResControl]  ([ConditionId],[Comment],[ConDateTime],[ErrorLogId],[TimeStamp])
VALUES		(1,'Finished going back to Idle',GETDATE(),NULL,@_timeStamp)

INSERT INTO [dbo].[Reservation_Log]
           ([Message],[DateTime],[ErrorLog_Id])
     VALUES('Upload from GDW succesful',GETDATE(),null)

END