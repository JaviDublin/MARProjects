


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetWsNonracForOperstatAndOwnarea]
(
	@operstat varchar(10),
	@ownarea varchar(10)		
)
RETURNS int
AS
BEGIN
	DECLARE @isCarsales bit
	SELECT @isCarsales = carsales FROM AREACODES WHERE ownarea=@ownarea

	RETURN 
		(
			SELECT 
				CASE 
					WHEN 
						(@isCarsales = 1)
						AND
						(@operstat = 'WS')
					THEN 
						1					
					ELSE 
						0
				END
		)

END