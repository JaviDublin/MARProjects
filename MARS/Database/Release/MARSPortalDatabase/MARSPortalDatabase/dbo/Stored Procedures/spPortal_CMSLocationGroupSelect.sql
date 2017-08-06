-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE procedure [dbo].[spPortal_CMSLocationGroupSelect]   
(  
 @country   VARCHAR(50) =NULL,  
 @cms_pool_id  INT=NULL,  
 @sortExpression  VARCHAR(50) =NULL,  
 @startRowIndex  INT=NULL,  
 @maximumRows  INT=NULL  
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
  
 DECLARE @QUERYTABLE TABLE  
 (  
  rowId      INT IDENTITY (1,1) NOT NULL,  
  cms_location_group_id  INT NULL,	--cms_location_group_code  VARCHAR(10) NULL,  
  cms_location_group_code_dw VARCHAR(10) NULL,  
  cms_location_group   VARCHAR(50) NULL,  
  cms_pool_id     INT NULL,  
  cms_pool     VARCHAR(50) NULL,  
  country      VARCHAR(10) NULL  
 )  
  
 INSERT INTO @QUERYTABLE  
  (cms_location_group_id, cms_location_group_code_dw, cms_location_group,   
   cms_pool_id, cms_pool, country)  
 SELECT  
  lg.cms_location_group_id, lg.cms_location_group_code_dw, lg.cms_location_group,   
  lg.cms_pool_id,  p.cms_pool, p.country  
 FROM CMS_Location_Groups lg  
  INNER JOIN CMS_Pools p on lg.cms_pool_id = p.cms_pool_id    
 WHERE   
  (p.country = ISNULL(@country,p.country)) AND  
  (p.cms_pool_id = ISNULL(@cms_pool_id,p.cms_pool_id))  
  
 --Select Records  
 INSERT INTO @PAGING (rowId) SELECT rowId FROM @QUERYTABLE  
 ORDER BY  
 CASE WHEN @sortExpression ='cms_location_group_id' THEN cms_location_group_id END ASC,  
 CASE WHEN @sortExpression ='cms_location_group_id DESC' THEN cms_location_group_id END DESC,  
 CASE WHEN @sortExpression ='cms_location_group_code_dw' THEN cms_location_group_code_dw END ASC,  
 CASE WHEN @sortExpression ='cms_location_group_code_dw DESC' THEN cms_location_group_code_dw END DESC,  
 CASE WHEN @sortExpression ='cms_location_group' THEN cms_location_group END ASC,  
 CASE WHEN @sortExpression ='cms_location_group DESC' THEN cms_location_group END DESC,  
 CASE WHEN @sortExpression ='cms_pool' THEN cms_pool END ASC,  
 CASE WHEN @sortExpression ='cms_pool DESC' THEN cms_pool END DESC,  
 CASE WHEN @sortExpression ='country' THEN country END ASC,  
 CASE WHEN @sortExpression ='country DESC' THEN country END DESC  
  
 SELECT  
  cms_location_group_id, cms_location_group_code_dw, cms_location_group,   
   cms_pool_id, cms_pool, country  
 FROM @QUERYTABLE q  
 INNER JOIN @PAGING p ON p.rowId = q.rowId   
 WHERE (p.pageIndex BETWEEN @startRowIndex AND @maximumRows)  
 ORDER BY p.pageIndex;  
  
 --Select total row count  
 SELECT COUNT(*) AS totalCount FROM @QUERYTABLE;  
  
END