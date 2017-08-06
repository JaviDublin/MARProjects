
create PROCEDURE [dbo].[FutureTrendDataAccessExcel] 
	
	@StartDate DateTime
	,@EndDate DateTime
	,@fleetPlanId int=1
	,@pool varchar(1)=''
	,@locGrp varchar(1)=''
	,@carSeg varchar(1)=''
	,@carCls varchar(1)=''
	,@carGrp varchar(1)=''
	,@country varchar(2)=''
AS
BEGIN
	
	SET NOCOUNT ON;

declare  @EMPTYSTRING varchar(1)=''
			,@GROUPBY varchar(1)='g'
			,@MINUSONE int=-1
			,@countryDesc varchar(20)=''
			
	if @country<>@EMPTYSTRING begin
		select @countryDesc=country_description from COUNTRIES where @country=country
	end 
	
    SELECT [REP_DATE]
      ,sum([CONSTRAINED]) constrained
      ,sum([UNCONSTRAINED])unconstrained
      ,SUM(Reservations_booked) reservations_booked	
      ,SUM(CURRENT_ONRENT) Current_Onrent
      ,SUM(necessaryConstrained) NecessaryConstrained
      ,SUM(necessaryUnconstrained) NecessaryUnconstrained
      ,SUM(necessaryBooked) NecessaryBooked
      ,case @country when @EMPTYSTRING then mcf.country else @country end country
      ,case when (@carSeg=@GROUPBY or @carCls=@GROUPBY or @carGrp=@GROUPBY) then mcf.Car_Segment_Id else @MINUSONE end Car_Segment_Id
      ,case when (@carCls=@GROUPBY or @carGrp=@GROUPBY) then mcf.Car_Class_Id else @MINUSONE end Car_Class_Id
      ,case @carGrp when @GROUPBY then mcf.Car_Group_Id else @MINUSONE end Car_Group_Id
      ,case when (@pool=@GROUPBY or @locGrp=@GROUPBY) then mcf.cms_pool_id else @MINUSONE end CMS_Pool_Id
      ,case @locGrp when @GROUPBY then mcf.CMS_Location_group_Id else @MINUSONE end CMS_Location_Group_Id
into #mcf 
FROM CmsForecastView mcf
where (mcf.REP_DATE>=@StartDate and mcf.REP_DATE<=@EndDate)
and (mcf.COUNTRY=@country or @country=null or @country=@EMPTYSTRING)
group by REP_DATE
	  ,case @country when @EMPTYSTRING then mcf.country else @country end 
      ,case when (@carSeg=@GROUPBY or @carCls=@GROUPBY or @carGrp=@GROUPBY) then mcf.Car_Segment_Id else @MINUSONE end
      ,case when (@carCls=@GROUPBY or @carGrp=@GROUPBY) then mcf.Car_Class_Id else @MINUSONE end
      ,case @carGrp when @GROUPBY then mcf.Car_Group_Id else @MINUSONE end
      ,case when (@pool=@GROUPBY or @locGrp=@GROUPBY) then mcf.cms_pool_id else @MINUSONE end
      ,case @locGrp when @GROUPBY then mcf.CMS_Location_group_Id else @MINUSONE end
      
select targetDate
	  ,SUM(ExpectedFleet) ExpectedFleet
	  ,case @country when @EMPTYSTRING then fsf.country else @country end country
      ,case when (@carSeg=@GROUPBY or @carCls=@GROUPBY or @carGrp=@GROUPBY) then fsf.Car_Segment_Id else @MINUSONE end Car_Segment_Id
      ,case when (@carCls=@GROUPBY or @carGrp=@GROUPBY) then fsf.Car_Class_Id else @MINUSONE end Car_Class_Id
      ,case @carGrp when @GROUPBY then fsf.CarGrpId else @MINUSONE end Car_Group_Id
      ,case when (@pool=@GROUPBY or @locGrp=@GROUPBY) then fsf.cms_pool_id else @MINUSONE end CMS_Pool_Id
      ,case @locGrp when @GROUPBY then fsf.LocGrpId else @MINUSONE end CMS_Location_Group_Id
