
CREATE PROCEDURE [dbo].[PopulateFleetHistoryFromFleetNow]
AS
BEGIN
	SET NOCOUNT ON;
	
	insert into [dbo].[FleetHistory](Timestamp, CarGroupId, LocationId, FleetTypeId, AvgTotal, MinTotal, MaxTotal, PeakTotal
				, TroughTotal, AvgBd, MinBd, MaxBd, PeakBd, TroughBd, AvgCu, MinCu, MaxCu, PeakCu, TroughCu, AvgFs, MinFs
				, MaxFs, PeakFs, TroughFs, AvgHa, MinHa, MaxHa, PeakHa, TroughHa, AvgHl, MinHl, MaxHl, PeakHl, TroughHl
				, AvgLl, MinLl, MaxLl, PeakLl, TroughLl, AvgMm, MinMm, MaxMm, PeakMm, TroughMm, AvgNc, MinNc, MaxNc, PeakNc
				, TroughNc, AvgPl, MinPl, MaxPl, PeakPl, TroughPl, AvgRl, MinRl, MaxRl, PeakRl, TroughRl, AvgRp, MinRp, MaxRp
				, PeakRp, TroughRp, AvgIdle, MinIdle, MaxIdle, PeakIdle, TroughIdle, AvgSu, MinSu, MaxSu, PeakSu, TroughSu, AvgSv
				, MinSv, MaxSv, PeakSv, TroughSv, AvgTb, MinTb, MaxTb, PeakTb, TroughTb, AvgTc, MinTc, MaxTc, PeakTc, TroughTc
				, AvgTn, MinTn, MaxTn, PeakTn, TroughTn, AvgTw, MinTw, MaxTw, PeakTw, TroughTw, AvgWs, MinWs, MaxWs, PeakWs, TroughWs
				, AvgOverdue, MinOverdue, MaxOverdue, PeakOverdue, TroughOverdue
				, AvgOperationalFleet, MinOperationalFleet, MaxOperationalFleet, PeakOperationalFleet, TroughOperationalFleet
				, AvgAvailableFleet, MinAvailableFleet, MaxAvailableFleet, PeakAvailableFleet, TroughAvailableFleet
				, AvgOnRent, MinOnRent, MaxOnRent, PeakOnRent, TroughOnRent)
			
select nowDate as [Timestamp], 
	AvgMinMax.CarGroupId, AvgMinMax.LocationId, AvgMinMax.FleetTypeId
	, AvgTotalFleet, MinTotalFleet, MaxTotalFleet, PeakData.SumTotal as PeakTotal, TroughData.SumTotal as TroughTotal
	, AvgBd, MinBd, MaxBd, PeakData.SumBd as PeakBd, TroughData.SumBd as TroughBd
	, AvgCu, MinCu, MaxCu, PeakData.SumCu as PeakCu, TroughData.SumCu as TroughCu
	, AvgFs, MinFs, MaxFs, PeakData.SumFs as PeakFs, TroughData.SumFs as TroughFs
	, AvgHa, MinHa, MaxHa, PeakData.SumHa as PeakHa, TroughData.SumHa as TroughHa
	, AvgHl, MinHl, MaxHl, PeakData.SumHl as PeakHl, TroughData.SumHl as TroughHl
	, AvgLl, MinLl, MaxLl, PeakData.SumLl as PeakLl, TroughData.SumLl as TroughLl
	, AvgMm, MinMm, MaxMm, PeakData.SumMm as PeakMm, TroughData.SumMm as TroughMm
	, AvgNc, MinNc, MaxNc, PeakData.SumNc as PeakNc, TroughData.SumNc as TroughNc
	, AvgPl, MinPl, MaxPl, PeakData.SumPl as PeakPl, TroughData.SumPl as TroughPl
	, AvgRl, MinRl, MaxRl, PeakData.SumRl as PeakRl, TroughData.SumRl as TroughRl
	, AvgRp, MinRp, MaxRp, PeakData.SumRp as PeakRp, TroughData.SumRp as TroughRp
	, AvgIdle, MinIdle, MaxIdle, PeakData.SumIdle as PeakIdle, TroughData.SumIdle as TroughIdle
	, AvgSu, MinSu, MaxSu, PeakData.SumSu as PeakSu, TroughData.SumSu as TroughSu
	, AvgSv, MinSv, MaxSv, PeakData.SumSv as PeakSv, TroughData.SumSv as TroughSv
	, AvgTb, MinTb, MaxTb, PeakData.SumTb as PeakTb, TroughData.SumTb as TroughTb
	, AvgTc, MinTc, MaxTc, PeakData.SumTc as PeakTc, TroughData.SumTc as TroughTc
	, AvgTn, MinTn, MaxTn, PeakData.SumTn as PeakTn, TroughData.SumTn as TroughTn
	, AvgTw, MinTw, MaxTw, PeakData.SumTw as PeakTw, TroughData.SumTw as TroughTw
	, AvgWs, MinWs, MaxWs, PeakData.SumWs as PeakWs, TroughData.SumWs as TroughWs
	, AvgOverdue, MinOverdue, MaxOverdue, PeakData.SumOverdue as PeakOverdue, TroughData.SumOverdue as TroughOverdue
	, AvgOperational, MinOperational, MaxOperational, PeakData.OperationalFleet as PeakOperational, TroughData.OperationalFleet as TroughOperational
	, AvgAvailable, MinAvailable, MaxAvailable, PeakData.AvailableFleet as PeakAvailabile, TroughData.AvailableFleet as TroughAvailable
	, AvgOnRent, MinOnRent, MaxOnRent, PeakData.OnRent as PeakOnRent , TroughData.OnRent as TroughOnRent
