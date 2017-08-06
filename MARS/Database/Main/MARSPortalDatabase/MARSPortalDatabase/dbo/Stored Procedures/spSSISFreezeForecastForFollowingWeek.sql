-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spSSISFreezeForecastForFollowingWeek]

	-- @theDay datetime	
AS
BEGIN
--	=========================================================================
--	Update 8 weeks Constrained/Unconstrained forecast for the day
--	==========================================================================

	SET NOCOUNT ON;

	declare @theDay datetime			
	-- set day in following week
	set @theDay = dateadd(day, 7, GETDATE()) 

	-- retrieve the corresponding calendar year, week and date range
	DECLARE @cYear int, @cWeek tinyint, @cDateFrom datetime, @cDateTo datetime
	DECLARE @Countries table ([id] int, [country] varchar(10))


	SET @cYear = (SELECT [year] FROM CalendarWeek WHERE @theDay BETWEEN dateFrom AND dateTo)
	SET @cWeek = (SELECT [week] FROM CalendarWeek WHERE @theDay BETWEEN dateFrom AND dateTo)
	SET @cDateFrom = (SELECT [dateFrom] FROM CalendarWeek WHERE @theDay BETWEEN dateFrom AND dateTo)
	SET @cDateTo = (SELECT [dateTo] FROM CalendarWeek WHERE @theDay BETWEEN dateFrom AND dateTo)


	-- Countries that have no frozen history for the week
	INSERT INTO @Countries
	SELECT ROW_NUMBER() OVER (ORDER BY C.country) as 'id', C.country
	FROM COUNTRIES C -- (SELECT country FROM COUNTRIES WHERE active = 1) AS C 
		LEFT JOIN MARS_CMS_FrozenZoneAcceptanceLog AS FZ ON C.country = FZ.country AND (FZ.[Year] = @cYear AND FZ.acceptedWeekNumber = @cWeek)
	WHERE C.active = 1 AND FZ.country IS NULL



	DECLARE @rowCount int, @curCountry varchar(10)
	SET @rowCount = 1

	WHILE(EXISTS(SELECT * FROM @Countries WHERE [id] = @rowCount))
	BEGIN
		-- select a country in @countries table
		SET @curCountry = (SELECT [country] FROM @Countries WHERE [id] = @rowCount)
		-- freeze forecast for the period
		EXEC dbo.spSSISCMSForecastHistoryUpdateFrozenForecast @curCountry, @cDateFrom, @cDateTo
		-- insert a log
		INSERT INTO MARS_CMS_FrozenZoneAcceptanceLog VALUES(@curCountry, @cYear, @cWeek, 'system', getdate())
		
		SET @rowCount = @rowCount + 1
	END
	
END

-- EXEC [spSSISFreezeForecastForFollowingWeek]