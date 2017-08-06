CREATE procedure [dbo].[proc_MarsV2ManagementCountriesGetAllByUser] 
	
	@User varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT C.country, C.country_description 
	FROM COUNTRIES C
	INNER JOIN dbo.MARS_UsersInRoles UR on UR.country = C.country
	INNER JOIN dbo.MARS_Roles MR on MR.id = UR.roleId
	WHERE C.active=1 
	AND UR.userId = @User
	AND MR.roleName = 'Sizing Tool Country Coordinator'
END