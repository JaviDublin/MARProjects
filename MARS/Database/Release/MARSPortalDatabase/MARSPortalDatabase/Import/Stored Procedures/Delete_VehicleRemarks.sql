-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Import].[Delete_VehicleRemarks]

AS
BEGIN

	SET NOCOUNT ON;

	DELETE FROM VEHICLE_REMARKS WHERE SERIAL in 
	(SELECT DISTINCT SERIAL FROM FLEET_EUROPE_ACTUAL WHERE ON_RENT = 1)
END