from 
(
SELECT cast(min(timeStamp) as date) as nowDate,  CarGroupId, LocationId, FleetTypeId
,Avg(SumTotal) as AvgTotalFleet	,Min(SumTotal) as MinTotalFleet, MAX(SumTotal) as MaxTotalFleet
,Avg(SumBD) as AvgBD			,Min(SumBD) as MinBD,			 MAX(SumBD) as MaxBD		 
,Avg(SumCU) as AvgCU			,Min(SumCU) as MinCU,			 MAX(SumCU) as MaxCU		 
,Avg(SumFS) as AvgFS			,Min(SumFS) as MinFS,			 MAX(SumFS) as MaxFS			 
,Avg(SumHA) as AvgHA			,Min(SumHA) as MinHA,			 MAX(SumHA) as MaxHA			 
,Avg(SumHL) as AvgHL			,Min(SumHL) as MinHL,			 MAX(SumHL) as MaxHL			 
,Avg(SumLL) as AvgLL			,Min(SumLL) as MinLL,			 MAX(SumLL) as MaxLL			 
,Avg(SumMM) as AvgMM			,Min(SumMM) as MinMM,			 MAX(SumMM) as MaxMM			 
,Avg(SumNC) as AvgNC			,Min(SumNC) as MinNC,			 MAX(SumNC) as MaxNC			 
,Avg(SumPL) as AvgPL			,Min(SumPL) as MinPL,			 MAX(SumPL) as MaxPL			 
,Avg(SumRL) as AvgRL			,Min(SumRL) as MinRL,			 MAX(SumRL) as MaxRL			 
,Avg(SumRP) as AvgRP			,Min(SumRP) as MinRP,			 MAX(SumRP) as MaxRP			 
,Avg(SumIdle) as AvgIdle		,Min(SumIdle) as MinIdle,		 MAX(SumIdle) as MaxIdle
,Avg(SumSU) as AvgSU			,Min(SumSU) as MinSU,			 MAX(SumSU) as MaxSU
,Avg(SumSV) as AvgSV			,Min(SumSV) as MinSV,			 MAX(SumSV) as MaxSV
,Avg(SumTB) as AvgTB			,Min(SumTB) as MinTB,			 MAX(SumTB) as MaxTB
,Avg(SumTC) as AvgTC			,Min(SumTC) as MinTC,			 MAX(SumTC) as MaxTC
,Avg(SumTN) as AvgTN			,Min(SumTN) as MinTN,			 MAX(SumTN) as MaxTN
,Avg(SumTW) as AvgTW			,Min(SumTW) as MinTW,			 MAX(SumTW) as MaxTW
,Avg(SumWS) as AvgWS			,Min(SumWS) as MinWS,			 MAX(SumWS) as MaxWS
,Avg(SumOverdue) as AvgOverdue  ,Min(SumOverdue) as MinOverdue,	 MAX(SumOverdue) as MaxOverdue
,Avg(OperationalFleet) as AvgOperational, Min(OperationalFleet) as MinOperational,	 MAX(OperationalFleet) as MaxOperational
,Avg(AvailableFleet) as AvgAvailable, Min(AvailableFleet) as MinAvailable,	 MAX(AvailableFleet) as MaxAvailable
,Avg(OnRent) as AvgOnRent, Min(OnRent) as MinOnRent,	 MAX(OnRent) as MaxOnRent
FROM FleetNow fn
where cast([TimeStamp] as date) = cast( dateadd(day, -1, getdate()) as date)
group by CarGroupId, LocationId, FleetTypeId
) AvgMinMax
join 
(
	select CarGroupId, LocationId, FleetTypeId, SumTotal, SumBd, SumCu, SumFs, SumHa, SumHl, SumLl, SumMm, SumNc
			, SumPl, SumRl, SumRp, SumIdle, SumSu, SumSv, SumTb, SumTc, SumTn, SumTw, SumWs, SumOverdue, Utilization
				, OperationalFleet, AvailableFleet, OnRent
	from FleetNow fn 
	where fleetNowId in 
	(
		select fleetnowId
		from (
				select fleetnowId
					, ROW_NUMBER() over (Partition BY CarGroupId, LocationId, FleetTypeId order by Utilization asc) as RowNum
				from FleetNow
				where cast([TimeStamp] as date) = cast( dateadd(day, -1, getdate()) as date)
			) MinUtilIds
		where RowNum = 1

	)
) TroughData on AvgMinMax.CarGroupId = TroughData.CarGroupId and AvgMinMax.LocationId = TroughData.LocationId and AvgMinMax.FleetTypeId = TroughData.FleetTypeId
join (
	select CarGroupId, LocationId, FleetTypeId, SumTotal, SumBd, SumCu, SumFs, SumHa, SumHl, SumLl, SumMm, SumNc
		, SumPl, SumRl, SumRp, SumIdle, SumSu, SumSv, SumTb, SumTc, SumTn, SumTw, SumWs, SumOverdue, Utilization
		, OperationalFleet, AvailableFleet, OnRent
	from FleetNow fn 
	where fleetNowId in 
	(
		select fleetnowId
		from (
				select fleetnowId
					, ROW_NUMBER() over (Partition BY CarGroupId, LocationId, FleetTypeId order by Utilization desc) as RowNum
				from FleetNow
				where cast([TimeStamp] as date) = cast( dateadd(day, -1, getdate()) as date)
			) MinUtilIds
		where RowNum = 1
	)
) PeakData on AvgMinMax.CarGroupId = PeakData.CarGroupId and AvgMinMax.LocationId = PeakData.LocationId and AvgMinMax.FleetTypeId = PeakData.FleetTypeId


truncate table fleetNow

END