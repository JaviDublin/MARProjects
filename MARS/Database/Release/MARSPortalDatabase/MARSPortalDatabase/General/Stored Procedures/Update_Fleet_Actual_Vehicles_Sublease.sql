
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Update_Fleet_Actual_Vehicles_Sublease] 
	
AS
BEGIN
	
	SET NOCOUNT ON;

    
    UPDATE FLEET_EUROPE_ACTUAL SET
		COUNTRY = VS.Country_Rent
	FROM FLEET_EUROPE_ACTUAL FEA
	INNER JOIN [General].VEHICLE_LEASE AS VS ON
		FEA.SERIAL = VS.Serial 
	AND 
		FEA.COUNTRY = VS.Country_Owner
	AND 
		VS.StartDate <= DATEADD(dd, DATEDIFF(dd, 0, GETDATE()), 0) 
    
    
END