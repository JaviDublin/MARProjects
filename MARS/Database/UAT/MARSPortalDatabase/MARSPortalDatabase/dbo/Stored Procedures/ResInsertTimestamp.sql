CREATE PROCEDURE [dbo].[ResInsertTimestamp]

AS
BEGIN
	SET NOCOUNT ON;

	declare @noOfRcds int = 0
		,@timestamp varchar(50)
		,@AsDate datetime
    select @noOfRcds=COUNT(*) from dbo.ReservationStaging
    
    if @noOfRcds=0 return;
    
    select top 1 @timestamp=eff_dttm from dbo.ReservationStaging order by eff_dttm desc    
    
    --bit scruffy but it works not sure about longetivity!
    select @AsDate= SUBSTRING(@timestamp,7,4)+'-'+SUBSTRING(@timestamp,4,2)+'-'+LEFT(@timestamp,2)+' '+ RIGHT(@timestamp,8)
    
    --declare @AsDate datetime='2013-09-08 11:00' -- change the timestamp here for a larger import
    INSERT INTO [MarsPortal].[dbo].[ResControl]
           ([ConditionId]
           ,[Comment]
           ,[ConDateTime]
           ,[ErrorLogId]
           ,[TimeStamp])
     VALUES
           (1,'Updating the timestamp',GETDATE(),null,@AsDate)
    
END