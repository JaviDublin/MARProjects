-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spSSISCMSForecastHistoryUpdate8weeksForecast]

	@theDay datetime
	
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
		SELECT UC.REP_DATE, UC.COUNTRY, UC.CMS_LOCATION_GROUP_ID, UC.CAR_CLASS_ID
			, C.[CW0], C.[CW1], C.[CW2], C.[CW3], C.[CW4], C.[CW5], C.[CW6], C.[CW7], C.[CW8] 
			, UC.[UCW0], UC.[UCW1], UC.[UCW2], UC.[UCW3], UC.[UCW4], UC.[UCW5], UC.[UCW6], UC.[UCW7], UC.[UCW8] 
		FROM
		(
			-- 1. Constrained
			SELECT @theDay AS [REP_DATE], COUNTRY, CMS_LOCATION_GROUP_ID, CAR_CLASS_ID
					, P.[0] AS [CW0], P.[1] AS [CW1], P.[2] AS [CW2], P.[3] AS [CW3], P.[4] AS [CW4], P.[5] AS [CW5], P.[6] AS [CW6], P.[7] AS [CW7], P.[8] AS [CW8]		
			FROM (
				SELECT FLOOR(DATEDIFF(day, @theDay, FC.REP_DATE) / 7) AS [Week]
						, FC.COUNTRY, FC.CMS_LOCATION_GROUP_ID, FC.CAR_CLASS_ID, FC.CONSTRAINED--, FC.UNCONSTRAINED
				FROM [MARS_CMS_FORECAST] AS FC
				WHERE (FC.REP_DATE = @theDay OR (DATEDIFF(day, @theDay, FC.REP_DATE) % 7) = 0)
						AND (FC.REP_DATE <= DATEADD(d, 56, @theDay))	
			) AS S
				PIVOT( AVG(CONSTRAINED) FOR [Week] IN ([0],[1],[2],[3],[4],[5],[6],[7],[8])	) AS P
		) AS C INNER JOIN
		(
			-- 2. UnConstrained
			SELECT @theDay AS [REP_DATE], COUNTRY, CMS_LOCATION_GROUP_ID, CAR_CLASS_ID
					, P.[0] AS [UCW0], P.[1] AS [UCW1], P.[2] AS [UCW2], P.[3] AS [UCW3], P.[4] AS [UCW4], P.[5] AS [UCW5], P.[6] AS [UCW6], P.[7] AS [UCW7], P.[8] AS [UCW8]		
			FROM (
				SELECT FLOOR(DATEDIFF(day, @theDay, FC.REP_DATE) / 7) AS [Week]
						, FC.COUNTRY, FC.CMS_LOCATION_GROUP_ID, FC.CAR_CLASS_ID, FC.UNCONSTRAINED
				FROM [MARS_CMS_FORECAST] AS FC
				WHERE (FC.REP_DATE = @theDay OR (DATEDIFF(day, @theDay, FC.REP_DATE) % 7) = 0)
						AND (FC.REP_DATE <= DATEADD(d, 56, @theDay))	
			) AS S
				PIVOT( AVG(UNCONSTRAINED) FOR [Week] IN ([0],[1],[2],[3],[4],[5],[6],[7],[8]) ) AS P
		) AS UC ON C.REP_DATE = UC.REP_DATE AND C.CMS_LOCATION_GROUP_ID = UC.CMS_LOCATION_GROUP_ID
				AND C.CAR_CLASS_ID = UC.CAR_CLASS_ID
	) AS [SOURCE]
	ON ([TARGET].REP_DATE = [SOURCE].REP_DATE AND [TARGET].CMS_LOCATION_GROUP_ID = [SOURCE].CMS_LOCATION_GROUP_ID 
		AND [TARGET].CAR_CLASS_ID = [SOURCE].CAR_CLASS_ID)

	WHEN MATCHED THEN
	UPDATE SET --[TARGET].FROZEN_CMS_CONSTRAINED = NULL	-- FROZEN will be updated separately
			--, [TARGET].FROZEN_CMS_UNCONSTRAINED = NULL	-- FROZEN will be updated separately
			  [TARGET].CMS_CONSTRAINED =	[SOURCE].[CW0]
			, [TARGET].CMS_UNCONSTRAINED =	[SOURCE].[UCW0]
			, [TARGET].CMS_CONSTRAINED_WK1 =	[SOURCE].[CW1]
			, [TARGET].CMS_UNCONSTRAINED_WK1 =	[SOURCE].[UCW1]
			, [TARGET].CMS_CONSTRAINED_WK2 =	[SOURCE].[CW2]
			, [TARGET].CMS_UNCONSTRAINED_WK2 =	[SOURCE].[UCW2]
			, [TARGET].CMS_CONSTRAINED_WK3 =	[SOURCE].[CW3]
			, [TARGET].CMS_UNCONSTRAINED_WK3 =	[SOURCE].[UCW3]
			, [TARGET].CMS_CONSTRAINED_WK4 =	[SOURCE].[CW4]
			, [TARGET].CMS_UNCONSTRAINED_WK4 =	[SOURCE].[UCW4]
			, [TARGET].CMS_CONSTRAINED_WK5 =	[SOURCE].[CW5]
			, [TARGET].CMS_UNCONSTRAINED_WK5 =	[SOURCE].[UCW5]
			, [TARGET].CMS_CONSTRAINED_WK6 =	[SOURCE].[CW6]
			, [TARGET].CMS_UNCONSTRAINED_WK6 =	[SOURCE].[UCW6]
			, [TARGET].CMS_CONSTRAINED_WK7 =	[SOURCE].[CW7]
			, [TARGET].CMS_UNCONSTRAINED_WK7 =	[SOURCE].[UCW7]
			, [TARGET].CMS_CONSTRAINED_WK8 =	[SOURCE].[CW8]
			, [TARGET].CMS_UNCONSTRAINED_WK8 =	[SOURCE].[UCW8]
	;
		


END