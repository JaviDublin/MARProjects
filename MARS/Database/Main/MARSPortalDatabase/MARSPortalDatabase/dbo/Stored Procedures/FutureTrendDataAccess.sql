
CREATE PROCEDURE [dbo].[FutureTrendDataAccess] 

		 @StartDate DateTime
		,@EndDate DateTime
		,@fleetPlanId int=1
		,@locGrp varchar(4)=''
		,@pool varchar(4)=''
		,@carSeg varchar(4)=''
		,@carCls varchar(4)=''
		,@carGrp varchar(4)=''
		,@country varchar(2)=''
		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	declare @EMPTYSTRING varchar(1)=''

    SELECT [REP_DATE]
      ,sum([CONSTRAINED]) constrained
      ,sum([UNCONSTRAINED])unconstrained
      ,SUM(Reservations_booked) reservations_booked	
      ,SUM(CURRENT_ONRENT) Current_Onrent
      ,SUM(necessaryConstrained) NecessaryConstrained
      ,SUM(necessaryUnconstrained) NecessaryUnconstrained
      ,SUM(necessaryBooked) NecessaryBooked
into #mcf
FROM CmsForecastView mcf
where (mcf.REP_DATE>=@StartDate and mcf.REP_DATE<=@EndDate)
and (mcf.COUNTRY=@country or @country=null or @country=@EMPTYSTRING)
and (car_segment_id=@carSeg or @carSeg is null or @carSeg=@EMPTYSTRING)
and (car_class_id=@carCls or @carCls is null or @carCls=@EMPTYSTRING)
and (Car_Group_id=@carGrp or @carGrp is null or @carGrp=@EMPTYSTRING)
and (cms_pool_id=@pool or @pool is null or @pool=@EMPTYSTRING)
and (cms_location_group_id=@locGrp or @locGrp is null or @locGrp=@EMPTYSTRING)
group by REP_DATE
  
select targetDate
	  ,SUM(ExpectedFleet) ExpectedFleet
into #fsf
from FleetSizeFutureTrendView fsf
where (fsf.TargetDate>=@StartDate and fsf.TargetDate<=@EndDate)
and (fsf.Country=@country or @country=null or @country=@EMPTYSTRING)
and FleetPlanId=@fleetPlanId
and (car_segment_id=@carSeg or @carSeg is null or @carSeg=@EMPTYSTRING)
and (car_class_id=@carCls or @carCls is null or @carCls=@EMPTYSTRING)
and (CarGrpId=@carGrp or @carGrp is null or @carGrp=@EMPTYSTRING)
and (cms_pool_id=@pool or @pool is null or @pool=@EMPTYSTRING)
and (LocGrpId=@locGrp or @locGrp is null or @locGrp=@EMPTYSTRING)
group by TargetDate
  
select REP_DATE
		,isnull(cast(round(constrained,0) as int),0)constrained
		,isnull(cast(round(unconstrained,0) as int),0)unconstrained
		,isnull(cast(round(reservations_booked,0) as int),0)reservations_booked
		,isnull(cast(round(Current_Onrent,0) as int),0)Current_Onrent
		,cast(round(NecessaryConstrained,0) as int)NecessaryConstrained
		,cast(round(NecessaryUnconstrained,0) as int)NecessaryUnconstrained
		,cast(round(NecessaryBooked,0) as int)NecessaryBooked
		,isnull(ExpectedFleet,0)ExpectedFleet
from #mcf
left join #fsf on #mcf.REP_DATE=#fsf.TargetDate
order by REP_DATE

drop table #fsf,#mcf

END