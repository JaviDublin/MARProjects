CREATE PROCEDURE [Settings].[RibbonMenuSelect] (@userId UNIQUEIDENTIFIER=NULL)
AS
  BEGIN
      SET NOCOUNT ON;
      DECLARE @NONEACTIVEROLE INT = 1;
      IF @userId IN (SELECT ua.userId
                     FROM   MARS_UsersInRoles uir
                            JOIN Mars_UsersAccount ua
                              ON uir.userId = ua.racfId)
        BEGIN
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
                   AND c.MenuId IN (SELECT MenuId
                                    FROM   MARS_RoleRibbonMenu
                                    WHERE  roleid IN (SELECT roleId
                                                      FROM   MARS_UsersInRoles uir
                                                             INNER JOIN (SELECT racfId
                                                                         FROM   MARS_UsersAccount
                                                                         WHERE  userId = @userId) ua
                                                                     ON uir.userId = ua.racfId)
                                           AND isActive = 1)
            ORDER  BY c.parentId,
                      c.position;
        END
      ELSE
        BEGIN
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
                   AND c.MenuId IN (SELECT MenuId
                                    FROM   MARS_RoleRibbonMenu rrm
                                    WHERE  roleid = 7
                                           AND rrm.isActive = 1)
            ORDER  BY c.parentId,
                      c.position;
        END
  END