-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spCarClassesSelect]
	
	@country		VARCHAR(10) = NULL,
	@car_segment_id INT			= NULL
	
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
		car_class_id , car_class 
	FROM 
		CAR_CLASSES AS CC	
	LEFT JOIN CAR_SEGMENTS	AS CS	ON CS.car_segment_id	= CC.car_segment_id
	LEFT JOIN COUNTRIES		AS C	ON C.country			= CS.country
	WHERE 
		((C.country = @country) OR @country IS NULL)
	AND 
		((CS.car_segment_id = @car_segment_id) OR @car_segment_id IS NULL)		
	ORDER BY 
		CS.sort_car_segment, CC.sort_car_class, CC.car_class


END






--set ANSI_NULLS ON
--set QUOTED_IDENTIFIER ON
--go
--
--
--
---- =============================================
---- Author:		<Author,,Name>
---- Create date: <Create Date,,>
---- Description:	<Description,,>
---- =============================================
--ALTER PROCEDURE [dbo].[spCarClassesSelect]
--	-- Add the parameters for the stored procedure here	
--	@country_code VARCHAR(10) = NULL,
--	@region_id INT = NULL,
--	@location_group_id INT = NULL,
--	@location_id VARCHAR(50) = NULL,
--	@segment_id INT = NULL,
--	@car_group CHAR = NULL -- C or V or NULL
--AS
--BEGIN
--	-- SET NOCOUNT ON added to prevent extra result sets from
--	-- interfering with SELECT statements.
--	SET NOCOUNT ON;
--
--    -- Insert statements for procedure here
--	SELECT DISTINCT CC.car_class FROM CAR_CLASSES AS CC
--	LEFT JOIN SEGMENTS AS S ON S.segment_id = CC.segment_id
--	WHERE CC.segment_id = @segment_id OR @segment_id IS NULL
--	AND ((S.group_id = 3 AND @car_group = 'V') OR (S.group_id <> 3 AND @car_group = 'C') OR (@car_group IS NULL))
--	AND ((CC.country_code IN 
--		(SELECT @country_code
--			UNION
--		 SELECT R.country_code FROM REGIONS AS R WHERE region_id = @region_id
--			UNION
--		 SELECT LG.country_code FROM LOCATION_GROUPS AS LG WHERE location_group_id = @location_group_id
--			UNION 
--		 SELECT R1.country_code FROM LOCATIONS AS L LEFT JOIN REGIONS AS R1 ON R1.region_id = L.region_id WHERE L.location_id = @location_id)) 
--		OR ((@country_code IS NULL) AND (@region_id IS NULL) AND (@location_group_id IS NULL) AND (@location_id IS NULL)))
--
--
--
--END
--
--
--