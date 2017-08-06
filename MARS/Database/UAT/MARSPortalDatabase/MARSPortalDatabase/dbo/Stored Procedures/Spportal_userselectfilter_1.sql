
-- =============================================
-- Author:		Damien Connaghan
-- Create date: 7.7.2014
-- Description:	Selects all users that match filter value
-- =============================================
CREATE PROCEDURE [dbo].[Spportal_userselectfilter] @searchValue VARCHAR(32)
AS
  BEGIN
     
      SET NOCOUNT ON;

      SELECT racfId,
             [name],
             dbo.Fnmarsuserroles(racfid) AS 'roles'
      FROM   MARS_Users
      WHERE  racfId LIKE '%' + @searchValue+'%'
              OR NAME LIKE  '%' + @searchValue+'%'
  END

  --exec [dbo].[Spportal_userselectfilter] @searchValue = 'irmz01'