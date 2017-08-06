-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_RolesSelectForUser] 
(
	@racfid VARCHAR(10)=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

  
	SELECT 
	CASE WHEN country IS NULL THEN CONVERT(VARCHAR(50),ur.roleId) 
	ELSE CONVERT(VARCHAR(50),ur.roleId) + '|' + ur.country 
	END AS roleId,
	r.roleName
	FROM dbo.MARS_UsersInRoles ur
	LEFT JOIN dbo.MARS_Roles r ON r.id = ur.roleId
	WHERE ur.userId =@racfid
	ORDER BY roleId

END