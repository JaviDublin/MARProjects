-- =============================================
-- Author:		Javier
-- Create date: August 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Select_Fleet_Serial_ByCountry]
	
		@country VARCHAR(2)
		
AS
BEGIN
	
	SET NOCOUNT ON;
	
	SELECT DISTINCT
		Serial 
	FROM
		FLEET_EUROPE_ACTUAL
	WHERE	
		COUNTRY = @country

    
END