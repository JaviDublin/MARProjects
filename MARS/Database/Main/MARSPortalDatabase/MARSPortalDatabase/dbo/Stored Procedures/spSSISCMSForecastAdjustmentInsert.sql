-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spSSISCMSForecastAdjustmentInsert]

	@theDay datetime
	
AS
BEGIN
--	=========================================================================
--	Create data entries in MARS_CMS_FORECAST_ADJUSTMENT table based on MARS_CMS_FORECAST table
--	==========================================================================

	SET NOCOUNT ON;

	
	-- MERGE Forecast History table with 8 weeks(56 days) of Forecast table
	MERGE [MARS_CMS_FORECAST_ADJUSTMENT] AS [TARGET]
	--USING (SELECT * FROM [MARS_CMS_FORECAST] WHERE REP_DATE BETWEEN @theDay AND DATEADD(d, 56, @theDay)) AS [SOURCE]
	USING [MARS_CMS_FORECAST] AS [SOURCE]
	ON ([TARGET].REP_DATE = [SOURCE].REP_DATE AND [TARGET].CMS_LOCATION_GROUP_ID = [SOURCE].CMS_LOCATION_GROUP_ID 
		AND [TARGET].CAR_CLASS_ID = [SOURCE].CAR_CLASS_ID)

	-- 1. If not exists in Adjustment table, then INSERT
	WHEN NOT MATCHED BY TARGET AND ([SOURCE].REP_DATE BETWEEN @theDay AND DATEADD(d, 56, @theDay)) THEN
	INSERT ([REP_DATE],[COUNTRY],[CMS_LOCATION_GROUP_ID],[CAR_CLASS_ID]
			,[ADJUSTMENT_TD],[ADJUSTMENT_BU1],[ADJUSTMENT_BU2],[ADJUSTMENT_RC])
	VALUES ([SOURCE].REP_DATE, [SOURCE].COUNTRY, [SOURCE].CMS_LOCATION_GROUP_ID, [SOURCE].CAR_CLASS_ID
			, [SOURCE].CONSTRAINED, [SOURCE].CONSTRAINED, [SOURCE].CONSTRAINED, [SOURCE].CONSTRAINED)
			
	WHEN MATCHED AND ([SOURCE].REP_DATE > DATEADD(d, 56, @theDay)) THEN
	UPDATE SET [TARGET].[ADJUSTMENT_TD] = [SOURCE].CONSTRAINED, 
				[TARGET].[ADJUSTMENT_BU1] = [SOURCE].CONSTRAINED, 
				[TARGET].[ADJUSTMENT_BU2] = [SOURCE].CONSTRAINED, 
				[TARGET].[ADJUSTMENT_RC] = [SOURCE].CONSTRAINED
	;
	
END

-- EXEC [spSSISCMSForecastAdjustmentInsert] '21/jul/2011'