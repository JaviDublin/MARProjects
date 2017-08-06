

CREATE FUNCTION [dbo].[fnGetFleetLicenseeForOwnarea]
(
	@ownarea varchar(10)	
)
RETURNS bit
AS
BEGIN
	DECLARE @isLicensee bit
	IF 
		(
			((SELECT licensee FROM AREACODES WHERE ownarea=@ownarea) = 1)
		)
		BEGIN
			SET @isLicensee = 1
		END
	ELSE
		BEGIN
			SET @isLicensee = 0
		END
	
	RETURN	@isLicensee		

END