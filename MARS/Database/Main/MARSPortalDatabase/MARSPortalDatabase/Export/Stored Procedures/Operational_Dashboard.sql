
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Export].[Operational_Dashboard]

AS
BEGIN
	SET NOCOUNT ON;

	declare @start_date date
	set @start_date = dateadd(month, datediff(month, 0, getdate()), 0)
	declare @end_date date
	set @end_date = dateadd(day, -(day(dateadd(month, 1, getdate()))),dateadd(month, 1, getdate()))
	declare @year varchar(4)
	set @year = datepart(year , getdate())
	declare @month varchar(2)
	set @month = datepart(month , getdate())
	set @month = stuff(@month, 1 , 0 , replicate('0', 2 - len(@month)))

	if OBJECT_ID('tempdb..#location_ids') is not null drop table #location_ids
	create table #location_ids (LocationId int , LocationCode varchar(8), Country varchar(2))
	insert into #location_ids (LocationId , LocationCode, Country)
	select dim_Location_id , location , country
	from [dbo].[LOCATIONS] 
	where country in ('BE',  'FR' ,  'GE',  'IT' ,  'LU',  'NE' ,  'SP',  'UK')

	if OBJECT_ID('tempdb..#car_segments') is not null drop table #car_segments
	create table #car_segments (CarGroupId int , CarSegment varchar(3), Country varchar(2))
	insert into #car_segments (CarGroupId , CarSegment , Country)
	select 
		cg.car_group_id ,
		case when cs.car_segment = 'Van' or cs.car_segment = 'Vans' then 'VAN' else 'CAR' end,
		cs.country
	from [dbo].[CAR_GROUPS] cg
	inner join [dbo].[CAR_CLASSES] cc on cg.car_class_id = cc.car_class_id
	inner join [dbo].[CAR_SEGMENTS] cs on cc.car_segment_id = cs.car_segment_id

	if OBJECT_ID('tempdb..#fleet_type') is not null drop table #fleet_type
	create table #fleet_type (FleetTypeId int , Brand varchar(5))
	insert into #fleet_type
	select 
		VehicleFleetTypeId , 
		case 
			when FleetTypeName = 'RAC OPS'			then 'RAC'
			when FleetTypeName = 'RAC NON OPS'		then 'RAC'
			when FleetTypeName = 'CARSALES'			then 'RAC'
			when FleetTypeName = 'FIREFLY'			then 'FF'
			when FleetTypeName = 'HERTZ ON DEMAND'	then 'HOD'
		end
	from [dbo].[VehicleFleetType]
	where FleetTypeName in ('RAC OPS','RAC NON OPS','CARSALES','FIREFLY','HERTZ ON DEMAND')

	select 
	l.Country					as [COUNTRY],
	ft.Brand					as [BRAND],
	@year						as [REP_YEAR],
	@month						as [REP_MONTH],
	fh.[Timestamp]				as [REP_DATE],
	l.LocationCode				as [LOCATION],
	cs.CarSegment				as [CAR_SEGMENT],
	sum(fh.AvgTotal)			as [TOTAL_FLEET],
	sum(fh.AvgWs)				as [WHOLESALE],
	sum(fh.AvgSv)				as [SALVAGE],
	sum(fh.AvgOperationalFleet)	as [OPERATIONAL_FLEET],
	sum(fh.AvgBd)				as [BODY_DAMAGE],
	sum(fh.AvgMm)				as [MAINTENANCE],
	sum(fh.AvgTb)				as [TURNBACK],
	sum(fh.AvgAvailableFleet)	as [AVAILABLE_FLEET],
	sum(fh.AvgOverdue)			as [OVERDUE],
	sum(fh.AvgOnRent)			as [ON_RENT],
	sum(fh.PeakOnRent)			as [ON_RENT_PEAK],
	sum(fh.PeakAvailableFleet)	as [AVAILABLE_FLEET_PEAK],
	sum(fh.AvgIdle) +
	sum(fh.AvgSu)				as [NON_REV_RENTABLE],
	((sum(fh.AvgOperationalFleet) - sum(fh.AvgOnRent)) 
	- (sum(fh.AvgIdle) + sum(fh.AvgSu))
	- sum(fh.AvgOverdue))		as [NON_REV_OTHER]
	from [dbo].[FleetHistory] fh
	inner join #location_ids l	on fh.LocationId	= l.LocationId
	inner join #car_segments cs on fh.CarGroupId	= cs.CarGroupId
	inner join #fleet_type ft	on fh.FleetTypeId	= ft.FleetTypeId
	where 
		fh.[Timestamp] >= @start_date and fh.[Timestamp] <= @end_date
	group by 
		l.Country , ft.Brand , fh.[Timestamp] , fh.[Timestamp] , l.LocationCode , cs.CarSegment

	drop table #location_ids
	drop table #car_segments
	drop table #fleet_type


END