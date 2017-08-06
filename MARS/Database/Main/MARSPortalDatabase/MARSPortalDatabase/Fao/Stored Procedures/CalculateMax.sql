
create PROCEDURE [Fao].[CalculateMax]

AS
BEGIN
	SET NOCOUNT ON;


	declare @country varchar(100) = 'GE'
	truncate table [Fao].[MaxFleetSize]


insert into [Fao].[MaxFleetSize](WeekNumber, PeakDay, LocationId, CarGroupId, MaxForecastForLocationGroup, MaxForecastForLocation, MaxFleet )
SELECT GroupedForecastDate.[WeekOfYear] AS [WeekNumber]
		, Forecast.[REP_DATE] AS [PeakDay]
		--, Forecast.[CMS_LOCATION_GROUP_ID] AS [LocationGroupId]
		, [DeaggregationTable].[LocationId]
		, Forecast.[CAR_CLASS_ID] AS [CarGroupId]
		, COALESCE(Forecast.[UNCONSTRAINED],0) AS [MaxForLocationGroup]
		, (COALESCE(Forecast.[UNCONSTRAINED],0)) * 
				CONVERT(Decimal(33,4), [DeaggregationTable].[PercentVehiclesAllocated]) AS [MaxForLocation]
		, (COALESCE(Forecast.[UNCONSTRAINED],0)) * 
				CONVERT(Decimal(33,4), [DeaggregationTable].[PercentVehiclesAllocated]) * ( 1 + (( 1 - ff.UtilizationPercentage) * (1 + ff.NonRevPercentage)) )
FROM [dbo].[MARS_CMS_FORECAST] AS Forecast
INNER JOIN (
		SELECT (
			SELECT TopDateByUnconstrained.[REP_DATE]
			FROM (
				SELECT TOP (1) fc.[REP_DATE]
				FROM [dbo].[MARS_CMS_FORECAST] AS fc
				INNER JOIN [dbo].[IsoWeekOfYear] AS woy ON fc.[REP_DATE] = woy.[Day]
				INNER JOIN [dbo].[CAR_GROUPS] AS cg ON cg.[car_group_id] = fc.[CAR_CLASS_ID]
				INNER JOIN [dbo].[CAR_CLASSES] AS cc ON cc.[car_class_id] = cg.[car_class_id]
				WHERE (GroupedForecast.[CMS_LOCATION_GROUP_ID] = fc.[CMS_LOCATION_GROUP_ID]) 
						AND (GroupedForecast.[car_segment_id] = cc.[car_segment_id]) 
						AND (GroupedForecast.[WeekOfYear] = woy.[WeekOfYear]) 
						AND (fc.[COUNTRY] = @country) 
					
				ORDER BY fc.[UNCONSTRAINED] DESC
				) AS TopDateByUnconstrained
			) AS TopUnconstrainedDate
			, GroupedForecast.[CMS_LOCATION_GROUP_ID]
			, GroupedForecast.[car_segment_id]
			, GroupedForecast.[WeekOfYear]
		FROM 
		(
			SELECT fc.[CMS_LOCATION_GROUP_ID], cc.[car_segment_id], woy.[WeekOfYear]
			FROM [dbo].[MARS_CMS_FORECAST] AS fc
			INNER JOIN [dbo].[IsoWeekOfYear] AS woy ON fc.[REP_DATE] = woy.[Day]
			INNER JOIN [dbo].[CAR_GROUPS] AS cg ON cg.[car_group_id] = fc.[CAR_CLASS_ID]
			INNER JOIN [dbo].[CAR_CLASSES] AS cc ON cc.[car_class_id] = cg.[car_class_id]
			WHERE (fc.[COUNTRY] = @country) 
			GROUP BY fc.[CMS_LOCATION_GROUP_ID], cc.[car_segment_id], woy.[WeekOfYear]
		) AS GroupedForecast
    ) AS GroupedForecastDate 
			ON (Forecast.[REP_DATE] = GroupedForecastDate.TopUnconstrainedDate) 
			AND (
				Forecast.[CMS_LOCATION_GROUP_ID] = GroupedForecastDate.[CMS_LOCATION_GROUP_ID]) 
				AND (
						(
							SELECT cc.[car_segment_id]
							FROM [dbo].[CAR_GROUPS] AS cg
							INNER JOIN [dbo].[CAR_CLASSES] AS cc ON cc.[car_class_id] = cg.[car_class_id]
							WHERE cg.[car_group_id] = Forecast.[CAR_CLASS_ID]
						) = GroupedForecastDate.[car_segment_id]
					)
INNER JOIN [Fao].[CmsToLocationLevelPercent] AS [DeaggregationTable] 
				ON (Forecast.[CAR_CLASS_ID] = [DeaggregationTable].[CarGroupId]) 
					AND (Forecast.[CMS_LOCATION_GROUP_ID] = [DeaggregationTable].[LocationGroupId])
join fao.MaxFleetFactor ff on Forecast.CAR_CLASS_ID = ff.CarGroupId
							and [DeaggregationTable].LocationId = ff.LocationId
							and datepart(dw, forecast.REP_DATE) = ff.DayOfWeekId
WHERE Forecast.[COUNTRY] = @country

END