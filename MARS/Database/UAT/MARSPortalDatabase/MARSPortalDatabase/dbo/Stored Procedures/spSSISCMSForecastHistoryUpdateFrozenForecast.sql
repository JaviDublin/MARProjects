-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spSSISCMSForecastHistoryUpdateFrozenForecast]

	@country char(2),
	@dateFrom datetime, 
	@dateTo datetime
	
AS
BEGIN
--	=========================================================================
--	Update 8 weeks Constrained/Unconstrained forecast for the day
--	==========================================================================

	SET NOCOUNT ON;

	--DECLARE @theDay datetime
	--SET @theDay = DATEADD(d, DATEDIFF(dd, 0, GETDATE()), 0)
	
	-- MERGE Forecast History table with 8 weeks(56 days) of Forecast table
	MERGE [MARS_CMS_FORECAST_HISTORY] AS [TARGET]
	USING 
	(		
		SELECT FC.REP_DATE, FC.COUNTRY, FC.CMS_LOCATION_GROUP_ID, FC.CAR_CLASS_ID, FC.CONSTRAINED, FC.UNCONSTRAINED
		FROM [MARS_CMS_FORECAST] AS FC
		WHERE ((FC.COUNTRY = @country) AND (FC.REP_DATE BETWEEN @dateFrom AND @dateTo))	

	) AS [SOURCE]
	ON ([TARGET].REP_DATE = [SOURCE].REP_DATE AND [TARGET].CMS_LOCATION_GROUP_ID = [SOURCE].CMS_LOCATION_GROUP_ID 
		AND [TARGET].CAR_CLASS_ID = [SOURCE].CAR_CLASS_ID)

	WHEN MATCHED  THEN
	UPDATE SET [TARGET].CMS_CONSTRAINED_FROZEN = [SOURCE].CONSTRAINED
			, [TARGET].CMS_UNCONSTRAINED_FROZEN = [SOURCE].UNCONSTRAINED 
	;
	
	-------------------------------------

	
	SELECT @@ROWCOUNT;
	
END

-- EXEC [spSSISCMSForecastHistoryUpdateFrozenForecast] 'IT', '23/may/2011', '29/may/2011'