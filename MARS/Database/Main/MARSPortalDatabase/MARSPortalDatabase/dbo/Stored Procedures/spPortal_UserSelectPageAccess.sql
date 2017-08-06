-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_UserSelectPageAccess] 
(
	@userRACFID			VARCHAR(50)
)
		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

SELECT
		UIR.roleId AS roleId, 
		UIR.country AS country , 
		pageAccess 
FROM 
		MARS_UsersInRoles UIR 
		INNER JOIN MARS_Roles R ON UIR.roleId = R.id 
WHERE	UIR.userID = @userRACFID

END