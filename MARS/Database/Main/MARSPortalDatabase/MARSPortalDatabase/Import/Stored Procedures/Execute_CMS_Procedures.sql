-- =============================================
-- Author:		Javier
-- Create date: June 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Import].[Execute_CMS_Procedures]
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @entity VARCHAR(100)
	DECLARE @key1	VARCHAR(100)
	DECLARE @key2   VARCHAR(5)
	DECLARE @data1	VARCHAR(100)
	DECLARE @data2	VARCHAR(max)
	
	SET @key2 = 'CMS'


    DECLARE @yesterday DATETIME , @today DATETIME
    
    SET @yesterday	= DATEADD(dd, DATEDIFF(dd, 0, GETDATE() - 1), 0) 
    
    SET @today		= DATEADD(dd, DATEDIFF(dd, 0, GETDATE()), 0) 
    
    EXEC [Import].[Insert_CMS_Forecast_History] --@theDay = @yesterday
    
    SELECT @entity	= '[Import].[Insert_CMS_Forecast_History]'
	SELECT @key1	= convert(varchar(20),@@spid)
	SELECT @data1	= 'Execute_CMS_Procedures - Complete 1'
	SELECT data2	= CONVERT(varchar(30),GETDATE(),113)
	INSERT Trace (Entity, key1, key2 , data1, data2)
	SELECT @entity, @key1, @key2 , @data1, @data2
    
    
    EXEC [Import].[Update_CMS_Forecast_History] --@theDay = @yesterday
    
    SELECT @entity	= '[Import].[Update_CMS_Forecast_History]'
	SELECT @key1	= convert(varchar(20),@@spid)
	SELECT @data1	= 'Execute_CMS_Procedures - Complete 2'
	SELECT data2	= CONVERT(varchar(30),GETDATE(),113)
	INSERT Trace (Entity, key1, key2 , data1, data2)
	SELECT @entity, @key1, @key2 , @data1, @data2
    
    EXEC [Import].[Update_CMS_Forecast_History_ONRENT] --@theDay = @yesterday
    
    SELECT @entity	= '[Import].[Update_CMS_Forecast_History_ONRENT]'
	SELECT @key1	= convert(varchar(20),@@spid)
	SELECT @data1	= 'Execute_CMS_Procedures - Complete 3'
	SELECT data2	= CONVERT(varchar(30),GETDATE(),113)
	INSERT Trace (Entity, key1, key2 , data1, data2)
	SELECT @entity, @key1, @key2 , @data1, @data2
    
    EXEC [Import].[Update_CMS_Forecast_History_ADJUSTMENT] --@theDay = @yesterday
    
    SELECT @entity	= '[Import].[Update_CMS_Forecast_History_ADJUSTMENT]'
	SELECT @key1	= convert(varchar(20),@@spid)
	SELECT @data1	= 'Execute_CMS_Procedures - Complete 4'
	SELECT data2	= CONVERT(varchar(30),GETDATE(),113)
	INSERT Trace (Entity, key1, key2 , data1, data2)
	SELECT @entity, @key1, @key2 , @data1, @data2
    
    EXEC [Import].[Truncate_CMS_Forecast]
    
    SELECT @entity	= '[Import].[Truncate_CMS_Forecast]'
	SELECT @key1	= convert(varchar(20),@@spid)
	SELECT @data1	= 'Execute_CMS_Procedures - Complete 5'
	SELECT data2	= CONVERT(varchar(30),GETDATE(),113)
	INSERT Trace (Entity, key1, key2 , data1, data2)
	SELECT @entity, @key1, @key2 , @data1, @data2
	
	EXEC [Import].[Insert_CMS_Forecast]
	
	SELECT @entity	= '[Import].[Insert_CMS_Forecast]'
	SELECT @key1	= convert(varchar(20),@@spid)
	SELECT @data1	= 'Execute_CMS_Procedures - Complete 6'
	SELECT data2	= CONVERT(varchar(30),GETDATE(),113)
	INSERT Trace (Entity, key1, key2 , data1, data2)
	SELECT @entity, @key1, @key2 , @data1, @data2
	
	EXEC [Import].[Update_CMS_Forecast_CURRENT_ONRENT] --@theDay = @today
	
	SELECT @entity	= '[Import].[Update_CMS_Forecast_CURRENT_ONRENT]'
	SELECT @key1	= convert(varchar(20),@@spid)
	SELECT @data1	= 'Execute_CMS_Procedures - Complete 7'
	SELECT data2	= CONVERT(varchar(30),GETDATE(),113)
	INSERT Trace (Entity, key1, key2 , data1, data2)
	SELECT @entity, @key1, @key2 , @data1, @data2
	
	EXEC [Import].[Insert_CMS_Adjustment] --@theDay = @today
	
	SELECT @entity	= '[Import].[Insert_CMS_Adjustment]'
	SELECT @key1	= convert(varchar(20),@@spid)
	SELECT @data1	= 'Execute_CMS_Procedures - Complete 8'
	SELECT data2	= CONVERT(varchar(30),GETDATE(),113)
	INSERT Trace (Entity, key1, key2 , data1, data2)
	SELECT @entity, @key1, @key2 , @data1, @data2	
	
	--EXEC [Import].[Update_CMS_Forecast_OPERATIONAL_FLEET] @theDay = @today,@COUNTRY = NULL, @fleetPlanID = 1
	
	--SELECT @entity	= '[Import].[Update_CMS_Forecast_OPERATIONAL_FLEET] @fleetPlanID = 1'
	--SELECT @key1	= convert(varchar(20),@@spid)
	--SELECT @data1	= 'Execute_CMS_Procedures - Complete 9'
	--SELECT data2	= CONVERT(varchar(30),GETDATE(),113)
	--INSERT Trace (Entity, key1, key2 , data1, data2)
	--SELECT @entity, @key1, @key2 , @data1, @data2

	--EXEC [Import].[Update_CMS_Forecast_OPERATIONAL_FLEET] @theDay = @today,@COUNTRY = NULL, @fleetPlanID = 2
	
	--SELECT @entity	= '[Import].[Update_CMS_Forecast_OPERATIONAL_FLEET] @fleetPlanID = 2'
	--SELECT @key1	= convert(varchar(20),@@spid)
	--SELECT @data1	= 'Execute_CMS_Procedures - Complete 10'
	--SELECT data2	= CONVERT(varchar(30),GETDATE(),113)
	--INSERT Trace (Entity, key1, key2 , data1, data2)
	--SELECT @entity, @key1, @key2 , @data1, @data2
	
	--EXEC [Import].[Update_CMS_Forecast_OPERATIONAL_FLEET] @theDay = @today,@COUNTRY = NULL, @fleetPlanID = 3
	
	--SELECT @entity	= '[Import].[Update_CMS_Forecast_OPERATIONAL_FLEET] @fleetPlanID = 3'
	--SELECT @key1	= convert(varchar(20),@@spid)
	--SELECT @data1	= 'Execute_CMS_Procedures - Complete 11'
	--SELECT data2	= CONVERT(varchar(30),GETDATE(),113)
	--INSERT Trace (Entity, key1, key2 , data1, data2)
	--SELECT @entity, @key1, @key2 , @data1, @data2
	
	--EXEC [Import].[Update_CMS_Forecast_OPERATIONAL_FLEET] @theDay = @today,@COUNTRY = NULL, @fleetPlanID = 4
	
	--SELECT @entity	= '[Import].[Update_CMS_Forecast_OPERATIONAL_FLEET] @fleetPlanID = 4'
	--SELECT @key1	= convert(varchar(20),@@spid)
	--SELECT @data1	= 'Execute_CMS_Procedures - Complete 12'
	--SELECT data2	= CONVERT(varchar(30),GETDATE(),113)
	--INSERT Trace (Entity, key1, key2 , data1, data2)
	--SELECT @entity, @key1, @key2 , @data1, @data2
	
	
	EXEC [dbo].[FleetSizeForecastGenerate] 
	SELECT @entity	= '[dbo].[FleetSizeForecastGenerate]'
	SELECT @key1	= convert(varchar(20),@@spid)
	SELECT @data1	= 'Execute_CMS_Procedures - Complete 9,10,11 and 12'
	SELECT data2	= CONVERT(varchar(30),GETDATE(),113)
	INSERT Trace (Entity, key1, key2 , data1, data2)
	SELECT @entity, @key1, @key2 , @data1, @data2
	
	
	EXEC [Import].[Update_CMS_Forecast_ONRENT_LY]

    SELECT @entity	= '[Import].[Update_CMS_Forecast_ONRENT_LY]'
	SELECT @key1	= convert(varchar(20),@@spid)
	SELECT @data1	= 'Execute_CMS_Procedures - Complete 13'
	SELECT data2	= CONVERT(varchar(30),GETDATE(),113)
	INSERT Trace (Entity, key1, key2 , data1, data2)
	SELECT @entity, @key1, @key2 , @data1, @data2
   
    
END