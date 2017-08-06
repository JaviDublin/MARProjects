-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE procedure [dbo].[spPortal_LocationSelect]   
(  
 @country     VARCHAR(50) =NULL,  
 @cms_location_group_id INT=NULL,  --@cms_location_group_code VARCHAR(50)=NULL,  
 @ops_area_id    INT = NULL,  
 @sortExpression    VARCHAR(50) =NULL,  
 @startRowIndex    INT=NULL,  
 @maximumRows    INT=NULL,  
 @_location as varchar(50) = null -- alteration by Gavin for MarsV3 19-4-12
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
  rowId     INT IDENTITY (1,1) NOT NULL,  
  location    VARCHAR(50) NULL,  
  location_dw    VARCHAR(50) NULL,  
  real_location_name  VARCHAR(50) NULL,  
  location_name   VARCHAR(50) NULL,  
  location_name_dw  VARCHAR(50) NULL,  
  active     BIT,  
  ap_dt_rr    VARCHAR(2) NULL,  
  cal      VARCHAR(1) NULL,  
  ops_area_id    INT NULL,  
  ops_area    VARCHAR(50) NULL,  
  cms_location_group_id INT NULL,  --cms_location_group_code VARCHAR(10) NULL,  
  cms_location_group  VARCHAR(50) NULL,  
  served_by_locn   VARCHAR(50) NULL,  
  turnaround_hours   INT NULL  
 )  
  
 INSERT INTO @QUERYTABLE  
  (location, location_dw, real_location_name, location_name, location_name_dw,  
   active, ap_dt_rr, cal, ops_area_id, ops_area, cms_location_group_id,   
    cms_location_group, served_by_locn, turnaround_hours)  
 SELECT   
  location, location_dw, real_location_name, location_name, location_name_dw,   
  active, ap_dt_rr, cal, ops_area_id,  ops_area,   
  cms_location_group_id, cms_location_group,  
  served_by_locn, turnaround_hours  
 FROM dbo.vw_MARS_Locations  
 WHERE   
  ((pCountry = ISNULL(@country,pCountry)) OR (rCountry = ISNULL(@country,rCountry))) AND  
  ((ops_area_id = ISNULL(@ops_area_id,ops_area_id))) AND  
  ((cms_location_group_id = ISNULL(@cms_location_group_id,cms_location_group_id))) 
  and ((@_location = location_name) or @_location is null) -- alteration by Gavin for MarsV3 19-4-12
     
  
 --Select Results  
 INSERT INTO @PAGING (rowId) SELECT rowId FROM @QUERYTABLE  
 ORDER BY  
 CASE WHEN @sortExpression ='location' THEN location END ASC,  
 CASE WHEN @sortExpression ='location DESC' THEN location END DESC,  
 CASE WHEN @sortExpression ='location_dw' THEN location_dw END ASC,  
 CASE WHEN @sortExpression ='location_dw DESC' THEN location_dw END DESC,  
 CASE WHEN @sortExpression ='real_location_name' THEN real_location_name END ASC,  
 CASE WHEN @sortExpression ='real_location_name DESC' THEN real_location_name END DESC,  
 CASE WHEN @sortExpression ='location_name' THEN location_name END ASC,  
 CASE WHEN @sortExpression ='location_name DESC' THEN location_name END DESC,  
 CASE WHEN @sortExpression ='location_name_dw' THEN location_name_dw END ASC,  
 CASE WHEN @sortExpression ='location_name_dw DESC' THEN location_name_dw END DESC,  
 CASE WHEN @sortExpression ='active' THEN active END ASC,  
 CASE WHEN @sortExpression ='active DESC' THEN active END DESC,  
 CASE WHEN @sortExpression ='ap_dt_rr' THEN ap_dt_rr END ASC,  
 CASE WHEN @sortExpression ='ap_dt_rr DESC' THEN ap_dt_rr END DESC,  
 CASE WHEN @sortExpression ='cal' THEN cal END ASC,  
 CASE WHEN @sortExpression ='cal DESC' THEN cal END DESC,  
 CASE WHEN @sortExpression ='ops_area' THEN ops_area END ASC,  
 CASE WHEN @sortExpression ='ops_area DESC' THEN ops_area END DESC,  
 CASE WHEN @sortExpression ='cms_location_group_id' THEN cms_location_group_id END ASC,  
 CASE WHEN @sortExpression ='cms_location_group_id DESC' THEN cms_location_group_id END DESC,  
 CASE WHEN @sortExpression ='cms_location_group' THEN cms_location_group END ASC,  
 CASE WHEN @sortExpression ='cms_location_group DESC' THEN cms_location_group END DESC,  
 CASE WHEN @sortExpression ='served_by_locn' THEN served_by_locn END ASC,  
 CASE WHEN @sortExpression ='served_by_locn DESC' THEN served_by_locn END DESC,  
 CASE WHEN @sortExpression ='turnaround_hours' THEN turnaround_hours END ASC,  
 CASE WHEN @sortExpression ='turnaround_hours DESC' THEN turnaround_hours END DESC  
  
 SELECT  
  location, location_dw, real_location_name, location_name, location_name_dw,  
   active, ap_dt_rr, cal, ops_area_id, ops_area, cms_location_group_id,   
    cms_location_group, served_by_locn, turnaround_hours  
 FROM @QUERYTABLE q  
 INNER JOIN @PAGING p ON p.rowId = q.rowId   
 WHERE (p.pageIndex BETWEEN @startRowIndex AND @maximumRows)  
 ORDER BY p.pageIndex;  
   
 --Select total row count  
 SELECT COUNT(*) AS totalCount FROM @QUERYTABLE;  
  
END