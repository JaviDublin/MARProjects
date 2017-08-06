

CREATE PROCEDURE [Export].[GenerateGEForecast]

AS
BEGIN

	SET NOCOUNT ON;

declare
	@_country as Varchar(2) = 'GE' -- the country to be used
	,@_NonRev Int = 7
	,@logEntry Int = null
	,@errorMessage varchar(200)='[Export].[GenerateGEForecast], '

	--=========Insert Entry in Export.Log=============
	INSERT INTO [Export].[Log]
           ([Message],[DateTime])
     VALUES('[Export].[GenerateGEForecast] automated upload begins.',GETDATE())
     
     SELECT TOP 1 @logEntry = [Id]
	 FROM [Export].[Log]
     order by Id desc

begin try
	--=========The NonRev7 from the Fleet_Europe_Actual Table===============
	declare @feaTable Table(importTime datetime, locGrp int, carGrp varchar(10), daysrev int)
	insert @feaTable
	SELECT IMPORTTIME, LOC_GROUP, VC, sum(DAYSREV)
	  FROM [dbo].[FLEET_EUROPE_ACTUAL] fea
	  where fea.COUNTRY = @_country and DAYSREV>=@_NonRev
	  group by IMPORTTIME, LOC_GROUP, VC
end try
begin catch
	set @errorMessage+=' generating @feaTable for NonRev7.'
	goto writeErrorMessage;
end catch 	  

--select * from @feaTable  

--==========The OnRentMax from the Fleet_Europe_Summary_history Table==============
begin try
	declare @feshTable table(repDate dateTime, carGrp varchar(10), locGrpId int
		,OnRentMax int, OnRent int,OrAvg decimal(6,2)
		, Maintenance int, MaintenanceMax int
		, UnavailableCars int , UnavailableCarsMax int
		, ServiceUnits int, ServiceUnitsMax int
		, Overdue int, OverdueMax int
		,CarSales int, CarsalesMax int
		,Turnback int, TurnbackMax int
		,Rt int,RtMax int
		,OpFleetMax int, OpFleet int)
	insert @feshTable
	/*
	SELECT	 REP_DATE
			,fesh.car_group
			,loc.cms_location_group_id
			,sum(ON_RENT_MAX) OnRentMax
			,AVG(on_rent) ORAvg
			,sum(on_rent) onRent
			,SUM(MM)+SUM(BD)+SUM(TW) Maintenance
			,SUM(MM_MAX)+SUM(BD_MAX)+SUM(TW_MAX) MaintenanceMax
			,SUM(cu)+SUM(ha)+SUM(hl)+SUM(ll)+SUM(nc)+SUM(pl)+SUM(tc)+SUM(sv)+SUM(fs)+SUM(rl)+SUM(rp)+SUM(tn) UnavailableCars
			,SUM(cu_max)+SUM(ha_max)+SUM(hl_max)+SUM(ll_max)+SUM(nc_max)+SUM(pl_max)+SUM(tc_max)+SUM(sv_max)+SUM(fs_max)+SUM(rl_max)+SUM(rp_max)+SUM(tn_max) UnavailableCarsMax
			,SUM(su) ServiceUnits
			,SUM(su_max) ServiceUnitsMax
			,SUM(OVERDUE_MAX) OverdueMax
			,SUM(OVERDUE) Overdue
			,SUM(WS_MAX) CarsalesMax
			,SUM(WS) Carsales
			,Sum(TB_Max) TurnbackMax
			,Sum(TB) Turnback
			,SUM(RT_MAX) RtMax
			,SUM(RT) Rt
			,sum(OPERATIONAL_FLEET_MAX) OPFleetMax
			,SUM(OPERATIONAL_FLEET) OpFleet
	  FROM inp.[FLEET_EUROPE_SUMMARY_history] fesh
	  join inp.dim_Location1 loc on fesh.location = loc.location
	  where Rep_Date= convert(date,DATEADD(day,-1,getdate()))
	  and fesh.COUNTRY=@_country
	  and (fesh.FLEET_RAC_TTL>0 or fesh.FLEET_CARSALES>0)
	  group by REP_DATE, fesh.car_group, loc.cms_location_group_id
	  */
	  SELECT	fesh.Timestamp as REP_DATE
			, cg.car_group
			,l.cms_location_group_id
			,sum(fesh.MaxOnRent) as  OnRentMax
			,AVG(fesh.AvgOnRent) ORAvg
			,sum(fesh.MaxOnRent) onRent
			,SUM(fesh.AvgMm)+SUM(fesh.AvgBd)+SUM(AvgTw) Maintenance
			,SUM(fesh.MaxMm)+SUM(fesh.MaxBd)+SUM(fesh.MaxTw) MaintenanceMax
			,SUM(AvgCu)+SUM(avgHa)+SUM(AvgHl)+SUM(AvgLl)+SUM(AvgNc)+SUM(AvgPl)+SUM(AvgTc)+SUM(AvgSv)
					+SUM(AvgFs)+SUM(AvgRl)+SUM(AvgRp)+SUM(AvgTn) UnavailableCars
			,SUM(MaxCu)+SUM(maxHa)+SUM(MaxHl)+SUM(MaxLl)+SUM(MaxNc)+SUM(MaxPl)+SUM(MaxTc)
					+SUM(MaxSv)+SUM(MaxFs)+SUM(MaxRl)+SUM(MaxRp)+SUM(MaxTn) UnavailableCarsMax
			,SUM(AvgSu) ServiceUnits
			,SUM(MaxSu) ServiceUnitsMax
			,SUM(MaxOverdue) OverdueMax
			,SUM(AvgOverdue) Overdue
			,SUM(MaxWs) CarsalesMax
			,SUM(AvgWs) Carsales
			,Sum(MaxTb) TurnbackMax
			,Sum(AvgTb) Turnback
			,SUM(MaxOnRent) RtMax
			,SUM(AvgOnRent) Rt
			,sum(MaxOperationalFleet) OPFleetMax
			,SUM(AvgOperationalFleet) OpFleet
	  FROM fleetHistory fesh
	  join locations l on fesh.locationId = l.dim_location_id
	  join [dbo].[CAR_GROUPS] cg on fesh.carGroupId = cg.[car_group_id]
	  join [dbo].[CAR_CLASSES] cc on cg.car_class_id = cc.car_class_id
	  join [dbo].[CAR_SEGMENTS] cs on cc.car_segment_id = cs.car_segment_id
	  where Timestamp= cast((getdate() -1) as date)
	  and cs.COUNTRY='GE'
	  and fesh.FleetTypeId in (4,5,6)

	  group by Timestamp, cg.car_group, l.cms_location_group_id
