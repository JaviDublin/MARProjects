CREATE procedure [dbo].[proc_MarsCountriesGetAll] 
		
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT DISTINCT country, country_description FROM COUNTRIES WHERE active=1
END