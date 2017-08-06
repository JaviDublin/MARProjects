-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spSSISCMSForecastHistoryUpdate8weeksForecastMultipleDays]

	@theDay datetime
	
AS
BEGIN
--	=========================================================================
--	Update 8 weeks Constrained/Unconstrained forecast for next 56 days from theDay
--	==========================================================================

	SET NOCOUNT ON;
	
	-- Added the 26/03/2012. Requested by Uwe and devloped by gavin
	--======================================================================
	
	EXEC [dbo].[forecastHistory8Week] @theDay
	

	-- Removed 26/03/2012
	---======================================================================
	--DECLARE @count int, @current_count int, @current_date datetime
	
	--set @count = 56			-- up to 56 days
	--set @current_count = 0
	--set @current_date = @theDay
	
	
	--WHILE(@current_count < @count)
	--BEGIN
		
	
	--	EXEC [dbo].[spSSISCMSForecastHistoryUpdate8weeksForecast] @current_date
		
	--	SET @current_count = @current_count + 1	
	--	SET @current_date = DATEADD(d, @current_count, @theDay)
	--END	
	

END

-- exec [spSSISCMSForecastHistoryUpdate8weeksForecastMultipleDays] '25/jul/2011'