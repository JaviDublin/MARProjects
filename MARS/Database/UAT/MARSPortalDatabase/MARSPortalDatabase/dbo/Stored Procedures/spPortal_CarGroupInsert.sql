-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CarGroupInsert] 
(	
	@car_group varchar(3)=NULL, 
	@car_group_gold varchar(3)=NULL, 
	@car_group_Fivestar varchar(3)=NULL, 
	@car_group_PresidentCircle varchar(3)=NULL, 
	@car_group_Platinum varchar(3)=NULL, 
	@car_class_id int=NULL,
	@sort_car_group int=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO CAR_GROUPS
	(car_group, car_group_gold, car_class_id, sort_car_group, car_group_FiveStar, car_group_presidentCircle, car_group_platinum) 

	VALUES( @car_group, @car_group_gold, @car_class_id, @sort_car_group,@car_group_Fivestar, @car_group_PresidentCircle,@car_group_Platinum )
	
	
	-- manage relevant entries in Necessary Fleet table
	DECLARE @COUNTRY varchar(3)--, @car_class_group_id int, @car_class varchar(50)

	set @COUNTRY = (select COUNTRY from vw_Mapping_CarClass where CAR_CLASS_GROUP_ID = @car_class_id AND CAR_CLASS = @car_group)

	-- Insert new entires onto Necessary Fleet for new location group
	MERGE MARS_CMS_NECESSARY_FLEET AS [TARGET]
	USING	
	(
		SELECT LG.COUNTRY, CMS_LOCATION_GROUP_ID, CAR_CLASS_ID
		FROM vw_Mapping_CMS_Location_Group LG
			INNER JOIN vw_Mapping_CarClass CC ON LG.COUNTRY = CC.COUNTRY
		WHERE LG.COUNTRY = @COUNTRY AND CC.CAR_CLASS = @car_group
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