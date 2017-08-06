CREATE procedure [dbo].[proc_Controls_MenuSelectRoles]
(
	@GlobalID VARCHAR(20)=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
		BEGIN	
			--Select the menu with no permissions required plus allowed management roles
			SELECT DISTINCT c.ControlID AS 'id', c.parentid, c.Url AS 'url', c.Name As 'title', c.position,	-- uc.roleId,
				CASE 
					WHEN (c.ImageUrl IS NOT NULL) THEN c.[description] + '|' + c.ImageUrl
					ELSE c.[description] END AS 'description'
			FROM
				Controls c
			LEFT JOIN dbo.MARS_UsersControls uc ON uc.ControlUrl=c.ControlUrl
			WHERE 
				c.isActive =1 AND c.menuEnabled=1 AND ((uc.roleId in 
				(Select distinct roleID FROM dbo.MARS_UsersInRoles WHERE userId=@GlobalID))
				 OR (ParentId IS NULL )
				 OR (c.PermissionsEnabled = 0))
				 
				
			ORDER BY c.position	
		END	
END