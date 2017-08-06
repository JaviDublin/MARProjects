CREATE procedure [dbo].[proc_UsersInRolesSelectAll] 
	@racfId		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- Insert statements for procedure here

	SELECT     
		racfId, [name], country, roleName, description, pageAccess, id
	FROM   dbo.vw_MARS_User_Roles

	WHERE racfId = @racfId
	END