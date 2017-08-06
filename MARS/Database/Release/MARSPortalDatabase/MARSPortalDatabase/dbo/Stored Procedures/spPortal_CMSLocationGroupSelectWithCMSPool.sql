--sp_helptext spPortal_CMSLocationGroupSelectWithCMSPool


  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE procedure [dbo].[spPortal_CMSLocationGroupSelectWithCMSPool]   
  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
SELECT       
  g.cms_location_group_id,   
  '(' +  p.country + ') - ' + p.cms_pool + ' - ' + g.cms_location_group AS cms_location_group  
  
FROM           
  CMS_LOCATION_GROUPS g  
INNER JOIN  
  CMS_POOLS p ON g.cms_pool_id = p.cms_pool_id  
ORDER BY p.country, p.cms_pool
  
  
  
END