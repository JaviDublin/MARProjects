
CREATE PROCEDURE [Fao].[CalculateMin]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @FleetId int = 4
	declare @FromDate date = '2014-12-19'
	declare @CountryId int = 22			--CountryId

--drop table #MinNessesaryFleet

truncate table [Fao].[MinNessesaryFleet]

insert into [Fao].[MinNessesaryFleet] (LocationId, CarGroupId, AverageOnRent, MinFleet )
SELECT LocationId
	, CarGroupId
	, AverageOnRent
	, sum(MinNessesaryFleet) as MinFleet
FROM
(

	SELECT [GroupedPeakData].[LocationId]
		, [GroupedPeakData].[CarGroupId]
		, [Mcs].[CommercialCarSegmentId] AS [CommericalCarSegmentId]
		, [GroupedPeakData].[value] AS [AverageOnRent]
		, [Mcs].[Percentage] AS [MinCommercialSegmentRequired]
		, [RevByGroupedPeak].[GrossRevenue] AS [Revenue]
		, RevenueGroupedByCarGroup.[value] AS [TotalRevenueByCarGroup]
		, ([GroupedPeakData].[value] * [Mcs].[Percentage] * ([RevByGroupedPeak].[GrossRevenue] / RevenueGroupedByCarGroup.[value])) as MinNessesaryFleet

	FROM 
	(
		SELECT AVG([PeakData].[value]) AS [value], [PeakData].[LocationId], [PeakData].[CarGroupId]
		FROM (
			SELECT MAX([PeakOnRent]) AS [value], [LocationId], [CarGroupId]
			FROM (
				SELECT [LocationId], [CarGroupId], woy.WeekOfYear
									, [FleetTypeId], [Timestamp], c.[CountryId], [PeakOnRent]
				FROM [dbo].[FleetHistory] AS [Fh]
				INNER JOIN [dbo].[LOCATIONS] AS l ON l.[dim_Location_id] = [Fh].[LocationId]
				LEFT OUTER JOIN [dbo].[COUNTRIES] AS c ON c.[country] = l.[country]
				join dbo.IsoWeekOfYear woy on fh.Timestamp = woy.[Day]
				) AS WeeklyData
			WHERE ([FleetTypeId] = @FleetId) 
					AND ([Timestamp] >= @FromDate) 
					AND ([CountryId] = @CountryId)
			GROUP BY [LocationId], [CarGroupId], WeekOfYear
			) AS [PeakData]
		GROUP BY [PeakData].[LocationId], [PeakData].[CarGroupId]
	) AS [GroupedPeakData]
	INNER JOIN [dbo].[LOCATIONS] AS l ON [GroupedPeakData].[LocationId] = l.[dim_Location_id]
	INNER JOIN [dbo].[CAR_GROUPS] AS Cg ON [GroupedPeakData].[CarGroupId] = Cg.[car_group_id]
	INNER JOIN [Fao].[MinCommercialSegment] AS [Mcs] 
						ON (
										(
										SELECT cc.[car_segment_id]
										FROM [dbo].[CAR_CLASSES] AS cc
										WHERE cc.[car_class_id] = Cg.[car_class_id]
										) = [Mcs].[CarSegmentId]
						) 
						AND ([GroupedPeakData].[LocationId] = [Mcs].[LocationId])
	INNER JOIN [Fao].[RevenueByCommercialCarSegment] AS [RevByGroupedPeak] 
						ON (Cg.[car_group_id] = [RevByGroupedPeak].[CarGroupId]) 
						AND ([GroupedPeakData].[LocationId] = [RevByGroupedPeak].[LocationId]) 
						AND ([Mcs].[CommercialCarSegmentId] = [RevByGroupedPeak].[CommercialCarSegmentId])
	INNER JOIN [Fao].[CommercialCarSegment] AS [Ccs] 
					ON [Ccs].[CommercialCarSegmentId] = [Mcs].[CommercialCarSegmentId]
	INNER JOIN 
	(
		SELECT SUM([Rev].[GrossRevenue]) AS [value], [RevData].[LocationId], [RevData].[CarGroupId]
		FROM 
		(
			SELECT [GroupedFleetData].[LocationId], [GroupedFleetData].[CarGroupId]
			FROM (
				SELECT [FleetData].[LocationId], [FleetData].[CarGroupId]
				FROM (
					SELECT [FH].[LocationId], [FH].[CarGroupId]
							, woy.WeekOfYear AS WeekOfYear
							, [FH].[FleetTypeId]
							, [FH].[Timestamp]
							, c.[CountryId]
					FROM [dbo].[FleetHistory] AS [FH]
					join dbo.IsoWeekOfYear woy on [FH].Timestamp = woy.[Day]
					INNER JOIN [dbo].[LOCATIONS] AS l 
												ON l.[dim_Location_id] = [FH].[LocationId]
					LEFT OUTER JOIN [dbo].[COUNTRIES] AS c 
												ON c.[country] = l.[country]
					) AS [FleetData]
				WHERE ([FleetData].[FleetTypeId] = @FleetId) 
						AND ([FleetData].[Timestamp] >= @FromDate) 
						AND ([FleetData].[CountryId] = @CountryId)
				GROUP BY [FleetData].[LocationId], [FleetData].[CarGroupId], [FleetData].[WeekOfYear]
				) AS [GroupedFleetData]
			GROUP BY [GroupedFleetData].[LocationId], [GroupedFleetData].[CarGroupId]
		) AS [RevData]
		INNER JOIN [dbo].[LOCATIONS] AS l ON [RevData].[LocationId] = l.[dim_Location_id]
		INNER JOIN [dbo].[CAR_GROUPS] AS cg ON [RevData].[CarGroupId] = cg.[car_group_id]
		INNER JOIN [Fao].[MinCommercialSegment] AS [MinComSeg] 
													ON (
															SELECT [CarSegmentLookup].[car_segment_id]
															FROM [dbo].[CAR_CLASSES] AS [CarSegmentLookup]
															WHERE [CarSegmentLookup].[car_class_id] = cg.[car_class_id]	
														) = [MinComSeg].[CarSegmentId]
													AND ([RevData].[LocationId] = [MinComSeg].[LocationId])
		INNER JOIN [Fao].[RevenueByCommercialCarSegment] AS [Rev] 
											ON (cg.[car_group_id] = [Rev].[CarGroupId]) 
												AND ([RevData].[LocationId] = [Rev].[LocationId]) 
												AND ([MinComSeg].[CommercialCarSegmentId] = [Rev].[CommercialCarSegmentId])
		GROUP BY [RevData].[LocationId], [RevData].[CarGroupId]
		) AS RevenueGroupedByCarGroup 
				ON ([GroupedPeakData].[LocationId] = RevenueGroupedByCarGroup.[LocationId]) 
				AND ([GroupedPeakData].[CarGroupId] = RevenueGroupedByCarGroup.[CarGroupId])
) CommercialCarSegmentData
group by LocationId, CarGroupId, AverageOnRent


END