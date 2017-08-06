








-- =============================================
-- Author:		Anthony McClorey

-- =============================================
CREATE FUNCTION [dbo].[fnMARSUserRoles]
(
	@racfid		VARCHAR(50) = NULL
)
RETURNS VARCHAR(8000)
AS
BEGIN
		DECLARE @roleNames VARCHAR(8000)

		(SELECT @roleNames = 
				(COALESCE(@roleNames + ', ', '') + 
				(CASE WHEN country IS NULL 
						THEN roleName 
						ELSE '(' + country + ') ' + roleName 
						END ))
		FROM dbo.vw_MARS_User_Roles 
		WHERE racfid =@racfid)
		
		RETURN @roleNames
END