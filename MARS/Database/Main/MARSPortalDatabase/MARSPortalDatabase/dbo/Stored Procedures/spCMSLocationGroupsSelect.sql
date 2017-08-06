-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE procedure [dbo].[spCMSLocationGroupsSelect]  
 -- Add the parameters for the stored procedure here  
 @country VARCHAR(10) = NULL,  
 @cms_pool_id INT = NULL  
 ,@CAL As VarChar(1) = '*' --Added by Gavin to get Cal Location Groups
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 SELECT LG.cms_location_group_id, LG.cms_location_group FROM CMS_LOCATION_GROUPS AS LG  
 JOIN CMS_POOLS AS CP ON CP.cms_pool_id = LG.cms_pool_id  
 WHERE ((CP.country = @country) OR (@country IS NULL))   
 -- Added by Gavin to Filter out CAL Location groups
  AND ((LG.cms_pool_id = @cms_pool_id) OR (@cms_pool_id IS NULL))  
  and (@CAL in (select loc.cal from dbo.LOCATIONS as loc 
		where LG.cms_location_group_id = loc.cms_location_group_id) 
		or @CAL = '*') 
 ORDER BY CP.country, LG.cms_location_group  
  
END