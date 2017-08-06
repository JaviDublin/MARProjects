
CREATE PROCEDURE [dbo].[FleetSizeForecastUpdate] 
	@fleetPlanId int
	,@targetDate DateTime
	,@locGrp int
	,@carGrp int
	,@additions int
	,@deletions int
AS
BEGIN

declare @efAmount int=0,@opFleet int=0, @amount int=0,@country varchar(10)
set @amount=@additions-@deletions

select top 1 @efAmount=isnull(amount,0),@opFleet=isnull(OperationalFleet,0) from [dbo].[FleetSizeFutureTrend]
where FleetPlanId=@fleetPlanId and CarGrpId=@carGrp and LocGrpId=@locGrp and TargetDate>=@targetDate

select top 1 @country=Country 
from CMS_LOCATION_GROUPS cg 
join CMS_POOLS cp on cg.cms_pool_id=cp.cms_pool_id
where cg.cms_location_group_id=@locGrp

declare @tmp table(fleetPlanId int,targetDate datetime,locGrp int,carGrp int,amount int)
insert into @tmp
(fleetPlanId,targetDate,locGrp,carGrp,amount)
values
(@fleetPlanId,@targetDate,@locGrp,@carGrp,@amount)

merge [dbo].[FleetSizeFutureTrend] as t
using @tmp as s
on (t.[FleetPlanId]=s.fleetPlanId and t.[targetDate]>=s.targetDate
	 and t.LocGrpId=s.locGrp and t.carGrpId=s.carGrp)
when not matched by target
	then insert ([TargetDate],[OperationalFleet],[Amount],[ExpectedFleet]
      ,[CarGrpId],[LocGrpId],[FleetPlanId],[Country])
      values(s.targetDate,@opFleet,s.amount,@opFleet-s.amount,s.carGrp,s.locGrp,s.fleetPlanId,@country)
when matched
	then update set t.amount=t.amount+(s.amount-@efAmount),t.[ExpectedFleet]=@opFleet+t.amount+(s.amount-@efAmount);
END