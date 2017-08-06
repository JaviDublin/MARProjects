CREATE PROCEDURE [Settings].[RibbonMenuSelect] (@employeeId varchar(10)=NULL)
AS
  BEGIN
  declare @MenuIdsAllowed table
  (
	MenuId int
  )

  insert into @MenuIdsAllowed
  select 1
--  SELECT c.MenuId AS 'id',
--        c.parentid,
--        c.Url,
--        c.Title  AS 'title',
--        CASE
--            WHEN ( c.iconUrl IS NOT NULL ) THEN c.[description] + '|' + c.IconUrl
--            ELSE c.[description]
--        END      AS 'description'
--FROM   Settings.RibbonMenu c
--WHERE  c.[Enabled] = 1

--ORDER  BY c.parentId,
--            c.position;

  if((select count(*) from MarsUser mu where mu.EmployeeId = @employeeId) > 0)
  begin
		insert into @MenuIdsAllowed
    	SELECT distinct rm.MenuId
		from [Settings].[RibbonMenu] rm
		join [MarsUserRoleMenuAccess] ma on rm.UrlId = ma.UrlId
		join MarsUserRole ur on ma.MarsUserRoleId = ur.MarsUserRoleId
		join MarsUserUserRole muur on ur.MarsUserRoleId = muur.MarsUserRoleId
		join MarsUser mu on muur.MarsUserId = mu.MarsUserId
		where mu.EmployeeId = @employeeId
  end
  else
  begin 
		insert into @MenuIdsAllowed
		SELECT distinct rm.MenuId
		from [Settings].[RibbonMenu] rm
		where rm.MenuId = 1
		--join [MarsUserRoleMenuAccess] ma on rm.UrlId = ma.UrlId
		--join MarsUserRole ur on ma.MarsUserRoleId = ur.MarsUserRoleId
		--where ur.BaseAccess = 1
  end


        
SELECT c.MenuId AS 'id',
        c.parentid,
        c.Url,
        c.Title  AS 'title',
        CASE
            WHEN ( c.iconUrl IS NOT NULL ) THEN c.[description] + '|' + c.IconUrl
            ELSE c.[description]
        END      AS 'description'
FROM   Settings.RibbonMenu c
WHERE  c.[Enabled] = 1
        AND c.MenuId IN (
							select menuId from @MenuIdsAllowed
						)
ORDER  BY c.parentId,
            c.position;
        
      
      
  END