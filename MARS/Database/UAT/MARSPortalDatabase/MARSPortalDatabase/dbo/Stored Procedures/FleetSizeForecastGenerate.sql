-- =============================================
-- Author:		Gavin Williams
-- Create date: 17 Dec 2012
-- Description:	Generates the data for the Fleet Size Forecast Expected fleet, stored in the FleetSizeFutureTrend table
-- =============================================
CREATE PROCEDURE [dbo].[FleetSizeForecastGenerate]
	
AS
BEGIN

	SET NOCOUNT ON;


declare @startDate dateTime
		,@numberOfDays int=90
		,@DEBUG int = 0
		,@status int = 0
		
select top 1 @status=Status_Id
from [MarsLogView] order by id desc
--if @status=2 return;

INSERT INTO [MarsLogView] ([Message],[DateTime],[Status_Id],[LogType_Id])
VALUES ('Please wait for current process to finish - Running stored procedure',GETDATE(),2,3)

select top 1  @startDate = IMPORTDATE
from FLEET_EUROPE_STATS order by IMPORTDATE

declare @fesTable table(cms_location_group_id int, car_group_id int, operationalFleet int, COUNTRY varchar(20))
insert @fesTable
SELECT fes.cms_location_group_id, fes.car_group_id, SUM(fes.operational_fleet) operationalFleet, fes.COUNTRY
FROM FEA_Location_CarGroup fes 
where fes.IMPORTDATE = @startDate
GROUP BY FES.IMPORTDATE, fes.cms_location_group_id, fes.car_group_id, fes.COUNTRY

INSERT INTO [MarsLogView] ([Message],[DateTime],[Status_Id],[LogType_Id])
VALUES ('Please wait for current process to finish - Deleting FleetSizeFutureTrend',GETDATE(),2,3)
    
truncate table [FleetSizeFutureTrend]

INSERT INTO [MarsLogView] ([Message],[DateTime],[Status_Id],[LogType_Id])
VALUES ('Please wait for current process to finish - Inserting Operational_Fleet',GETDATE(),2,3)

SELECT cal.Rep_Date TargetDate,OperationalFleet,0 Amount,isnull(car_group_id, -999)CarGrpId,isnull(cms_location_group_id,-999) LocGrpId,1 FleetPlanId,COUNTRY
into #t1
FROM @fesTable	
join Inp.dim_Calendar cal on Rep_Date<= cal.Rep_Date and Rep_Date>=DATEadd(day, -1, GETDATE())
where (cal.rep_Date>=DATEadd(day, -1, GETDATE()) and Rep_Date<=DATEadd(day, @numberOfDays, GETDATE()))

insert into #t1
SELECT cal.Rep_Date TargetDate,OperationalFleet,0 Amount,isnull(car_group_id, -999)CarGrpId,isnull(cms_location_group_id,-999) LocGrpId,2 FleetPlanId,COUNTRY
FROM @fesTable	
join Inp.dim_Calendar cal on Rep_Date<= cal.Rep_Date and Rep_Date>=DATEadd(day, -1, GETDATE())
where (cal.rep_Date>=DATEadd(day, -1, GETDATE()) and Rep_Date<=DATEadd(day, @numberOfDays, GETDATE()))

insert into #t1
SELECT cal.Rep_Date TargetDate,OperationalFleet,0 Amount,isnull(car_group_id, -999)CarGrpId,isnull(cms_location_group_id,-999) LocGrpId,3 FleetPlanId,COUNTRY
FROM @fesTable	
join Inp.dim_Calendar cal on Rep_Date<= cal.Rep_Date and Rep_Date>=DATEadd(day, -1, GETDATE())
where (cal.rep_Date>=DATEadd(day, -1, GETDATE()) and Rep_Date<=DATEadd(day, @numberOfDays, GETDATE()))

insert into #t1
SELECT cal.Rep_Date TargetDate,OperationalFleet,0 Amount,isnull(car_group_id, -999)CarGrpId,isnull(cms_location_group_id,-999) LocGrpId,4 FleetPlanId,COUNTRY
FROM @fesTable	
join Inp.dim_Calendar cal on Rep_Date<= cal.Rep_Date and Rep_Date>=DATEadd(day, -1, GETDATE())
where (cal.rep_Date>=DATEadd(day, -1, GETDATE()) and Rep_Date<=DATEadd(day, @numberOfDays, GETDATE()))


INSERT INTO [MarsLogView] ([Message],[DateTime],[Status_Id],[LogType_Id])
VALUES ('Please wait for current process to finish - Inserting FleetPlanDetails',GETDATE(),2,3)

SELECT cal.Rep_Date TargetDate, 0 OperationalFleet,sum(Amount) Amount, fpd.car_class_id CarGrpId, fpd.cms_Location_Group_ID LocGrpId, fpe.fleetPlanID FleetPlanId, fpe.Country Country
into #t2
FROM [MARS_CMS_FleetPlanDetails] fpd
join MARS_CMS_FleetPlanEntry fpe on fpd.fleetPlanEntryID=fpe.PKID
join Inp.dim_Calendar cal on targetDate<= cal.Rep_Date and targetDate>=DATEadd(day, -1, GETDATE())
where (cal.rep_Date>=DATEadd(day, -1, GETDATE()) and Rep_Date<=DATEadd(day, @numberOfDays, GETDATE()))
group by Rep_Date, fpd.car_class_id, fpd.cms_Location_Group_ID, fpe.fleetPlanID, fpe.Country

INSERT INTO [MarsLogView] ([Message],[DateTime],[Status_Id],[LogType_Id])
VALUES ('Please wait for current process to finish - Joining tables',GETDATE(),2,3)	

select * into #t3 from #t1 t1 union select * from #t2

INSERT INTO [MarsLogView] ([Message],[DateTime],[Status_Id],[LogType_Id])
VALUES ('Please wait for current process to finish - Inserting into database',GETDATE(),2,3)

insert into FleetSizeFutureTrend
select TargetDate, sum(OperationalFleet), sum(Amount),SUM(OperationalFleet)+SUM(amount), CarGrpId, LocGrpId, FleetPlanId, Country
from #t3 t3
group by t3.COUNTRY, t3.TargetDate, t3.FleetPlanId, t3.CarGrpId, t3.LocGrpId
order by t3.COUNTRY, t3.TargetDate, t3.FleetPlanId, t3.CarGrpId, t3.LocGrpId

INSERT INTO [MarsLogView] ([Message],[DateTime],[Status_Id],[LogType_Id])
VALUES ('Please wait for current process to finish - Temp tables dropped',GETDATE(),2,3)

drop table #t1; drop table #t2; drop table #t3;

INSERT INTO [MarsLogView]
           ([Message],[DateTime],[Status_Id],[LogType_Id])
     VALUES
           ('Ready for upload.....',GETDATE(),3,3)

if @DEBUG<1 return

select TargetDate, Country, SUM(operationalFleet), SUM(amount), SUM(ExpectedFleet), fleetplanid
from FleetSizeFutureTrend
where fleetplanid=1
group by country, TargetDate, fleetplanId
order by country, TargetDate, fleetplanid

--select *
--from FleetSizeFutureTrend
--where fleetPlanID=1
--and country='ge'
--and targetdate='2013-01-25'
--order by carGrpId, locgrpid


END