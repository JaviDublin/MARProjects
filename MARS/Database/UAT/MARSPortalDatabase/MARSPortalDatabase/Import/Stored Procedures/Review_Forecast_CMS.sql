-- =============================================
-- Author:		Javier
-- Create date: June 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Import].[Review_Forecast_CMS]
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	-- Remove blank spaces
	--==========================================================
	UPDATE MARS_CMS_FORECAST_cle SET
		COUNTRY			= LTRIM(RTRIM(COUNTRY))			,
		CAR_CLASS		= LTRIM(RTRIM(CAR_CLASS))		,
		CAR_CLASS_GROUP = LTRIM(RTRIM(CAR_CLASS_GROUP))



	DELETE FROM MARS_CMS_Forecast_cle WHERE FC_DATE IS NULL

	-- Delete Rows without countries or car groups
	--==========================================================
	DELETE FROM MARS_CMS_FORECAST_cle
	WHERE
		COUNTRY IS NULL OR COUNTRY = ''
		
	DELETE FROM MARS_CMS_FORECAST_cle
	WHERE
		CAR_CLASS IS NULL OR CAR_CLASS = ''
		
	UPDATE MARS_CMS_FORECAST_cle SET 
		CAR_CLASS_GROUP = 'UNMAPPED' 
	WHERE 
		CAR_CLASS_GROUP IS NULL OR CAR_CLASS_GROUP = ''
	
	
	-- Add new Car Groups
	--==========================================================
	DECLARE @TABLE_CAR_GROUP TABLE 
	(
		car_segment_id INT , car_class_id INT , car_class VARCHAR(100), 
		car_group VARCHAR(3), country VARCHAR(10))
	
	INSERT INTO @TABLE_CAR_GROUP (car_group , country , car_class)
	SELECT 
		CAR_CLASS , COUNTRY  , CAR_CLASS_GROUP
	FROM 
		MARS_CMS_FORECAST_cle
	WHERE
		CAR_CLASS NOT IN 
			(SELECT CAR_GROUP FROM CAR_GROUPS)
	GROUP BY 
		COUNTRY , CAR_CLASS , CAR_CLASS_GROUP

	UPDATE @TABLE_CAR_GROUP SET
		car_segment_id = T.car_segment_id
	FROM @TABLE_CAR_GROUP TCG_T
	INNER JOIN 
	(
		SELECT	
			CC.car_class_id , CC.car_class , CS.car_segment_id , CS.car_segment  , CS.country
		FROM 
			CAR_CLASSES CC
		INNER JOIN CAR_SEGMENTS AS CS ON CC.car_segment_id = CS.car_segment_id
		INNER JOIN @TABLE_CAR_GROUP AS TCG ON
			CS.country = TCG.country AND CC.car_class = TCG.car_class
	) T
	ON TCG_T.country = T.country AND TCG_T.car_class = T.car_class

	UPDATE @TABLE_CAR_GROUP SET
		car_segment_id = CS.car_segment_id
	FROM 
		@TABLE_CAR_GROUP TCG
	INNER JOIN CAR_SEGMENTS AS CS ON 
		TCG.country = CS.country 
	AND CS.car_segment = 'UNMAPPED' 
	WHERE 
		TCG.car_segment_id IS NULL

	UPDATE @TABLE_CAR_GROUP SET
		car_class_id = CC.car_class_id
	FROM 
		@TABLE_CAR_GROUP TCG
	INNER JOIN CAR_CLASSES AS CC ON 
		TCG.car_segment_id = CC.car_segment_id 
	AND CC.car_class = TCG.car_class

	INSERT INTO  CAR_GROUPS (car_group, car_group_gold, car_class_id, sort_car_group, car_group_fivestar, car_group_presidentCircle, car_group_platinum)
	SELECT 
		car_group , car_group , car_class_id , 0, car_group, car_group, car_group
	FROM 
		@TABLE_CAR_GROUP
	GROUP BY 
		car_group , car_class_id
		
		
		
	

    
END