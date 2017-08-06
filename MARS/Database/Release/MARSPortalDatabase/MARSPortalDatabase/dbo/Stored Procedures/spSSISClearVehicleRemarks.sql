-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spSSISClearVehicleRemarks]
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM VEHICLE_REMARKS
	WHERE 
	SERIAL NOT IN (SELECT SERIAL FROM FLEET_EUROPE_ACTUAL)
	OR (REMARK IS NULL AND EXPECTEDRESOLUTIONDATE IS NULL)

-- 2dn method - much slower (2:30 mins)
--	SELECT * FROM VEHICLE_REMARKS AS VR
--	LEFT JOIN FLEET_EUROPE_ACTUAL AS FEA ON FEA.SERIAL = VR.SERIAL
--	WHERE FEA.SERIAL IS NULL OR VR.REMARK IS NULL
	
END