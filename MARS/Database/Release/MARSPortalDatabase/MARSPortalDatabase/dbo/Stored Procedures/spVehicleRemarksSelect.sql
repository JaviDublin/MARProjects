-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spVehicleRemarksSelect] 
	
	@serial VARCHAR(25)
AS
BEGIN
	
	SET NOCOUNT ON;

    -- Insert statements for procedure here	
	SELECT 
		R.REMARK AS 'remark',
		R.EXPECTEDRESOLUTIONDATE AS 'nextOnRentDate',
		FEA.VC AS 'group', 
		FEA.MODEL AS 'modelcode', 
		FEA.MODDESC AS 'model', 
		FEA.OWNAREA AS 'ownarea', 
		FEA.DRVNAME AS 'driverName', 
		FEA.CAPDATE AS 'blockdate', 		
		FEA.LICENSE AS 'license', 
		FEA.UNIT AS 'unit', 
		FEA.SERIAL AS 'vehicleIdentNbr', 
		CAST(ISNULL(FEA.LSTMLG,0)AS INT) AS 'kilometers', 
		FEA.IDATE AS 'regDate', 	
		FEA.LSTWWD AS 'lstwwd', 
		FEA.LSTDATE AS 'lstdate', 
		FEA.DUEWWD AS 'duewwd', 
		FEA.DUEDATE AS 'duedate', 
		FEA.MOVETYPE AS 'movetype', 
		FEA.LSTNO AS 'lastDocument', 
		FEA.OPERSTAT AS 'operstat', 
		FEA.DAYSREV AS 'nonRev', 
		FEA.BDDAYS AS 'bdDays', 
		FEA.PREVWWD AS 'prevwwd',
		FEA.MMDAYS AS 'mmDays',
		FEA.RC AS 'groupCharged',		
		FEA.LSTTIME AS 'lsttime',
		FEA.DUETIME AS 'duetime',		
		FEA.CARHOLD1 AS 'carhold'

	FROM 
		FLEET_EUROPE_ACTUAL AS FEA 
	LEFT JOIN VEHICLE_REMARKS AS R ON R.SERIAL = FEA.SERIAL 
	WHERE 
		R.SERIAL = @serial 
	OR 
		FEA.SERIAL = @serial

END