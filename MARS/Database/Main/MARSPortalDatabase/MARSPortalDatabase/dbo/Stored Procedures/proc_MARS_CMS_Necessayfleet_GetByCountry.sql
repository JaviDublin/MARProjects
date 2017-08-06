CREATE procedure [dbo].[proc_MARS_CMS_Necessayfleet_GetByCountry]
	
	@countryID varchar(3) 
AS
BEGIN
	
	SET NOCOUNT ON;

    select 
		C.[COUNTRY],
		C.[country_description],
		LC.[cms_location_group_id],
		LC.[cms_location_group],
		CG.[car_group_id],
		CG.[car_group],
		NF.[UTILISATION],
		NF.[NONREV_FLEET] from [dbo].[MARS_CMS_NECESSARY_FLEET] NF
		INNER JOIN COUNTRIES C on C.country = NF.[COUNTRY]
		INNER JOIN CMS_LOCATION_GROUPS LC on LC.cms_location_group_id = NF.[CMS_LOCATION_GROUP_ID]
		--INNER JOIN CAR_SEGMENTS CS on CS.country = C.country
		--INNER JOIN CAR_CLASSES CC on CC.car_segment_id = CS.car_segment_id
		INNER JOIN CAR_GROUPS CG on CG.car_group_id = NF.CAR_CLASS_ID
	WHERE NF.COUNTRY = @countryID
	group by 
		C.[country],
		C.[country_description],
		LC.[cms_location_group_id],
		LC.[cms_location_group],
		CG.[car_group_id],
		CG.[car_group],
		NF.[UTILISATION],
		NF.[NONREV_FLEET]
	order by 
		LC.[CMS_LOCATION_GROUP_ID]
END