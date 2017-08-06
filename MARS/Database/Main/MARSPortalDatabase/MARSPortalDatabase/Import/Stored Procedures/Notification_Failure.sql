-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Import].[Notification_Failure]
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	
	DECLARE @today DATETIME  = DATEADD(D,0,DATEDIFF(D,0,GETDATE()))

	DECLARE @hour INT = DATEPART(HOUR,GETDATE())

	DECLARE @FileLog TABLE 
		(
			BatchControlId INT , [File_Name] VARCHAR(100), File_Type VARCHAR(30) ,
			File_Status VARCHAR(30) , Entity VARCHAR(30) , Inserted DATETIME ,
			Updated DATETIME , StepNumber INT
		)
		
	INSERT INTO @FileLog 
	SELECT 
		bc.BatchControl_id			as [BatchControlId] ,
		bc.Data1					as [File_Name],
		bc.Data3					as [File_Type],
		bc.[status]					as [File_Status],
		fp.Entity					as [Entity],
		fp.z_inserted				as [Inserted],
		fp.z_updated				as [Updated] ,
		bc.PrevProcessControlSeq	as [StepNumber]
	FROM 
		[bcs].BatchControl bc
	INNER JOIN 
		[bcs].FileProcess fp on bc.FileProcess_id = fp.FileProcess_id
	WHERE 
		fp.z_inserted > @today
	
	DECLARE @status_daily VARCHAR(30) , @status_hourly VARCHAR(30) ,@status_cms VARCHAR(30)
	DECLARE @counter_hourly INT
	
	SET @status_daily = 
		(SELECT TOP 1 File_Status FROM @FileLog WHERE Entity = 'Mars_Daily' ORDER BY BatchControlId DESC)
	SET @status_hourly = 
		(SELECT TOP  1 File_Status FROM @FileLog WHERE Entity = 'Mars_Hourly' ORDER BY BatchControlId DESC)
	SET @status_cms = 
		(SELECT TOP  1 File_Status FROM @FileLog WHERE Entity = 'CMS_Forecast' ORDER BY BatchControlId DESC)
	
	SET @counter_hourly = 
		(SELECT COUNT(*) FROM @FileLog WHERE Entity = 'Mars_Hourly')
	
	--SELECT @status_daily , @status_hourly , @status_cms , @counter_hourly
	
 
	IF @status_daily			IS NULL			SELECT 0  AS [result]
	ELSE IF @status_daily		= 'Failed'		SELECT 1  AS [result]
	ELSE IF @status_daily		= 'Success'		SELECT 2  AS [result]
	ELSE IF @status_cms			IS NULL			SELECT 3  AS [result]
	ELSE IF @status_cms			= 'Failed'		SELECT 4  AS [result]
	--ELSE IF @status_cms			= 'Success'		SELECT 5  AS [result]
	ELSE IF @status_hourly		IS NULL			SELECT 6  AS [result]
	ELSE IF @status_hourly		= 'Failed'		SELECT 7  AS [result]
	ELSE IF @status_hourly		= 'Success'		SELECT 8  AS [result]
	ELSE IF @counter_hourly		= 0				SELECT 9  AS [result]
	--ELSE IF @counter_hourly		< @hour - 2		SELECT 10 AS [result]
	ELSE										SELECT 11 AS [result]
	
	
	
	
	--DECLARE @today DATETIME  = DATEADD(D,0,DATEDIFF(D,0,GETDATE()))

	--DECLARE @status VARCHAR(50) = NULL

	--; WITH FileLog AS
	--(
	--SELECT 
	--	bc.BatchControl_id			as [BatchControlId] ,
	--	bc.Data1					as [File_Name],
	--	bc.Data3					as [File_Type],
	--	bc.[status]					as [File_Status],
	--	fp.Entity					as [Entity],
	--	fp.z_inserted				as [Inserted],
	--	fp.z_updated				as [Updated] ,
	--	bc.PrevProcessControlSeq	as [StepNumber]
	--FROM bcs.BatchControl bc
	--inner join bcs.FileProcess fp on bc.FileProcess_id = fp.FileProcess_id
	--WHERE fp.z_inserted > @today AND bc.Data3 = 'Daily'
	--)
	
	--SELECT @status = (SELECT File_Status FROM FileLog)
	 
	--IF @status IS NULL SELECT 0 as [result]
	--ELSE IF @status = 'Failed' SELECT 1 as [result]
	--ELSE SELECT 2 as [result]
	
    
END