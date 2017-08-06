

CREATE PROCEDURE [dbo].[LogDashboardForecast]
		@theDay datetime
AS
BEGIN
	
	SET NOCOUNT ON;

    DECLARE @ReportDate DATETIME
	SET @ReportDate = @theDay --CONVERT(DATETIME, DATEDIFF(DAY, 0, GETDATE()))

SELECT
	f.REP_DATE AS 'ReportDate'
	,f.COUNTRY AS 'CountryCode'
	,p.cms_pool AS 'CmsPool'
	,lg.cms_location_group AS 'CmsLocationGroup'
	,CC.car_class AS 'CarClass'
	,CONVERT(TINYINT,cc.sort_car_class) AS 'CarClassSortOrder'
	,SUM(ISNULL(f.UNCONSTRAINED,0)) AS 'Unconstrained'
	,SUM(ISNULL(f.CONSTRAINED,0)) AS 'Constrained'
FROM dbo.MARS_CMS_FORECAST f
INNER JOIN dbo.CMS_LOCATION_GROUPS lg ON lg.cms_location_group_id=f.CMS_LOCATION_GROUP_ID
INNER JOIN [dbo].[CMS_POOLS] p ON p.cms_pool_id =lg.cms_pool_id
INNER JOIN dbo.CAR_GROUPS cg ON cg.car_group_id= f.CAR_CLASS_ID
INNER JOIN dbo.CAR_CLASSES cc ON cc.car_class_id=cg.car_class_id
INNER JOIN [dbo].[CAR_SEGMENTS] cs ON cc.car_segment_id = cs.car_segment_id AND cs.country =f.COUNTRY
WHERE
	cs.car_segment = 'Car'
	AND cs.car_segment <> 'Unmapped'
	AND (f.COUNTRY IN ('BE', 'FR', 'GE', 'IT', 'LU', 'NE', 'SP', 'UK'))
	AND	f.REP_DATE >= @ReportDate
	AND	f.REP_DATE <= DATEADD(day,90,@ReportDate)
	AND p.cms_pool NOT IN (SELECT DISTINCT AreaName FROM dbo.LogDashboardRestrictAreas WHERE (AreaType = 'CMSPool'))
	AND lg.cms_location_group NOT IN (SELECT DISTINCT AreaName FROM LogDashboardRestrictAreas WHERE (AreaType = 'CMSLocationGroup'))	
GROUP BY
	f.REP_DATE
	,f.COUNTRY
	,p.cms_pool
	,lg.cms_location_group
	,cc.car_class
	,cc.sort_car_class;

END