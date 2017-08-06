-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spPortal_RolesSelectAll]
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
      -- Insert statements for procedure here
      DECLARE @ROLES TABLE
        (
           roleId   VARCHAR(100) NULL,
           roleName VARCHAR(100) NULL
        )
      INSERT INTO @ROLES
                  (roleId,
                   roleName)
      SELECT CASE
               WHEN ( r.id = 1
                       OR r.id = 7 ) THEN CONVERT(VARCHAR(10), r.id)
               ELSE CONVERT (VARCHAR(10), r.Id) + '|' + c.country
             END,
             r.roleName + CASE WHEN (r.id =1 OR r.id=7) THEN '' ELSE ' - ' + c.country END
      FROM   MARS_Roles r
             CROSS JOIN COUNTRIES c
      WHERE  c.active = 1
      
      SELECT roleId,
             roleName
      FROM   @ROLES
      where roleId <> '7'
      GROUP  BY roleId,
                roleName
      
      ORDER  BY rolename
  END 

--exec [Spportal_rolesselectall]