CREATE FUNCTION [dbo].[FleetPlan_pre_Summary](@COUNTRY char(2), @fleetPlanID int, @isAddition bit)
RETURNS @Results TABLE (targetDate datetime, CMS_LOCATION_GROUP_ID int, CAR_CLASS_ID int, amount int)
AS

    BEGIN                

	INSERT INTO @Results
	SELECT 
		targetDate, L.cms_location_group_id, CC.CAR_CLASS_ID, SUM(ROUND(amount, 0)) as 'amount'
	FROM 
		MARS_CMS_FleetPlan_pre FPP
	INNER JOIN LOCATIONS L					ON	L.location = FPP.location
	INNER JOIN [dbo].vw_Mapping_CarClass CC	ON	CC.COUNTRY	= FPP.COUNTRY
											AND CC.CAR_CLASS = FPP.carclass	
	WHERE 
		FPP.country = @COUNTRY 
	AND 
		FPP.fleetPlanID = @fleetPlanID 
	AND 
		FPP.isAddition = @isAddition 
	AND 
		ROUND(FPP.amount, 0) > 0
	GROUP BY  
		FPP.targetDate, L.cms_location_group_id, CC.CAR_CLASS_ID

	
    RETURN
END