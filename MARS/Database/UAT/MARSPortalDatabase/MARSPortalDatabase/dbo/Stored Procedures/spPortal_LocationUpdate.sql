-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE procedure [dbo].[spPortal_LocationUpdate]   
(   
 @location varchar(50)=NULL,  
 @location_dw varchar(50)=NULL,  
 @real_location_name varchar(50)=NULL,  
 @location_name varchar(50)=NULL,  
 @location_name_dw varchar(50)=NULL,  
 @active bit=NULL,  
 @ap_dt_rr varchar(2)=NULL,  
 @cal varchar(1)=NULL,  
 @ops_area_id int=NULL,  
 @cms_location_group_id INT=NULL,  --@cms_location_group_code varchar(10)=NULL,  
 @served_by_locn varchar(50)=NULL,  
 @turnaround_hours int=NULL  
)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 UPDATE Locations SET    
 location_dw = @location_dw,   
 real_location_name = @real_location_name,   
 location_name = @location_name,   
 location_name_dw = @location_name_dw,   
 active = @active,   
 ap_dt_rr = @ap_dt_rr,   
 cal = @cal,  
 ops_area_id = @ops_area_id,   
 cms_location_group_id = @cms_location_group_id,   
 served_by_locn = @served_by_locn,   
 turnaround_hours = @turnaround_hours  
     
 WHERE location = @location  
  
END