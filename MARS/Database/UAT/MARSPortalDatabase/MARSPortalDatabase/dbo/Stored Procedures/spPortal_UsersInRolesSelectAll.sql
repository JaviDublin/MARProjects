-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_UsersInRolesSelectAll] 
	@racfId		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- Insert statements for procedure here

	SELECT     
		racfId, [name], country, roleName, description, pageAccess
	FROM   dbo.vw_MARS_User_Roles

	WHERE racfId = @racfId

END