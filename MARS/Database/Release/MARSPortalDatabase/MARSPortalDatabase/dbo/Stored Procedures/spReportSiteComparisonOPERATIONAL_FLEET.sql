﻿
CREATE procedure [dbo].[spReportSiteComparisonOPERATIONAL_FLEET]
-- Add the parameters for the stored procedure here
@country VARCHAR(10) = NULL,
@cms_pool_id INT = NULL,
@cms_location_group_id int = NULL, --@cms_location_group_code VARCHAR(10) = NULL,
@ops_region_id INT = NULL,
@ops_area_id INT = NULL,  
@fleet_name VARCHAR(50) = NULL,
@car_segment_id INT = NULL,
@car_class_id INT = NULL,
@car_group_id INT = NULL,
@start_date DATETIME,
@end_date DATETIME,
@day_of_week INT = NULL, 
@select_by VARCHAR(10),
@grouping_criteria VARCHAR(20)
AS
 -- This SP has been generated by s_Generate_spReportSiteComparison
 -- Do not edit the SP - change the generator
/*
exec spReportSiteComparisonOPERATIONAL_FLEET
@start_date = '20110120' ,
@end_date = '20120410' ,
@select_by = 'VALUE' ,
@grouping_criteria = 'LOCATION'

exec spReportSiteComparisonOPERATIONAL_FLEET
@start_date = '20120717' ,
@end_date = '20120717 23:59:59' ,
@country='GE' ,
@car_segment_id=4 ,
@select_by = 'VALUE' ,
@grouping_criteria = 'CMS_POOL'
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
      + ',@fleet_name=' + coalesce(''''+@fleet_name+'''', 'null')
      + ',@car_segment_id=' + coalesce(convert(varchar(20),@car_segment_id), 'null')
      + ',@car_class_id=' + coalesce(convert(varchar(20),@car_class_id), 'null')
      + ',@car_group_id=' + coalesce(convert(varchar(20),@car_group_id), 'null')
      + ',@start_date=' + coalesce(''''+convert(varchar(23),@start_date,126)+'''', 'null')
      + ',@end_date=' + coalesce(''''+convert(varchar(23),@end_date,126)+'''', 'null')
      + ',@day_of_week=' + coalesce(convert(varchar(20),@day_of_week), 'null')
      + ',@select_by=' + coalesce(''''+@select_by+'''', 'null')
      + ',@grouping_criteria=' + coalesce(''''+@grouping_criteria+'''', 'null')
 insert trace (entity, key1, key2, data2)
 select @entity, @key1, 'start', @data2

 -- Insert statements for procedure here
 CREATE TABLE #TMP_LOCATIONS
  (
  dim_calendar_id int,
  COUNTRY VARCHAR(2) COLLATE Latin1_General_CI_AS,
  LOCATION VARCHAR(7) COLLATE Latin1_General_CI_AS,
  TOPIC_COUNTER INT, --TOPIC: OPERATIONAL_FLEET
  GROUP_COUNTER INT, --GROUP: TOTAL_FLEET
  NumDays    int ,   
  rowtype    varchar(1)
  )

 CREATE TABLE #TMP_GROUPS(
  dim_calendar_id int,
  GROUP_ID VARCHAR(10) COLLATE Latin1_General_CI_AS,
  GROUP_NAME VARCHAR(50) COLLATE Latin1_General_CI_AS,
  TOPIC_COUNTER INT, --TOPIC: RT
  GROUP_COUNTER INT, --GROUP: AVAILABLE_FLEET
  NumDays    int
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
   select @type = 4  -- do not use 5
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
 where coalesce(@cms_pool_id, @cms_location_group_id, @ops_region_id, @ops_area_id) is not null

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
 else
  insert @Locations select dim_Location_id, l.location from inp.dim_Location l
 
 -- Car groups
 declare @CarGroupFlag int = 0
 select @CarGroupFlag = 1
 where coalesce(@car_segment_id, @car_class_id, @car_group_id) is not null
 
 declare @CarGroups table (CarGroup varchar(10))
 if @CarGroupFlag = 1
  insert @CarGroups
  select distinct CAR_GROUP
  from CAR_GROUPS FES
  where ((FES.CAR_GROUP IN (SELECT CG.car_group FROM CAR_GROUPS AS CG INNER JOIN CAR_CLASSES AS CC ON CG.car_class_id = CC.car_class_id WHERE CC.car_segment_id = @car_segment_id)) OR @car_segment_id IS NULL) --@car_segment_id
  AND ((FES.CAR_GROUP IN (SELECT CG.car_group FROM CAR_GROUPS AS CG WHERE CG.car_class_id = @car_class_id)) OR @car_class_id IS NULL) --@car_class_id
  AND ((FES.CAR_GROUP = (SELECT car_group FROM CAR_GROUPS WHERE car_group_id = @car_group_id)) OR @car_group_id IS NULL) --@car_group_id
 else
  insert @CarGroups select null
 
 -- daily data
 INSERT INTO #TMP_LOCATIONS(dim_calendar_id, COUNTRY, LOCATION, TOPIC_COUNTER, GROUP_COUNTER, NumDays, rowtype)
 SELECT 
  FES.dim_Calendar_id, FES.COUNTRY, l.LOCATION, SUM(FES.OPERATIONAL_FLEET), SUM(FES.TOTAL_FLEET), 1, 'd' 
 FROM 
  inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY AS FES
   join inp.dim_Calendar d
    on d.dim_Calendar_id = FES.dim_Calendar_id
    join @Locations l
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
  FES.dim_Calendar_id, l.LOCATION, FES.COUNTRY
 
 if @day_of_week is null
 begin
  INSERT INTO #TMP_LOCATIONS(dim_calendar_id, COUNTRY, LOCATION, TOPIC_COUNTER, GROUP_COUNTER, NumDays, rowtype)
  SELECT 
   FES.dim_Calendar_id_start, FES.COUNTRY, l.LOCATION, SUM(FES.OPERATIONAL_FLEET), SUM(FES.TOTAL_FLEET), max(FES.NumDays), 'm' 
  FROM 
   inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY_Aggregate AS FES
    join @periods m
     on m.dim_Calendar_id = FES.dim_Calendar_id_start
     and m.type = FES.Type
    join @Locations l
     on l.dim_Location_id = fes.dim_Location_id
    join @CarGroups cg
     on cg.CarGroup = FES.Car_Group
     or cg.CarGroup is null
  WHERE
   (FES.COUNTRY = @country OR @country IS NULL) -- Country
     AND ((@fleet_name = 'CARSALES' AND FES.FLEET_CARSALES > 0) OR (@fleet_name = 'RAC OPS' AND FES.FLEET_RAC_OPS > 0) OR (@fleet_name = 'RAC TTL' AND FES.FLEET_RAC_TTL > 0) OR (@fleet_name = 'ADVANTAGE' AND FES.FLEET_ADV > 0) OR (@fleet_name = 'HERTZ ON DEMAND' AND FES.FLEET_HOD > 0) OR (@fleet_name = 'LICENSEE' AND FES.FLEET_LICENSEE > 0) OR (@fleet_name IS NULL AND (FES.FLEET_CARSALES > 0 OR FES.FLEET_RAC_TTL > 0)  AND NOT(FLEET_ADV = 1) AND NOT(FLEET_HOD = 1) AND NOT(FLEET_LICENSEE = 1)))--Fleet
  GROUP BY 
   FES.dim_Calendar_id_Start, l.LOCATION, FES.COUNTRY
 end
 else
 begin
  INSERT INTO #TMP_LOCATIONS(dim_calendar_id, COUNTRY, LOCATION, TOPIC_COUNTER, GROUP_COUNTER, NumDays, rowtype)
  SELECT 
   FES.dim_Calendar_id, FES.COUNTRY, l.LOCATION, SUM(FES.OPERATIONAL_FLEET), SUM(FES.TOTAL_FLEET), max(FES.NumDays), 'm' 
  FROM 
   inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY_Month_DOW AS FES
    join @periods m
     on m.dim_Calendar_id = FES.dim_Calendar_id
    join @Locations l
     on l.dim_Location_id = fes.dim_Location_id
    join @CarGroups cg
     on cg.CarGroup = FES.Car_Group
     or cg.CarGroup is null
  WHERE
   (FES.COUNTRY = @country OR @country IS NULL) -- Country
     AND ((@fleet_name = 'CARSALES' AND FES.FLEET_CARSALES > 0) OR (@fleet_name = 'RAC OPS' AND FES.FLEET_RAC_OPS > 0) OR (@fleet_name = 'RAC TTL' AND FES.FLEET_RAC_TTL > 0) OR (@fleet_name = 'ADVANTAGE' AND FES.FLEET_ADV > 0) OR (@fleet_name = 'HERTZ ON DEMAND' AND FES.FLEET_HOD > 0) OR (@fleet_name = 'LICENSEE' AND FES.FLEET_LICENSEE > 0) OR (@fleet_name IS NULL AND (FES.FLEET_CARSALES > 0 OR FES.FLEET_RAC_TTL > 0)  AND NOT(FLEET_ADV = 1) AND NOT(FLEET_HOD = 1) AND NOT(FLEET_LICENSEE = 1)))--Fleet
  AND FES.Rep_DayOfWeek = @day_of_week
  GROUP BY 
   FES.dim_Calendar_id, l.LOCATION, FES.COUNTRY
 end

 -- CALCULATE sums of cars per group
 IF (@grouping_criteria = 'COUNTRY')
 BEGIN
  INSERT INTO #TMP_GROUPS(dim_calendar_id, GROUP_ID, GROUP_NAME, TOPIC_COUNTER, GROUP_COUNTER, NumDays)
   SELECT dim_calendar_id, #TMP_LOCATIONS.COUNTRY, #TMP_LOCATIONS.COUNTRY AS location, SUM(TOPIC_COUNTER) AS TOPIC_COUNTER, SUM(GROUP_COUNTER) AS GROUP_COUNTER, max(NumDays)
   FROM #TMP_LOCATIONS       
   LEFT JOIN COUNTRIES ON COUNTRIES.country = #TMP_LOCATIONS.COUNTRY
   WHERE COUNTRIES.active = 1
   GROUP BY dim_calendar_id, #TMP_LOCATIONS.COUNTRY
 END
 ELSE 
 BEGIN
  IF (@grouping_criteria = 'CMS_POOL')
  BEGIN
   INSERT INTO #TMP_GROUPS(dim_calendar_id, GROUP_ID, GROUP_NAME, TOPIC_COUNTER, GROUP_COUNTER, NumDays)
    SELECT dim_calendar_id, CP.cms_pool_id, CP.cms_pool AS location, SUM(TOPIC_COUNTER) AS TOPIC_COUNTER, SUM(GROUP_COUNTER) AS GROUP_COUNTER, max(NumDays)
    FROM #TMP_LOCATIONS
    LEFT JOIN LOCATIONS AS L ON L.location = #TMP_LOCATIONS.location
    LEFT JOIN CMS_LOCATION_GROUPS AS CLG ON CLG.cms_location_group_id = L.cms_location_group_id  
    LEFT JOIN CMS_POOLS AS CP ON CP.cms_pool_id = CLG.cms_pool_id AND CP.country = #TMP_LOCATIONS.COUNTRY      
    GROUP BY dim_calendar_id, CP.cms_pool_id, CP.cms_pool
  END 
  ELSE 
  BEGIN    
   IF (@grouping_criteria = 'CMS_LOCATION_GROUP')
   BEGIN
    INSERT INTO #TMP_GROUPS(dim_calendar_id, GROUP_ID, GROUP_NAME, TOPIC_COUNTER, GROUP_COUNTER, NumDays)
     SELECT dim_calendar_id, CLG.cms_location_group_id, CLG.cms_location_group, SUM(TOPIC_COUNTER), SUM(GROUP_COUNTER), max(NumDays)
     FROM #TMP_LOCATIONS
     LEFT JOIN LOCATIONS AS L ON L.location = #TMP_LOCATIONS.location
     LEFT JOIN CMS_LOCATION_GROUPS AS CLG ON CLG.cms_location_group_id = L.cms_location_group_id      
     GROUP BY dim_calendar_id, CLG.cms_location_group_id, CLG.cms_location_group
   END 
   ELSE
   BEGIN    
    IF (@grouping_criteria = 'LOCATION')
    BEGIN
     INSERT INTO #TMP_GROUPS(dim_calendar_id, GROUP_ID, GROUP_NAME, TOPIC_COUNTER, GROUP_COUNTER, NumDays)
      SELECT dim_calendar_id, #TMP_LOCATIONS.LOCATION, #TMP_LOCATIONS.LOCATION, SUM(TOPIC_COUNTER), SUM(GROUP_COUNTER), max(NumDays) 
      FROM #TMP_LOCATIONS 
      GROUP BY dim_calendar_id, #TMP_LOCATIONS.LOCATION
    END 
    ELSE
    BEGIN
     IF (@grouping_criteria = 'OPS_REGION')
     BEGIN
      INSERT INTO #TMP_GROUPS(dim_calendar_id, GROUP_ID, GROUP_NAME, TOPIC_COUNTER, GROUP_COUNTER, NumDays)
       SELECT dim_calendar_id, ORE.ops_region_id, ORE.ops_region, SUM(TOPIC_COUNTER), SUM(GROUP_COUNTER), max(NumDays) 
       FROM #TMP_LOCATIONS
       LEFT JOIN LOCATIONS AS L ON L.location = #TMP_LOCATIONS.location
       LEFT JOIN OPS_AREAS AS OA ON OA.ops_area_id = L.ops_area_id
       LEFT JOIN OPS_Regions AS ORE ON ORE.ops_region_id = OA.ops_region_id AND ORE.country = #TMP_LOCATIONS.COUNTRY 
       GROUP BY dim_calendar_id, ORE.ops_region_id, ORE.ops_region
      
     END 
     ELSE
     BEGIN
      IF (@grouping_criteria = 'OPS_AREA')
      BEGIN
       INSERT INTO #TMP_GROUPS(dim_calendar_id, GROUP_ID, GROUP_NAME, TOPIC_COUNTER, GROUP_COUNTER, NumDays)
        SELECT dim_calendar_id, OA.ops_area_id, OA.ops_area, SUM(TOPIC_COUNTER), SUM(GROUP_COUNTER), max(NumDays) 
        FROM #TMP_LOCATIONS
        LEFT JOIN LOCATIONS AS L ON L.location = #TMP_LOCATIONS.location
        LEFT JOIN OPS_AREAS AS OA ON OA.ops_area_id = L.ops_area_id        
        GROUP BY dim_calendar_id, OA.ops_area_id, OA.ops_area
      END 
     END
    END
   END     
  END
 END

 -- DISPLAY NUMBERS OF CARS
 IF (@select_by = 'VALUE')
 BEGIN  
  SELECT GROUP_NAME AS [location], convert(numeric(18,2),1.0*sum(TOPIC_COUNTER)/sum(NumDays)) AS [car_count]
  FROM #TMP_GROUPS
  GROUP BY GROUP_ID, GROUP_NAME
  ORDER BY GROUP_NAME
  select @rowcount = @@rowcount
 END
 -- Display percentage of cars in the TOPIC
 ELSE IF (@select_by = 'PERCENTAGE')
 BEGIN 
  SELECT GROUP_NAME AS [location], case when convert(numeric(18,2),1.0*sum(TOPIC_COUNTER)/sum(NumDays)) = 0
             then convert(numeric(18,2),0)
             else convert(numeric(18,2),100.0*sum(TOPIC_COUNTER)/sum(GROUP_COUNTER))
           end as car_count
  FROM #TMP_GROUPS
  GROUP BY GROUP_ID, GROUP_NAME
  ORDER BY GROUP_NAME
  select @rowcount = @@rowcount
 END
 
 -- Drop the tempory table 
 DROP TABLE #TMP_GROUPS
 -- Drop the tempory table 
 DROP TABLE #TMP_LOCATIONS



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