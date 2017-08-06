-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE procedure [dbo].[spPortal_LocationSelectByLocation]   
(  
 @location VARCHAR(50) =NULL  
)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
  
SELECT     location, location_dw, real_location_name, location_name, location_name_dw, active, ap_dt_rr, cal, cms_location_group_id, ops_area_id,   
                      served_by_locn, turnaround_hours, NULL AS ops_area, NULL AS cms_location_group  
FROM         LOCATIONS  
WHERE location=@location  
  
END