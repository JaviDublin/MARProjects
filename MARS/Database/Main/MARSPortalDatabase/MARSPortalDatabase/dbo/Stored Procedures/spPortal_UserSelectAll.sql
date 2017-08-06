--sp_helptext spPortal_UserSelectAll

  
    
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE procedure [dbo].[spPortal_UserSelectAll]   
(  
 @sortExpression VARCHAR(50) =NULL,  
 @startRowIndex INT=NULL,  
 @maximumRows INT=NULL  
)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
  
 DECLARE @PAGING TABLE  
 (  
  pageIndex  INT IDENTITY (1,1) NOT NULL,  
  rowId   INT NULL  
 )  
  
 DECLARE @USERS TABLE  
 (  
  rowId  INT IDENTITY (1,1) NOT NULL,  
  racfId  VARCHAR(50) NULL,  
  [name]  VARCHAR(50) NULL,  
  roles  VARCHAR(3000) NULL  
 )  
  
 INSERT INTO @USERS  
 (  
  racfid,  
  [name],  
  roles  
 )  
 SELECT racfId, [name], dbo.fnMARSUserRoles(racfid)  
 FROM MARS_Users  
  
 --Select Results  
 INSERT INTO @PAGING (rowId) SELECT rowId FROM @USERS  
 ORDER BY  
 CASE WHEN @sortExpression = 'racfId' THEN racfId END ASC,  
 CASE WHEN @sortExpression = 'racfId DESC' THEN racfId END DESC,  
 CASE WHEN @sortExpression = 'name' THEN [name] END ASC,  
 CASE WHEN @sortExpression = 'name DESC' THEN [name] END DESC,  
 CASE WHEN @sortExpression = 'roles' THEN roles END ASC,  
 CASE WHEN @sortExpression = 'roles DESC' THEN roles END DESC  
   
 SELECT   
  u.racfId, u.[name], u.roles  
 FROM @USERS u  
 INNER JOIN @PAGING p ON p.rowId = u.rowId   
 WHERE (p.pageIndex BETWEEN @startRowIndex AND @maximumRows)  
 ORDER BY p.pageIndex;  
  
 --Select total row count  
 SELECT COUNT(*) AS totalCount FROM @USERS;  
  
END