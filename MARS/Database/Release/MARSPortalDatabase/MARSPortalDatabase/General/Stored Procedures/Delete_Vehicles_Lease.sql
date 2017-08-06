
-- =============================================
-- Author:		Javier
-- Create date: August 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Delete_Vehicles_Lease]
	
	@serial VARCHAR(25)
	
AS
BEGIN
	
	SET NOCOUNT ON;

BEGIN TRY
	
	DELETE FROM [General].[VEHICLE_LEASE]
	WHERE Serial = @serial
	
	SELECT 0
	
END TRY
BEGIN CATCH
	
	SELECT -1

END CATCH

    
END