into #fsf
from FleetSizeFutureTrendView fsf
where (fsf.TargetDate>=@StartDate and fsf.TargetDate<=@EndDate)
and (fsf.Country=@country or @country=null or @country=@EMPTYSTRING)
and FleetPlanId=@fleetPlanId
group by TargetDate
	  ,case @country when @EMPTYSTRING then fsf.country else @country end 
      ,case when (@carSeg=@GROUPBY or @carCls=@GROUPBY or @carGrp=@GROUPBY) then fsf.Car_Segment_Id else @MINUSONE end 
      ,case when (@carCls=@GROUPBY or @carGrp=@GROUPBY) then fsf.Car_Class_Id else @MINUSONE end
      ,case @carGrp when @GROUPBY then fsf.CarGrpId else @MINUSONE end 
      ,case when (@pool=@GROUPBY or @locGrp=@GROUPBY) then fsf.cms_pool_id else @MINUSONE end 
      ,case @locGrp when @GROUPBY then fsf.LocGrpId else @MINUSONE end

select REP_DATE
	  ,isnull(sum(cast(round(constrained,0) as int)),0)constrained
	  ,isnull(sum(cast(round(unconstrained,0) as int)),0)unconstrained
	  ,isnull(sum(cast(round(reservations_booked,0) as int)),0)reservations_booked
	  ,isnull(sum(cast(round(Current_Onrent,0) as int)),0)Current_Onrent
	  ,sum(cast(round(NecessaryConstrained,0) as int))NecessaryConstrained
	  ,sum(cast(round(NecessaryUnconstrained,0) as int))NecessaryUnconstrained
	  ,sum(cast(round(NecessaryBooked,0) as int))NecessaryBooked
	  ,isnull(sum(ExpectedFleet),0) ExpectedFleet
      ,case @country when @EMPTYSTRING then c.country_description else @countryDesc end country
      ,case when (@carSeg=@GROUPBY or @carCls=@GROUPBY or @carGrp=@GROUPBY) then cs.Car_Segment else CONVERT(varchar(2),@MINUSONE) end Car_Segment
      ,case when (@carCls=@GROUPBY or @carGrp=@GROUPBY) then cc.Car_Class else CONVERT(varchar(2),@MINUSONE) end Car_Class
      ,case @carGrp when @GROUPBY then cg.Car_Group else CONVERT(varchar(2),@MINUSONE) end Car_Group
      ,case when (@pool=@GROUPBY or @locGrp=@GROUPBY) then cp.cms_pool else CONVERT(varchar(2),@MINUSONE) end CMS_Pool
      ,case @locGrp when @GROUPBY then clg.CMS_Location_group else CONVERT(varchar(2),@MINUSONE) end CMS_Location_Group
from #mcf mcf
left join #fsf fsf on mcf.rep_date=fsf.TargetDate 
	and mcf.Car_Segment_Id=fsf.Car_Segment_Id
	and mcf.Car_Class_Id=fsf.Car_Class_Id
	and mcf.Car_Group_Id=fsf.Car_Group_Id
	and mcf.country=fsf.country
	and mcf.CMS_Pool_Id=fsf.CMS_Pool_Id
	and mcf.CMS_Location_Group_Id=fsf.CMS_Location_Group_Id
left join COUNTRIES c on mcf.country=c.country
left join CAR_SEGMENTS cs on mcf.Car_Segment_Id=cs.car_segment_id
left join CAR_CLASSES cc on mcf.Car_Class_Id=cc.car_class_id
left join CAR_GROUPS cg on mcf.Car_Group_Id=cg.car_group_id
left join CMS_POOLS cp on mcf.CMS_Pool_Id=cp.cms_pool_id
left join CMS_LOCATION_GROUPS clg on mcf.CMS_Location_Group_Id=clg.cms_location_group_id
group by REP_DATE 
	  ,case when @country=@EMPTYSTRING then c.country_description else @countryDesc end 
      ,case when (@carSeg=@GROUPBY or @carCls=@GROUPBY or @carGrp=@GROUPBY) then cs.Car_Segment else CONVERT(varchar(2),@MINUSONE) end
      ,case when (@carCls=@GROUPBY or @carGrp=@GROUPBY) then cc.Car_Class else CONVERT(varchar(2),@MINUSONE) end
      ,case @carGrp when @GROUPBY then cg.Car_Group else CONVERT(varchar(2),@MINUSONE) end
      ,case when (@pool=@GROUPBY or @locGrp=@GROUPBY) then cp.CMS_Pool else CONVERT(varchar(2),@MINUSONE) end
      ,case @locGrp when @GROUPBY then clg.CMS_Location_group else CONVERT(varchar(2),@MINUSONE) end
order by country,rep_date

drop table #mcf,#fsf
END