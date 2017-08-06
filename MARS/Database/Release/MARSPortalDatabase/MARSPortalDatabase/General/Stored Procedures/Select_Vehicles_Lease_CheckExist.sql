
-- =============================================
-- Author:		Javier
-- Create date: August 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Select_Vehicles_Lease_CheckExist]
	
		@serial VARCHAR(25)
	
AS
BEGIN
	
	SET NOCOUNT ON;
	IF NOT EXISTS(SELECT Serial FROM [General].VEHICLE_LEASE where Serial = @serial)	
	BEGIN
		SELECT 0
	END
	ELSE
		SELECT -1


    
END