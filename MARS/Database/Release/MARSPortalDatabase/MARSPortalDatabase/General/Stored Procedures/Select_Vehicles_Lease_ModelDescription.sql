
-- =============================================
-- Author:		Javier
-- Create date: August 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Select_Vehicles_Lease_ModelDescription]
	
		@country_owner		VARCHAR(2)	= NULL, 
		@country_rent		VARCHAR(2)  = NULL, 
		@start_date			DATETIME	= NULL
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT DISTINCT 
		ModelDescription 
	FROM 
		[General].[VEHICLE_LEASE]
	WHERE 
		(Country_Owner = ISNULL(@country_owner,Country_Owner))
	AND
		(Country_Rent = ISNULL(@country_rent,Country_Rent))
	AND
		(StartDate >= ISNULL(@start_date,StartDate))
    
END