
-- =============================================
-- Author:		Javier
-- Create date: August 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Update_Vehicles_Lease]
	
		@serial			VARCHAR(25),
		@country_owner	VARCHAR(2),
		@country_rent	VARCHAR(2),
		@start_date		DATETIME = NULL
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	UPDATE [General].[VEHICLE_LEASE]  SET
		Country_Owner	= @country_owner	,
		Country_Rent	= @country_rent		,
		StartDate		= @start_date		
	WHERE
		Serial = @serial
    
END