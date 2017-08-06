
-- =============================================
-- Author:		Javier
-- Create date: August 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Insert_Vehicles_Lease]
	
		@serial			VARCHAR(25),
		@country_owner	VARCHAR(2),
		@country_rent	VARCHAR(2),
		@start_date		DATETIME = NULL
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @plate VARCHAR(25) , @unit VARCHAR(25) , @modelDescription VARCHAR(255), @counter SMALLINT
	
	SET @plate				= (SELECT TOP 1 LICENSE	FROM FLEET_EUROPE_ACTUAL		WHERE SERIAL = @serial)
	
	SET @unit				= (SELECT TOP 1 UNIT	FROM FLEET_EUROPE_ACTUAL		WHERE SERIAL = @serial)
	
	SET @modelDescription	= (SELECT TOP 1 MODDESC FROM FLEET_EUROPE_ACTUAL		WHERE SERIAL = @serial)
	
	SET @counter			= (SELECT COUNT(*)		FROM [General].VEHICLE_LEASE	WHERE Serial = @serial)
	
	IF @counter = 0
	BEGIN
		INSERT INTO [General].[VEHICLE_LEASE] 
		(
			Serial , Plate , Unit , ModelDescription , Country_Owner , Country_Rent, StartDate 
		)
		VALUES
		(
			@serial , @plate , @unit , @modelDescription , @country_owner , @country_rent , 
			ISNULL(@start_date,DATEADD(d, 0, DATEDIFF(d, 0, GETDATE())))
		)
	END
	ELSE
	BEGIN
		UPDATE [General].[VEHICLE_LEASE]  SET
			Country_Owner		= @country_owner	,
			Country_Rent		= @country_rent		,
			StartDate			= ISNULL(@start_date,DATEADD(d, 0, DATEDIFF(d, 0, GETDATE()))),
			ModelDescription	= @modelDescription
		WHERE
			Serial = @serial
	END
	

    
END