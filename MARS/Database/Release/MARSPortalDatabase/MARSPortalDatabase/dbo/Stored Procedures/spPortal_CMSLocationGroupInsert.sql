-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE procedure [dbo].[spPortal_CMSLocationGroupInsert]   
(   
 --@cms_location_group_code varchar(10)=NULL,  
 @cms_location_group_code_dw varchar(10)=NULL,  
 @cms_location_group varchar(50)=NULL,   
 @cms_pool_id int =NULL  
)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
   
 INSERT INTO cms_location_groups  
 (cms_location_group_code_dw, cms_location_group, cms_pool_id )  --(cms_location_group_code, cms_location_group_code_dw, cms_location_group, cms_pool_id )  
 VALUES  
 (@cms_location_group_code_dw, @cms_location_group, @cms_pool_id )  --(@cms_location_group_code, @cms_location_group_code_dw, @cms_location_group, @cms_pool_id )  


	
 -- manage relavent entries in Necessary Fleet tablwe
 	-- Insert new entires onto Necessary Fleet for new location group
	MERGE MARS_CMS_NECESSARY_FLEET AS [TARGET]
	USING	
	(
		SELECT LG.COUNTRY, CMS_LOCATION_GROUP_ID, CAR_CLASS_ID
		FROM vw_Mapping_CMS_Location_Group LG
			INNER JOIN vw_Mapping_CarClass CC ON LG.COUNTRY = CC.COUNTRY
		WHERE LG.CMS_LOCATION_GROUP = @cms_location_group			
	) AS [SOURCE]
	ON ([TARGET].CMS_LOCATION_GROUP_ID = [SOURCE].CMS_LOCATION_GROUP_ID AND [TARGET].CAR_CLASS_ID = [SOURCE].CAR_CLASS_ID)
	
	-- insert new entries
	WHEN NOT MATCHED BY TARGET THEN
	INSERT (COUNTRY, CMS_LOCATION_GROUP_ID, CAR_CLASS_ID, UTILISATION, NONREV_FLEET)
	VALUES([SOURCE].COUNTRY, [SOURCE].CMS_LOCATION_GROUP_ID, [SOURCE].CAR_CLASS_ID, 100.0, 0.00) 
	;
  
 -- Return Success  
 SELECT 0  
   
END