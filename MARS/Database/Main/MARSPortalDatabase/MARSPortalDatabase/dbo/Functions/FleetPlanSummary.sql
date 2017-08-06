CREATE FUNCTION [dbo].[FleetPlanSummary](@COUNTRY varchar(2), @fleetPlanID tinyint, @dateFrom datetime, @dateTo datetime)
RETURNS @Results TABLE (targetDate datetime, LOCATION_GROUP_ID int, CAR_CLASS_ID int, amount int)
AS

        BEGIN      
        
    INSERT INTO @Results
    SELECT PD.targetDate as 'targetDate', PD.cms_Location_Group_ID as 'LOCATION_GROUP_ID',  PD.car_class_id as 'CAR_CLASS_ID', PD.amount as 'amount'
	FROM MARS_CMS_FleetPlanEntry PE
			INNER JOIN MARS_CMS_FleetPlanDetails PD ON PD.fleetPlanEntryID = PE.PKID			
	WHERE ((PE.Country = @COUNTRY OR @COUNTRY IS NULL) AND (PE.fleetPlanID = @fleetPlanID)
			AND (PD.targetDate BETWEEN @dateFrom AND @dateTo))
    
    
	--SELECT P.targetDate, P.LOCATION_GROUP_ID, P.CAR_CLASS_ID, SUM(P.Amount) AS 'Amount'
	--FROM
	--(
	--	-- Deletion
	--	(SELECT PE.Country, PE.fleetPlanID, PD.targetDate, PD.car_class_id AS 'CAR_CLASS_ID', PD.srcLocationGroupID AS 'LOCATION_GROUP_ID', amount * (-1) AS 'Amount'
	--	FROM MARS_CMS_FleetPlanEntry PE
	--		INNER JOIN MARS_CMS_FleetPlanDetails PD ON PD.fleetPlanEntryID = PE.PKID
	--	WHERE (PD.srcLocationGroupID IS NOT NULL) AND (PD.srcLocationGroupID > 0)
	--			AND (PE.Country = @COUNTRY OR @COUNTRY IS NULL) AND (PE.fleetPlanID = @fleetPlanID)
	--			AND PD.targetDate BETWEEN @dateFrom AND @dateTo	)
				
	--	UNION
	--	-- Addition
	--	(SELECT PE.Country, PE.fleetPlanID, PD.targetDate, PD.car_class_id AS 'CAR_CLASS_ID', PD.desLocationGroupID AS 'LOCATION_GROUP_ID', amount AS 'Amount'
	--	FROM MARS_CMS_FleetPlanEntry PE
	--		INNER JOIN MARS_CMS_FleetPlanDetails PD ON PD.fleetPlanEntryID = PE.PKID
	--	WHERE (PD.desLocationGroupID IS NOT NULL) AND (PD.desLocationGroupID > 0)	
	--			AND (PE.Country = @COUNTRY OR @COUNTRY IS NULL) AND (PE.fleetPlanID = @fleetPlanID)
	--			AND PD.targetDate BETWEEN @dateFrom AND @dateTo	)
	--) AS P
	--GROUP BY P.COUNTRY, P.fleetPlanID, P.targetDate, P.CAR_CLASS_ID, P.LOCATION_GROUP_ID
	
	


	
    RETURN
END