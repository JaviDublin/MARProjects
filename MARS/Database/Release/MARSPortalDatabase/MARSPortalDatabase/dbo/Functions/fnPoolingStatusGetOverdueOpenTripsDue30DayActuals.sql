








-- =============================================
-- Author:		Anthony McClorey
-- Description:	Pooling - Status - Overdue open trips due - 30 Day Actuals
-- This is calculated from FEA
-- =============================================
CREATE FUNCTION [dbo].[fnPoolingStatusGetOverdueOpenTripsDue30DayActuals]
(
	@optionLogic				INT=NULL,
	@country					VARCHAR(10)=NULL,
	@cms_pool_Id				INT=NULL,
	@cms_location_group_code	VARCHAR(10)=NULL,
	@ops_region_Id				INT=NULL,
	@ops_area_Id				INT=NULL,
	@location					VARCHAR(50)=NULL,
	@car_segment_Id				INT=NULL,
	@car_class_Id				INT=NULL,
	@car_group_Id				INT=NULL
)
RETURNS INT
AS
BEGIN
-- Declare variable to hold result
DECLARE @result INT
			
-- Check Option Logic 
-- CMS = 1 / OPS = 2
IF (@optionLogic =1)

	BEGIN	-- (CMS Option)

		SELECT @result = 
				ISNULL(SUM(total_fleet),0)
		FROM
				dbo.vw_Pooling_CMS_FEA
		WHERE
				(location = ISNULL(@location,location)) AND
				(country = ISNULL(@country,country)) AND
				(cms_pool_Id = ISNULL(@cms_pool_Id,cms_pool_Id)) AND
				(cms_location_group_id = ISNULL(@cms_location_group_code,cms_location_group_id)) AND
				(car_group_Id=ISNULL(@car_group_id,car_group_id)) AND
				(car_class_Id =ISNULL(@car_class_Id,car_class_Id)) AND
				(car_segment_Id = ISNULL(@car_segment_Id,car_segment_Id)) AND
				(ci_days < 0) AND (movetype='T-O')

	END	-- ( END of CMS Option)		
		
ELSE

	BEGIN	-- (OPS Option)

		SELECT @result = 
				ISNULL(SUM(total_fleet),0)
		FROM
				dbo.vw_Pooling_OPS_FEA
		WHERE 
				(location = ISNULL(@location,location)) AND
				(country = ISNULL(@country,country)) AND
				(ops_region_Id = ISNULL(@ops_region_Id,ops_region_Id)) AND
				(ops_area_Id = ISNULL(@ops_area_Id,ops_area_Id)) AND
				(car_group_Id=ISNULL(@car_group_id,car_group_id)) AND
				(car_class_Id =ISNULL(@car_class_Id,car_class_Id)) AND
				(car_segment_Id = ISNULL(@car_segment_Id,car_segment_Id)) AND
				(ci_days < 0) AND (movetype='T-O')

	END	-- (End of OPS Option)
	
	-- Return Result
			
		-- The Overdue open trips due
		RETURN @result
	
END