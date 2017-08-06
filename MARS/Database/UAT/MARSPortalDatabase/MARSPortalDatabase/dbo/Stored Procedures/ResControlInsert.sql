CREATE PROCEDURE [dbo].[ResControlInsert]
	-- Add the parameters for the stored procedure here
	@ConditionId int = 1
	,@Comment varchar(max)
	,@ConDateTime datetime
	,@ErrorLogId int
	,@Timestamp datetime
AS
BEGIN

	SET NOCOUNT ON;
	
	if @ConDateTime is null set @ConDateTime=GETDATE()
	
	if @Timestamp is null exec [dbo].[ResGetLastTimstamp] @Timestamp out
	
	if @ErrorLogId < 1 set @ErrorLogId=null	 -- from the ssis package
	
    insert into dbo.[ResControl]  ([ConditionId],[Comment],[ConDateTime],[ErrorLogId],[TimeStamp])
	VALUES	(@ConditionId,@Comment,@ConDateTime,@ErrorLogId,@Timestamp)

END