-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Import].[Update_CMS_Forecast_History_ADJUSTMENT]
	
	--@theDay DATETIME
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	declare @theDay DATETIME = DATEADD(dd, DATEDIFF(dd, 0, GETDATE() - 1), 0) 
	
	--UPDATE CURRENT_ONRENT in CMS_FORECAST table	
	MERGE MARS_CMS_FORECAST_HISTORY AS [TARGET]
	USING	
	(
		-- ON_RENT aggregated by Location_Group level on the day
		SELECT 
			FCA.REP_DATE, 
			FCA.COUNTRY, 
			FCA.CMS_LOCATION_GROUP_ID, 
			FCA.CAR_CLASS_ID, 
			FCA.ADJUSTMENT_TD, 
			FCA.ADJUSTMENT_BU1, 
			FCA.ADJUSTMENT_BU2, 
			FCA.ADJUSTMENT_RC
		FROM MARS_CMS_FORECAST_ADJUSTMENT FCA	
		WHERE REP_DATE = @theDay		
		
	) AS [SOURCE]
	ON 
	(
		[TARGET].REP_DATE				= [SOURCE].REP_DATE 
	AND [TARGET].CMS_LOCATION_GROUP_ID	= [SOURCE].CMS_LOCATION_GROUP_ID 
	AND [TARGET].CAR_CLASS_ID			= [SOURCE].CAR_CLASS_ID
	AND [TARGET].COUNTRY				= [SOURCE].COUNTRY 
	)
	
	-- update ONRENT_AT
	WHEN MATCHED THEN
	UPDATE SET 
		[TARGET].ADJUSTMENT_TD	= [SOURCE].ADJUSTMENT_TD	, 
		[TARGET].ADJUSTMENT_BU1 = [SOURCE].ADJUSTMENT_BU1	, 
		[TARGET].ADJUSTMENT_BU2 = [SOURCE].ADJUSTMENT_BU2	, 
		[TARGET].ADJUSTMENT_RC	= [SOURCE].ADJUSTMENT_RC
	;
	
	
	-- Once archieved, clean entries older than a week (could be 'older than today', but just in case)
	DELETE FROM MARS_CMS_FORECAST_ADJUSTMENT WHERE REP_DATE <=  dateadd(d, -7, @theDay)
	
	DELETE FROM MARS_CMS_FleetPlanDetails WHERE targetDate <=  dateadd(d, -7, @theDay)
	

   
END