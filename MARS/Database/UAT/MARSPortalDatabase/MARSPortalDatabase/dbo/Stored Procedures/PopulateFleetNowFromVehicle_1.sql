	CREATE PROCEDURE [dbo].[PopulateFleetNowFromVehicle]
AS
BEGIN
	SET NOCOUNT ON;
	

	    WITH 
		VehicleStatus as (
	select cg.car_group_id as CarGroupId, l.dim_Location_id, v.VehicleFleetTypeId
		, sum(case when v.[LastOperationalStatusId] > 0 then 1 else 0 end) as TotalFleet
		, sum(case when v.[LastOperationalStatusId] = 1 then 1 else 0 end) as BD
		, sum(case when v.[LastOperationalStatusId] = 2 then 1 else 0 end) as CU
		, sum(case when v.[LastOperationalStatusId] = 3 then 1 else 0 end) as FS
		, sum(case when v.[LastOperationalStatusId] = 4 then 1 else 0 end) as HA
		, sum(case when v.[LastOperationalStatusId] = 5 then 1 else 0 end) as HL
		, sum(case when v.[LastOperationalStatusId] = 6 then 1 else 0 end) as LL
		, sum(case when v.[LastOperationalStatusId] = 7 then 1 else 0 end) as MM
		, sum(case when v.[LastOperationalStatusId] = 8 then 1 else 0 end) as NC
		, sum(case when v.[LastOperationalStatusId] = 9 then 1 else 0 end) as PL
		, sum(case when v.[LastOperationalStatusId] = 10 then 1 else 0 end) as RL
		, sum(case when v.[LastOperationalStatusId] = 11 then 1 else 0 end) as RP
		, sum(case when v.[LastOperationalStatusId] = 12 and v.LastMovementTypeId <> 10 then 1 else 0 end) as Idle
		, sum(case when v.[LastOperationalStatusId] = 13 then 1 else 0 end) as SU
		, sum(case when v.[LastOperationalStatusId] = 14 then 1 else 0 end) as SV
		, sum(case when v.[LastOperationalStatusId] = 15 then 1 else 0 end) as TB
		, sum(case when v.[LastOperationalStatusId] = 16 then 1 else 0 end) as TC
		, sum(case when v.[LastOperationalStatusId] = 17 then 1 else 0 end) as TN
		, sum(case when v.[LastOperationalStatusId] = 18 then 1 else 0 end) as TW
		, sum(case when v.[LastOperationalStatusId] = 19 then 1 else 0 end) as WS
		, sum(case when v.[LastOperationalStatusId] = 12 and
				v.LastMovementTypeId = 10 and v.ExpectedDateTime < cast(getdate() as date) 
				then 1 else 0 end) as Overdue

	from [dbo].Vehicle v
	join CAR_GROUPS cg on v.CarGroup = cg.car_group
	join CAR_CLASSES cc on cg.car_class_id = cc.car_class_id
	join CAR_SEGMENTS cs on cc.car_segment_id = cs.car_segment_id
	join LOCATIONS l on v.LastLocationCode = l.location
	where cs.country = v.OwningCountry
	group by cg.car_group_id, l.dim_Location_id, v.VehicleFleetTypeId
	)

	insert into [dbo].[FleetNow](Timestamp, CarGroupId, LocationId, FleetTypeId, SumTotal, SumBd, SumCu, SumFs, SumHa, SumHl, SumLl, SumMm, SumNc, SumPl, SumRl, SumRp, SumIdle, SumSu, SumSv, SumTb, SumTc, SumTn, SumTw, SumWs, SumOverdue, Utilization)
	
	SELECT getdate() as Timestamp, v.CarGroupId, v.dim_Location_id, v.VehicleFleetTypeId
	,sum(TotalFleet) as TotalFleet
	,sum(BD) as SumBD
	,sum(CU) as SumCU
	,sum(FS) as SumFS
	,sum(HA) as SumHA
	,sum(HL) as SumHL
	,sum(LL) as SumLL
	,sum(MM) as SumMM
	,sum(NC) as SumNC
	,sum(PL) as SumPL
	,sum(RL) as SumRL
	,sum(RP) as SumRP
	,sum(Idle) as SumIdle
	,sum(SU) as SumSU
	,sum(SV) as SumSV
	,sum(TB) as SumTB
	,sum(TC) as SumTC
	,sum(TN) as SumTN
	,sum(TW) as SumTW
	,sum(WS) as SumWS
	,sum(Overdue) as SumOverdue
	, case when 
			(sum(TotalFleet) - sum(CU) - sum(HA) - sum(HL) - sum(LL) - sum(NC) - sum(PL) - sum(TC) - sum(SV) - sum(WS)) = 0
			then 0
			else
			 
					cast(((sum(TotalFleet) - sum(CU) - sum(HA) - sum(HL) - sum(LL) - sum(NC) - sum(PL) - sum(TC) - sum(SV) - sum(WS) 
					- sum(BD) - sum(MM) - sum(TW) - sum(TB) - sum(FS) - sum(RL) - sum(RP) - sum(TN) ) --Available Fleet
					- sum(Idle) - sum(SU) - sum(Overdue)											  -- OnRent
					) as float)
				/
				(sum(TotalFleet) - sum(CU) - sum(HA) - sum(HL) - sum(LL) - sum(NC) - sum(PL) - sum(TC) - sum(SV) - sum(WS)) --Operational Fleet
			end
		as Utilization
	FROM VehicleStatus v
	--where cast(v.[TimeStamp] as date) = cast(getdate() as date)
	group by v.CarGroupId, v.dim_Location_id, v.VehicleFleetTypeId
END