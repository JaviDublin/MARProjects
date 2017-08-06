-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE procedure [dbo].[spPortal_LocationInsert]   
(   
	 @location				VARCHAR(50)	= NULL,  
	 @location_dw			VARCHAR(50)	= NULL,  
	 @real_location_name	VARCHAR(50)	= NULL,  
	 @location_name			VARCHAR(50)	= NULL,  
	 @location_name_dw		VARCHAR(50)	= NULL,  
	 @active				BIT			= NULL,  
	 @ap_dt_rr				VARCHAR(2)	= NULL,  
	 @cal					VARCHAR(1)	= NULL,  
	 @ops_area_id			INT			= NULL,  
	 @cms_location_group_id INT			= NULL,  
	 @served_by_locn		VARCHAR(50)	= NULL,  
	 @turnaround_hours		INT			= NULL  
)  
AS  
BEGIN  
   
	 SET NOCOUNT ON;  
	  
	 DECLARE @country VARCHAR(2)
	 
	 SET @country = SUBSTRING(@location,1,2)
	 IF (SELECT COUNT(*) FROM COUNTRIES WHERE country = @country) = 0
		SET @country = 'NN'
	 
	 
	  
	 INSERT INTO [dbo].[LOCATIONS]
		(	
			location , location_dw , real_location_name , location_name,   
			location_name_dw , active , ap_dt_rr , cal ,   
			cms_location_group_id , ops_area_id ,   
			served_by_locn , turnaround_hours , country , city_desc , ownarea
		)  
	 VALUES  
		( 
			@location , @location_dw , @real_location_name , @location_name,   
			@location_name_dw , @active , @ap_dt_rr , @cal ,   
			@cms_location_group_id , @ops_area_id ,   
			@served_by_locn , @turnaround_hours , @country , @location_name , '000' + @country
		)  
	  
	 -- Return Success  
	 SELECT 0  
  
END