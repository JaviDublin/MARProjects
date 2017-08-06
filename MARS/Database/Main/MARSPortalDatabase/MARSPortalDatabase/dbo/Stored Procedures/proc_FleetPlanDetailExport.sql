CREATE procedure [dbo].[proc_FleetPlanDetailExport] 
	
	@country varchar(2), 
	@fleetPlanID int, 
	@locationGroupID int = null,
	@carClassGroupID int = null,
	@dateFrom datetime, 
	@dateTo datetime = null,
	@isAddition bit
	
AS
BEGIN

	SET NOCOUNT ON;
	SELECT 
		L.Location ,
		PE.Country + '-' + CC.car_group	,
		CONVERT(varchar,PD.targetDate,101),
		CASE @isAddition
			WHEN 
				1
			THEN
				SUM (PD.addition) 
			ELSE
				SUM(PD.deletion)
		END
		FROM 
			MARS_CMS_FleetPlanEntry PE
		INNER JOIN MARS_CMS_FleetPlanDetails PD ON PE.PKID = PD.fleetPlanEntryID				
		INNER JOIN CMS_LOCATION_GROUPS LG ON LG.cms_location_group_id = PD.cms_Location_Group_ID
		INNER JOIN [dbo].[RepresentiveLocationByLocationGroup] (@country) AS L
		ON LG.cms_location_group_id = L.cms_Location_Group_ID
		INNER JOIN CAR_GROUPS CC ON CC.car_group_id = PD.car_class_id
		WHERE 
			PE.Country = @country AND PE.fleetPlanID = @fleetPlanID
		AND ((PD.targetDate BETWEEN @dateFrom AND @dateTo) OR @dateTo IS NULL)
		AND (LG.cms_location_group_id = @locationGroupID OR @locationGroupID IS NULL)
		AND (CC.car_group_id = @carClassGroupID OR @carClassGroupID IS NULL)
		GROUP BY 
			PE.Country, PD.targetDate, LG.cms_location_group, CC.car_group, L.Location, amount 
		HAVING 
		(CASE @isAddition
			WHEN 1
			THEN
				SUM (PD.addition) 
			ELSE
				SUM(PD.deletion) 
		END)
		> 0
END