end try
begin catch
	set @errorMessage+=' building @feshTable which contains all the max and avgs.'
	goto writeErrorMessage;
end catch
  
 --select * from @feshTable
 begin try
	truncate table [Export].[GermanyForecast]
 end try
 begin catch
	set @errorMessage+=' truncating [Export].[GermanyForecast].'
	goto writeErrorMessage;
 end catch
 
 begin try
	 insert into Export.GermanyForecast
				([Repdate]
			   ,[Country]
			   ,[CmsLocationGroupId]
			   ,[CmsLocationGroup]
			   ,[CmsPool]
			   ,[CarClassId]
			   ,[CarClass]
			   ,[Constrained]
			   ,[Unconstrained]
			   ,[FleetForecast]
			   ,[ReservationsBooked]
			   ,[CurrentOnRent]
			   ,[OnRentLy]
			   ,[AvailableFleet]
			   ,[AdjustmentBu1]
			   ,[AdjustmentBu2]
			   ,[AdjustmentRc]
			   ,[AdjustmentTd]
			   ,[TotalFleet]
			   ,[Additions]
			   ,[Deletions]
			   ,[Amount]
			   ,[NonRev7]
			   ,[OnRentMax]
			   ,[OnRentAvg]
			   ,[OnRent]
			   ,[Maintenance]
			   ,[MaintenanceMax]
			   ,[Unavailable]
			   ,[UnavailableMax]
			   ,[ServiceUnits]
			   ,[ServiceUnitsMax]
			   ,[Overdue]
			   ,[OverdueMax]
			   ,[CarSales]
			   ,[CarSalesMax]
			   ,[Turnback]
			   ,[TurnbackMax]
			   ,[RtMax]
			   ,[Rt]
			   ,[OpFleet]
			   ,[OpFleetMax]
			   ,[OpFleetCalc]
			   ,[ExpectedFleet])	           
	SELECT distinct
		   mcf.[REP_DATE] Repdate
		  ,mcf.[COUNTRY] Country
		  ,isnull(mcf.[CMS_LOCATION_GROUP_ID],0) CmsLocationGroupId
		  ,isnull(clg.cms_location_group,0) CmsLocationGroup
		  ,isnull(cp.cms_pool,0) CmsPool
		  ,isnull(mcf.[CAR_CLASS_ID],0) CarClassId
		  ,isnull(cc.CAR_CLASS,0) CarClass
		  ,isnull([CONSTRAINED],0) Constrained
		  ,isnull([UNCONSTRAINED],0) Unconstrained
		  ,isnull([FLEET],0) Fleet
		  ,isnull([RESERVATIONS_BOOKED],0)ReservationsBooked
		  ,isnull([CURRENT_ONRENT],0) CurrentOnRent
		  ,isnull([ONRENT_LY],0) OnRentLy
		  ,isnull([AVAILABLE_FLEET],0) AvailableFleet
		  ,isnull(mcfa.ADJUSTMENT_BU1,0) AdjustmentBu1
		  ,isnull(mcfa.ADJUSTMENT_BU2,0) AdjustmentBu2
		  ,isnull(mcfa.ADJUSTMENT_RC,0)AdjustmentRc
		  ,isnull(mcfa.ADJUSTMENT_TD,0) AdjustmentTd
		  ,isnull([TOTAL_FLEET],0) TotalFleet
		  ,0
		  ,0
		  ,0
		  ,isnull(ft.daysrev,0) NonRev7
		  ,ISNULL(fst.OnRentMax, 0) OnRentMax
		  ,ISNULL(fst.OrAvg,0)
		  ,ISNULL(fst.onRent,0)
		  ,ISNULL(fst.Maintenance,0)
		  ,ISNULL(fst.MaintenanceMax,0)
		  ,ISNULL(fst.UnavailableCars,0)
		  ,ISNULL(fst.UnavailableCarsMax,0)
		  ,ISNULL(fst.ServiceUnits,0)
		  ,ISNULL(fst.ServiceUnitsMax,0)
		  ,ISNULL(fst.Overdue,0)
		  ,ISNULL(fst.OverdueMax,0)
		  ,ISNULL(fst.CarSales,0)
		  ,ISNULL(fst.CarSalesMax,0)
		  ,ISNULL(fst.Turnback,0)
		  ,ISNULL(fst.TurnbackMax,0)
		  ,ISNULL(fst.RtMax,0)
		  ,ISNULL(fst.Rt,0)
		  ,ISNULL(fst.OpFleet,0)
		  ,ISNULL(fst.OpFleetMax,0)
		  ,ISNULL(fst.OpFleet,0)
		  ,0
	  FROM MARS_CMS_FORECAST as mcf
	  join CMS_LOCATION_GROUPS as clg on mcf.cms_location_group_id = clg.cms_location_group_id
	  join CMS_POOLS as cp on clg.cms_pool_id = cp.cms_pool_id
	  join MARS_CMS_FORECAST_ADJUSTMENT as mcfa on mcf.REP_DATE = mcfa.REP_DATE 
		and mcf.CMS_LOCATION_GROUP_ID = mcfa.CMS_LOCATION_GROUP_ID
		and mcf.CAR_CLASS_ID = mcfa.CAR_CLASS_ID
	  join vw_Mapping_CarClass cc on mcf.CAR_CLASS_ID=cc.CAR_CLASS_ID
	  left join @feaTable ft on mcf.CMS_LOCATION_GROUP_ID=ft.locGrp and cc.CAR_CLASS=ft.carGrp
	  left join @feshTable fst on mcf.CMS_LOCATION_GROUP_ID=fst.locGrpId and cc.CAR_CLASS=fst.carGrp
	  where mcf.COUNTRY = @_country
	  order by mcf.REP_DATE
  end try
  begin catch
	set @errorMessage+=' inserting into [Export].[GermanyForecast].'
	goto writeErrorMessage;
  end catch
  
  begin try
	  update Export.GermanyForecast
	  set [Additions] = fpd.addition
		  ,[Deletions] = fpd.deletion
		  ,[Amount] = fpd.amount
		  ,[opFleetCalc] = OpFleetCalc+fpd.amount
	   from MARS_CMS_FleetPlanDetails as fpd 
	   where Export.GermanyForecast.CmsLocationGroupId = fpd.cms_Location_Group_ID
			and Export.GermanyForecast.CarClassId = fpd.car_class_id 
			and Export.GermanyForecast.RepDate = fpd.targetDate
  end try
  begin catch
	set @errorMessage+=' updating [Export].[GermanyForecast] with Mars_CMS_FleetPlanDetails data.'
	goto writeErrorMessage;
  end catch
	    	  
  return	  
  
  writeErrorMessage:
	set @errorMessage+=' SQL Error = '
	set @errorMessage+=ERROR_MESSAGE()
    	INSERT into [Export].[Error_Log]
           ([Message],[DateTime])
    VALUES (@errorMessage, GETDATE())
           
    UPDATE [Export].[Log]
	SET [ErrorLog_Id] = (select top 1 Id from [Export].[Error_Log] ORDER BY Id desc)
	WHERE Id=@logEntry
	return
  end