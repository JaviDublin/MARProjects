
-- =============================================
-- Author:		Javier
-- Create date: August 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Delete_Vehicles_Lease_Actual]
	
AS
BEGIN
	
	SET NOCOUNT ON;

    DELETE FROM [General].[VEHICLE_LEASE] 
    WHERE 
		Serial NOT IN (SELECT Serial FROM FLEET_EUROPE_ACTUAL)
END