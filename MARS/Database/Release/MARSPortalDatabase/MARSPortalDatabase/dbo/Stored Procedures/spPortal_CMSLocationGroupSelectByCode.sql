-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE procedure [dbo].[spPortal_CMSLocationGroupSelectByCode]   
(  
 @cms_location_group_id INT =NULL  --@cms_location_group_code VARCHAR(10) =NULL  
)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
SELECT     cms_location_group_id, cms_location_group_code_dw,   
   cms_location_group, cms_pool_id, NULL AS cms_pool, NULL as country  
FROM         CMS_LOCATION_GROUPS  
WHERE cms_location_group_id=@cms_location_group_id  --cms_location_group_code=@cms_location_group_code  
  
END