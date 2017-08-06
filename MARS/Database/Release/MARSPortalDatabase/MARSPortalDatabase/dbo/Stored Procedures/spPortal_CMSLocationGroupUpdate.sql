-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE procedure [dbo].[spPortal_CMSLocationGroupUpdate]   
(   
 @cms_location_group_id INT,  --@cms_location_group_code varchar(10)=NULL,  
 @cms_location_group_code_dw varchar(10)=NULL,  
 @cms_location_group varchar(50)=NULL,   
 @cms_pool_id int=NULL  
)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 UPDATE CMS_Location_groups SET    
 cms_location_group_code_dw = @cms_location_group_code_dw,   
 cms_location_group = @cms_location_group,   
 cms_pool_id = @cms_pool_id  
          
 WHERE cms_location_group_id = @cms_location_group_id  --cms_location_group_code = @cms_location_group_code  
  
END