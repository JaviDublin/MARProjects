
-- ==========================================================================================
-- Description:	Check to see if MARS has completed Daily Import and CMS Forecast
-- ==========================================================================================
CREATE PROCEDURE [dbo].[CheckMarsReadyForLogisticsDashboardData]
(
	@ResultCode	INT OUTPUT
)
AS  
BEGIN 

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Set Result Code = failed
	SET @ResultCode = -1

	DECLARE @Count INT
	SET @Count=(
	SELECT COUNT(*)
	FROM [bcs].[FileProcess]
	WHERE 
		([z_inserted] > convert(DATE, GETDATE())
	AND ([Entity] = 'Mars_Daily' OR [Entity] = 'CMS_Forecast')
	AND [Status] = 'Complete'))

	--Check to see we have two records
	IF (@Count =2)
		--Set Result Code = Success
		SET @ResultCode =0

 
END