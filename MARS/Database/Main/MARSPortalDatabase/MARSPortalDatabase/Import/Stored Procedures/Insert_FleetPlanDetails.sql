-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Import].[Insert_FleetPlanDetails]

	@COUNTRY		CHAR(2)	, 
	@fleetPlanID	INT		, 
	@isAddition		BIT
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @fleetPlanEntryID int
	SET @fleetPlanEntryID = 
			(
					SELECT 
						PKID 
					FROM 
						MARS_CMS_FleetPlanEntry 
					WHERE 
						Country		= @COUNTRY 
					AND fleetPlanID = @fleetPlanID
			)
		
	IF (@isAddition = 1)
	-- Addition
	BEGIN
		-- set [addition] field to zero
		UPDATE MARS_CMS_FleetPlanDetails
		SET [addition] = 0
		FROM 
			MARS_CMS_FleetPlanDetails PD
		INNER JOIN MARS_CMS_FleetPlanEntry PE ON PE.PKID = PD.fleetPlanEntryID
		WHERE 
			PE.Country		= @COUNTRY 
		AND PE.fleetPlanID	= @fleetPlanID	
		
		-- Update MARS_CMS_FleetPlanDetails table		
		MERGE MARS_CMS_FleetPlanDetails AS [TARGET]
		USING dbo.[FleetPlan_pre_Summary]
			(
				@COUNTRY, @fleetPlanID, @isAddition
			) AS [SOURCE]
		ON 
			(
					[TARGET].targetDate				= [SOURCE].targetDate 
				AND [TARGET].CMS_LOCATION_GROUP_ID	= [SOURCE].CMS_LOCATION_GROUP_ID 
				AND [TARGET].CAR_CLASS_ID			= [SOURCE].CAR_CLASS_ID
				AND [TARGET].fleetPlanEntryID		= @fleetPlanEntryID
			)
		
		-- Update [addition] field
		WHEN MATCHED THEN
		UPDATE SET 
			[TARGET].addition = [SOURCE].amount
		
		-- insert new entries
		WHEN NOT MATCHED BY TARGET THEN
		INSERT 
			(
				fleetPlanEntryID, targetDate, 
				CMS_LOCATION_GROUP_ID, CAR_CLASS_ID, 
				addition, deletion
			)
		VALUES 
			(
				@fleetPlanEntryID, [SOURCE].targetDate, 
				[SOURCE].CMS_LOCATION_GROUP_ID, [SOURCE].CAR_CLASS_ID, 
				[SOURCE].amount, 0
			);	
	END
	
	ELSE
	-- Deletion
	BEGIN
		-- set [deletion] field to zero
		UPDATE MARS_CMS_FleetPlanDetails
		SET [deletion] = 0
		FROM 
			MARS_CMS_FleetPlanDetails PD
		INNER JOIN MARS_CMS_FleetPlanEntry PE ON PE.PKID = PD.fleetPlanEntryID
		WHERE 
			PE.Country		= @COUNTRY 
		AND PE.fleetPlanID	= @fleetPlanID	
		
		-- Update MARS_CMS_FleetPlanDetails table		
		MERGE MARS_CMS_FleetPlanDetails AS [TARGET]
		USING dbo.[FleetPlan_pre_Summary]
			(
				@COUNTRY, @fleetPlanID, @isAddition
			) AS [SOURCE]
		ON 
			(
				[TARGET].targetDate				= [SOURCE].targetDate 
			AND [TARGET].CMS_LOCATION_GROUP_ID	= [SOURCE].CMS_LOCATION_GROUP_ID 
			AND [TARGET].CAR_CLASS_ID			= [SOURCE].CAR_CLASS_ID
			AND [TARGET].fleetPlanEntryID		= @fleetPlanEntryID
			)
		
		-- Update [addition] field
		WHEN MATCHED THEN
		UPDATE SET [TARGET].deletion = [SOURCE].amount
		
		-- insert new entries
		WHEN NOT MATCHED BY TARGET THEN
		INSERT 
			(
				fleetPlanEntryID, targetDate, 
				CMS_LOCATION_GROUP_ID, CAR_CLASS_ID, 
				addition, deletion
			)
		VALUES 
			(
				@fleetPlanEntryID, [SOURCE].targetDate, 
				[SOURCE].CMS_LOCATION_GROUP_ID, [SOURCE].CAR_CLASS_ID, 
				0, [SOURCE].amount
			);
			
	END
	
	
	-- clean dummy entries (both addition and deletion are 0 OR targetDate is older than 5 days)
	DELETE MARS_CMS_FleetPlanDetails
	FROM 
		MARS_CMS_FleetPlanDetails PD
	INNER JOIN MARS_CMS_FleetPlanEntry PE ON PD.fleetPlanEntryID = PE.PKID			
	WHERE 
		PE.Country = @COUNTRY 
	AND PE.fleetPlanID = @fleetPlanID
	AND ((PD.addition = 0 and PD.deletion = 0) 
		OR (PD.targetDate <= DATEADD(d, -5, getdate())))

	
-- EXEC [spSSISCMSFleetPlanInsert] 'GE', 1, 1

    
END