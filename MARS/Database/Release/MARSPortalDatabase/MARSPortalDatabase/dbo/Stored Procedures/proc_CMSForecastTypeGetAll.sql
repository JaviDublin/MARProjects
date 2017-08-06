CREATE PROCEDURE [dbo].[proc_CMSForecastTypeGetAll]
	
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
		FCTypeID, FCType 
	FROM 
		[dbo].[CMS_ForecastTypes]
		
END