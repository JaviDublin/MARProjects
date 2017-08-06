

CREATE FUNCTION [dbo].[fnGetFleetRACTTLForOwnarea]
(
	@ownarea varchar(10)
)
RETURNS bit
AS
BEGIN
	DECLARE @isCarsales bit
	DECLARE @isLicensee bit
	SELECT @isCarsales = carsales FROM AREACODES WHERE ownarea=@ownarea
	SELECT @isLicensee = licensee FROM AREACODES WHERE ownarea=@ownarea

	RETURN
		(
			SELECT
				CASE
					WHEN
						(@isCarsales = 0)
						AND
						(@isLicensee = 0)
					THEN
						1
					ELSE
						0
				END
		)
END