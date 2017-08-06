﻿
CREATE procedure [dbo].[spReportFleetComparisonSU]
 @country    VARCHAR(10) = NULL ,
 @cms_pool_id   INT   = NULL ,
 @cms_location_group_id int   = NULL , 
 @ops_region_id   INT   = NULL ,
 @ops_area_id   INT   = NULL , 
 @location    VARCHAR(50) = NULL , 
 @fleet_name    VARCHAR(50) = NULL ,
 @car_segment_id   INT   = NULL ,
 @car_class_id   INT   = NULL , 
 @start_date    DATETIME   ,
 @end_date    DATETIME   ,
 @day_of_week   INT   = NULL , 
 @select_by    VARCHAR(10)
 
AS
 -- This SP has been generated by s_Generate_spReportFleetComparison
 -- Do not edit the SP - change the generator
/*
exec spReportFleetComparisonSU
 @start_date = '20120105' ,
 @end_date = '20120105' ,
 @select_by = 'VALUE'
*/
 
 SET NOCOUNT ON;
begin try

declare @rowcount int, @inserted int, @updated int
declare @entity varchar(100)
declare @key1 varchar(100)
declare @data2 varchar(max)

 select @entity = OBJECT_NAME(@@PROCID)
 select @entity = coalesce(@entity,'test')
 select @key1 = convert(varchar(20),@@spid)
 select @data2 =   '@country=' + coalesce(''''+@country+'''', 'null')
      + ',@cms_pool_id=' + coalesce(convert(varchar(20),@cms_pool_id), 'null')
      + ',@cms_location_group_id=' + coalesce(convert(varchar(20),@cms_location_group_id), 'null')
      + ',@ops_region_id=' + coalesce(convert(varchar(20),@ops_region_id), 'null')
      + ',@ops_area_id=' + coalesce(convert(varchar(20),@ops_area_id), 'null')
      + ',@location=' + coalesce(''''+@location+'''', 'null')
      + ',@fleet_name=' + coalesce(''''+@fleet_name+'''', 'null')
      + ',@car_segment_id=' + coalesce(convert(varchar(20),@car_segment_id), 'null')
      + ',@car_class_id=' + coalesce(convert(varchar(20),@car_class_id), 'null')
      + ',@start_date=' + coalesce(''''+convert(varchar(23),@start_date,126)+'''', 'null')
      + ',@end_date=' + coalesce(''''+convert(varchar(23),@end_date,126)+'''', 'null')
      + ',@day_of_week=' + coalesce(convert(varchar(20),@day_of_week), 'null')
      + ',@select_by=' + coalesce(''''+@select_by+'''', 'null')
 insert trace (entity, key1, key2, data2)
 select @entity, @key1, 'start', @data2

 CREATE TABLE #TMP_CAR_GROUPS(
  dim_calendar_id int ,
  CAR_GROUP  VARCHAR(3),
  TOPIC_COUNTER INT, --TOPIC: SU

  GROUP_COUNTER INT , --GROUP: TOTAL_FLEET
  NumDays   int ,
  rowtype   varchar(1)
  )

-- get start and end months for month table access
declare @StartFullPeriod datetime
declare @EndFullPeriod datetime
declare @StartDateEnd datetime
declare @EndDateStart datetime

  -- get full periods at aggregation level
  declare @periods table (dim_calendar_id int, dim_calendar_id_end int, PeriodStart datetime, PeriodEnd datetime, type int)
  if @day_of_week is null
  begin
   declare @type int
   select @type = 5
   while @type > 1
   begin
    select @type = @type - 1

    select @StartFullPeriod = MIN(PeriodStart) from @periods  
    select @EndFullPeriod = max(PeriodEnd) from @periods 
    select  @StartFullPeriod = coalesce(@StartFullPeriod,'25000101')
    select  @EndFullPeriod = coalesce(@EndFullPeriod,'19000101')
    
    insert @periods
    select m.min_dim_calendar_id, m.max_dim_calendar_id, m.PeriodStart, m.PeriodEnd, convert(varchar(20),Type)
    from inp.Data_Aggregate m
    where ( m.PeriodStart between @start_date and @end_date
      and m.PeriodEnd between @start_date and @end_date
      and ( m.PeriodEnd < @StartFullPeriod
       or m.PeriodStart > @EndFullPeriod
       )
      )
    and  m.Type = @Type
    
    select @StartFullPeriod = MIN(PeriodStart) from @periods  
    select @EndFullPeriod = max(PeriodEnd) from @periods 

    select @data2 = '@Type=' + coalesce(convert(varchar(20),@Type),'null')
         + ',@StartFullPeriod=' + '''' + coalesce(convert(varchar(19),@StartFullPeriod,126)+'''','null')
         + ',@EndFullPeriod=' + '''' + coalesce(convert(varchar(19),@EndFullPeriod,126)+'''','null')
    insert trace (entity, key1, key2, data2)
    select @entity, @key1, 'Aggregate', @data2
   end
  end
  else
  begin
   insert @periods
   select m.min_dim_calendar_id, m.max_dim_calendar_id, m.MonthStart, m.MonthEnd, 999
   from inp.MonthlyData m
   where m.MonthStart >= @start_date
   and  m.MonthEnd <= @end_date

  end

  select @StartFullPeriod = min(PeriodStart) ,
    @EndFullPeriod = max(PeriodEnd)
  from @periods
  
  if @StartFullPeriod > @EndFullPeriod or @StartFullPeriod is null or @EndFullPeriod is null
   select @StartDateEnd = @end_date ,
     @EndDateStart = null
  else
   select @StartDateEnd = @StartFullPeriod - 1 ,
     @EndDateStart = @EndFullPeriod + 1
  
  insert trace (entity, key1, Key2, Data1)
  select @entity, @key1, 'Periods', 'start=' + coalesce(convert(varchar(19),@start_date,126),'null')
           + ',@StartDateEnd=' + coalesce(convert(varchar(19),@StartDateEnd,126),'null')
           + ',@EndDateStart=' + coalesce(convert(varchar(19),@EndDateStart,126),'null')
           +  ',end=' + coalesce(convert(varchar(19),@end_date,126),'null')
           +  ',StartPeriod=' + coalesce(convert(varchar(19),@StartFullPeriod,126),'null')
           +  ',EndPeriod=' + coalesce(convert(varchar(19),@EndFullPeriod,126),'null')

 -- Locations
 declare @LocationFlag int = 0
 select @LocationFlag = 1
 where coalesce(@cms_pool_id, @cms_location_group_id, @ops_region_id, @ops_area_id, case when @location is not null then 1 else null end) is not null

 declare @Locations table (dim_Location_id int, Location varchar(100))
 if @LocationFlag = 1
  insert @Locations (dim_Location_id, Location)
  select FES.dim_Location_id, l.location
  from inp.dim_Location FES
   join inp.dim_Location l
    on FES.dim_Location_id = l.dim_Location_id
  where ((FES.LOCATION IN (SELECT L.location FROM LOCATIONS AS L INNER JOIN CMS_LOCATION_GROUPS AS CLG ON L.cms_location_group_id = CLG.cms_location_group_id WHERE CLG.cms_pool_id = @cms_pool_id)) OR @cms_pool_id IS NULL) -- CMS_POOLS
  AND ((FES.LOCATION IN (SELECT location FROM LOCATIONS WHERE cms_location_group_id = @cms_location_group_id)) OR @cms_location_group_id IS NULL) -- CMS_LOCATION_GROUPS
  AND ((FES.LOCATION IN (SELECT L.location FROM LOCATIONS AS L INNER JOIN OPS_AREAS AS OA ON L.ops_area_id = OA.ops_area_id WHERE OA.ops_region_id = @ops_region_id)) OR @ops_region_id IS NULL) -- OPS_REGIONS
  AND ((FES.LOCATION IN (SELECT location FROM LOCATIONS WHERE ops_area_id = @ops_area_id)) OR @ops_area_id IS NULL) -- OPS_AREAS
  AND (FES.LOCATION = @location OR @location IS NULL) -- Location
 else
  insert @Locations select dim_Location_id, l.location from inp.dim_Location l

  -- Car groups
  declare @CarGroupFlag int = 0
  select @CarGroupFlag = 1
  where coalesce(@car_segment_id, @car_class_id) is not null

 declare @CarGroups table (CarGroup varchar(10))
 if @CarGroupFlag = 1
  insert @CarGroups
  select distinct CAR_GROUP
  from CAR_GROUPS FES
  where ((FES.CAR_GROUP IN (SELECT CG.car_group FROM CAR_GROUPS AS CG INNER JOIN CAR_CLASSES AS CC ON CG.car_class_id = CC.car_class_id WHERE CC.car_segment_id = @car_segment_id)) OR @car_segment_id IS NULL) --@car_segment_id
  AND ((FES.CAR_GROUP IN (SELECT CG.car_group FROM CAR_GROUPS AS CG WHERE CG.car_class_id = @car_class_id)) OR @car_class_id IS NULL) --@car_class_id
 else
  insert @CarGroups select null

    
 -- daily data
 INSERT INTO #TMP_CAR_GROUPS(dim_calendar_id, CAR_GROUP, TOPIC_COUNTER, GROUP_COUNTER, NumDays, rowtype)
 SELECT 
  FES.dim_Calendar_id,  FES.CAR_GROUP, SUM(FES.SU), SUM(FES.TOTAL_FLEET), 1, 'd'
 FROM 
  inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY AS FES
   join inp.dim_Calendar d
    on d.dim_Calendar_id = FES.dim_Calendar_id
    join @Locations ltmp
     on ltmp.dim_Location_id = FES.dim_Location_id
    join inp.dim_location l
     on l.dim_Location_id = fes.dim_Location_id
   join @CarGroups cg
    on cg.CarGroup = FES.Car_Group
    or cg.CarGroup is null
 WHERE
  (FES.COUNTRY = @country OR @country IS NULL) -- Country
 AND ((@fleet_name = 'CARSALES' AND FES.FLEET_CARSALES > 0) OR (@fleet_name = 'RAC OPS' AND FES.FLEET_RAC_OPS > 0) OR (@fleet_name = 'RAC TTL' AND FES.FLEET_RAC_TTL > 0) OR (@fleet_name = 'ADVANTAGE' AND FES.FLEET_ADV > 0) OR (@fleet_name = 'HERTZ ON DEMAND' AND FES.FLEET_HOD > 0) OR (@fleet_name = 'LICENSEE' AND FES.FLEET_LICENSEE > 0) OR (@fleet_name IS NULL AND (FES.FLEET_CARSALES > 0 OR FES.FLEET_RAC_TTL > 0)  AND NOT(FLEET_ADV = 1) AND NOT(FLEET_HOD = 1) AND NOT(FLEET_LICENSEE = 1)))--Fleet
 AND ( d.REP_DATE between @start_date and @StartDateEnd 
  or  d.REP_DATE between @EndDateStart and @end_date
  )
 AND (d.Rep_DayOfWeek = @day_of_week OR @day_of_week IS NULL)
 GROUP BY 
  FES.dim_Calendar_id , FES.CAR_GROUP

 -- full monthly data
 if @day_of_week is null
 begin
  INSERT INTO #TMP_CAR_GROUPS(dim_calendar_id, CAR_GROUP, TOPIC_COUNTER, GROUP_COUNTER, NumDays, rowtype)
  SELECT 
   FES.dim_Calendar_id_start,  FES.CAR_GROUP, SUM(FES.SU), SUM(FES.TOTAL_FLEET), max(FES.NumDays), 'm'
  FROM 
   inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY_Aggregate AS FES
    join @periods m
     on FES.dim_Calendar_id_start = m.dim_calendar_id
     and m.type = FES.Type
    join @Locations ltmp
     on ltmp.dim_Location_id = FES.dim_Location_id
    join inp.dim_location l
     on l.dim_Location_id = fes.dim_Location_id
    join @CarGroups cg
     on cg.CarGroup = FES.Car_Group
     or cg.CarGroup is null
  WHERE
   (FES.COUNTRY = @country OR @country IS NULL) -- Country
     AND ((@fleet_name = 'CARSALES' AND FES.FLEET_CARSALES > 0) OR (@fleet_name = 'RAC OPS' AND FES.FLEET_RAC_OPS > 0) OR (@fleet_name = 'RAC TTL' AND FES.FLEET_RAC_TTL > 0) OR (@fleet_name = 'ADVANTAGE' AND FES.FLEET_ADV > 0) OR (@fleet_name = 'HERTZ ON DEMAND' AND FES.FLEET_HOD > 0) OR (@fleet_name = 'LICENSEE' AND FES.FLEET_LICENSEE > 0) OR (@fleet_name IS NULL AND (FES.FLEET_CARSALES > 0 OR FES.FLEET_RAC_TTL > 0)  AND NOT(FLEET_ADV = 1) AND NOT(FLEET_HOD = 1) AND NOT(FLEET_LICENSEE = 1)))--Fleet
  GROUP BY 
   FES.dim_Calendar_id_start , FES.CAR_GROUP
 end
 else
 begin
  INSERT INTO #TMP_CAR_GROUPS(dim_calendar_id, CAR_GROUP, TOPIC_COUNTER, GROUP_COUNTER, NumDays, rowtype)
  SELECT 
   FES.dim_calendar_id,  FES.CAR_GROUP, SUM(FES.SU), SUM(FES.TOTAL_FLEET), max(FES.NumDays), 'm'
  FROM 
   inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY_Month_DOW AS FES
    join @periods m
     on FES.dim_calendar_id = m.dim_calendar_id
     join @Locations ltmp
      on ltmp.dim_Location_id = FES.dim_Location_id
      or ltmp.dim_Location_id is null
     join inp.dim_location l
      on l.dim_Location_id = fes.dim_Location_id
    join @CarGroups cg
     on cg.CarGroup = FES.Car_Group
     or cg.CarGroup is null
  WHERE
   (FES.COUNTRY = @country OR @country IS NULL) -- Country
     AND ((@fleet_name = 'CARSALES' AND FES.FLEET_CARSALES > 0) OR (@fleet_name = 'RAC OPS' AND FES.FLEET_RAC_OPS > 0) OR (@fleet_name = 'RAC TTL' AND FES.FLEET_RAC_TTL > 0) OR (@fleet_name = 'ADVANTAGE' AND FES.FLEET_ADV > 0) OR (@fleet_name = 'HERTZ ON DEMAND' AND FES.FLEET_HOD > 0) OR (@fleet_name = 'LICENSEE' AND FES.FLEET_LICENSEE > 0) OR (@fleet_name IS NULL AND (FES.FLEET_CARSALES > 0 OR FES.FLEET_RAC_TTL > 0)  AND NOT(FLEET_ADV = 1) AND NOT(FLEET_HOD = 1) AND NOT(FLEET_LICENSEE = 1)))--Fleet
  AND FES.Rep_DayOfWeek = @day_of_week
  GROUP BY 
   FES.dim_calendar_id , FES.CAR_GROUP
 end


 -- DISPLAY NUMBERS OF CARS
 IF (@select_by = 'VALUE')
 BEGIN
  SELECT car_group = CAR_GROUP, convert(numeric(12,0),1.0 * sum(TOPIC_COUNTER) / sum(NumDays)) AS car_count
  FROM #TMP_CAR_GROUPS 
  GROUP BY CAR_GROUP 
  ORDER BY CAR_GROUP
  select @rowcount = @@rowcount
 END
 -- Display percentage of cars in the TOPIC
 ELSE IF (@select_by = 'PERCENTAGE')
 BEGIN 
  DECLARE @topicSum INT 
  SET @topicSum = (SELECT SUM(TOPIC_COUNTER) FROM #TMP_CAR_GROUPS)
  
  SELECT 
   CAR_GROUP AS car_group, 
   CASE WHEN (@topicSum = 0) THEN 0 ELSE (SUM(TOPIC_COUNTER)*100.0)/@topicSum END AS car_count
  FROM #TMP_CAR_GROUPS 
  GROUP BY CAR_GROUP 
  ORDER BY CAR_GROUP
  select @rowcount = @@rowcount
 END

 -- Drop the tempory table 
 DROP TABLE #TMP_CAR_GROUPS


 insert trace (entity, key1, key2, data1)
 select @entity, @key1, 'end', 'returned=' + coalesce(convert(varchar(20),@rowcount),'null')
end try
begin catch
declare @ErrDesc varchar(max)
declare @ErrProc varchar(128)
declare @ErrLine varchar(20)

 select  @ErrProc = ERROR_PROCEDURE() ,
  @ErrLine = ERROR_LINE() ,
  @ErrDesc = ERROR_MESSAGE()
 select @ErrProc = coalesce(@ErrProc,'') ,
  @ErrLine = coalesce(@ErrLine,'') ,
  @ErrDesc = coalesce(@ErrDesc,'')

 insert Trace (Entity, key1, data1, data2)
 select Entity = @entity, key1 = 'Failure',
  data1 = '<ErrProc=' + @ErrProc + '>'
   + '<ErrLine=' + @ErrLine + '>'
   + '<ErrDesc=' + @ErrDesc + '>',
  data2 = @key1
 raiserror('Failed %s', 16, -1, @ErrDesc)
end catch