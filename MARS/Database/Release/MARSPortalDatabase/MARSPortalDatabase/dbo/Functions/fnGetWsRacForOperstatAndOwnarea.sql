



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetWsRacForOperstatAndOwnarea]
(
	@operstat varchar(10),
	@ownarea varchar(10)		
)
RETURNS int
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
						(@isLicensee = 0)
						AND
						(@isCarsales = 0)
						AND
						(@operstat = 'WS')
					THEN 
						1					
					ELSE 
						0
				END
		)

END