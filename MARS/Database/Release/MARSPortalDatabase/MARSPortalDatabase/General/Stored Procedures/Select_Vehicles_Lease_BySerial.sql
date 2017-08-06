
-- =============================================
-- Author:		Javier
-- Create date: August 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Select_Vehicles_Lease_BySerial]
	
		@serial VARCHAR(25)
		
AS
BEGIN
	
	SET NOCOUNT ON;

   SELECT
		 Serial , Plate , Unit , ModelDescription , Country_Owner , Country_Rent , StartDate 
	FROM
		[General].[VEHICLE_LEASE]
	WHERE
		Serial = @serial
END