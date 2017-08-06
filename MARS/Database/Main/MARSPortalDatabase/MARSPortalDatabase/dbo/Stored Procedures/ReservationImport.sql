CREATE PROCEDURE [dbo].[ReservationImport]

AS
BEGIN
	SET NOCOUNT ON;

select distinct *  into #tmp from dbo.ReservationStaging

delete  from #tmp
where res_id_nbr in(
select res_id_nbr from #tmp  group by res_id_nbr having COUNT(res_id_nbr)>=2)

merge dbo.Reservations t
using #tmp s
on (t.RES_ID_NBR=s.RES_ID_NBR)
when not matched by TARGET
	then insert
	(RES_ID_NBR
	,COUNTRY
	,RENT_LOC
	,RTRN_LOC
	,[ONEWAY]
	,[RS_ARRIVAL_DATE]
	,[RS_ARRIVAL_TIME]
	,[RTRN_DATE]
	,[RTRN_TIME]
	,[RES_DAYS]
	,[GR_INCL_GOLDUPGR]
	,[RATE_QUOTED]
	,[SUBTOTAL_2]
	,[MOP]
	,[PREPAID]
	,[NEVERLOST]
	,[PREDELIVERY]
	,[CUST_NAME]
	,[PHONE]
	,[CDPID_NBR]
	,[CNTID_NBR]
	,[NO1_CLUB_GOLD]
	,[TACO]
	,[FLIGHT_NBR]
	,[GS]
	,[N1TYPE]
	,[DATE_SOLD]
	,[CO_HOURS]
	,[CO_DAYS]
	,[CI_HOURS]
	,[CI_HOURS_OFFSET]
	,[CI_DAYS])
	values
	(s.RES_ID_NBR
	,s.COUNTRY
	,(Select top 1 dim_Location_id from locations where location=s.RENT_LOC)
	,(Select top 1 dim_Location_id from locations where location=s.RTRN_LOC)
	,case when s.RENT_LOC<>s.RTRN_LOC then 'Y' else 'N' end
	,[RS_ARRIVAL_DATE]
	,[RS_ARRIVAL_TIME]
	,[RTRN_DATE]
	,[RTRN_TIME]
	,[RES_DAYS]
	,(select top 1 car_group_id from vw_Pooling_Vehicles where car_group=s.GR_INCL_GOLDUPGR and country=s.country)
	,[RATE_QUOTED]
	,[SUBTOTAL_2]
	,[MOP]
	,case when [PREPAID]='P' then 'Y' else 'N' end
	,case when [NEVERLOST]='NAVSYSREQ' then 'Y' else 'N' end
	,[PREDELIVERY]
	,[CUST_NAME]
	,[PHONE]
	,[CDPID_NBR]
	,[CNTID_NBR]
	,right([NO1_CLUB_GOLD],10)
	,[TACO]
	,[FLIGHT_NBR]
	,[GS]
	,[N1TYPE]
	,[DATE_SOLD_DT]+[DATE_SOLD_TM]
	,DateDiff(hour,GETDATE(),([RS_ARRIVAL_DATE]+Convert(time,[RS_ARRIVAL_TIME])))
	,DateDiff(day,GETDATE(),([RS_ARRIVAL_DATE]+Convert(time,[RS_ARRIVAL_TIME])))
	,DateDiff(hour,GETDATE(),([RTRN_DATE]+Convert(time,[RTRN_TIME])))
	,DateDiff(hour,GETDATE(),([RTRN_DATE]+Convert(time,[RTRN_TIME])))
	,DateDiff(day,GETDATE(),([RTRN_DATE]+Convert(time,[RTRN_TIME]))))
when matched
	then update set 
	t.COUNTRY=s.COUNTRY
	,t.RENT_LOC=(Select top 1 dim_Location_id from locations where location=s.RENT_LOC)
	,t.RTRN_LOC=(Select top 1 dim_Location_id from locations where location=s.RTRN_LOC)
	,t.[ONEWAY]=case when s.RENT_LOC<>s.RTRN_LOC then 'Y' else 'N' end
	,t.[RS_ARRIVAL_DATE]=s.[RS_ARRIVAL_DATE]
	,t.[RS_ARRIVAL_TIME]=s.[RS_ARRIVAL_TIME]
	,t.[RTRN_DATE]=s.[RTRN_DATE]
	,t.[RTRN_TIME]=s.[RTRN_TIME]
	,t.[RES_DAYS]=s.[RES_DAYS]
	,t.[GR_INCL_GOLDUPGR]=(select top 1 car_group_id from vw_Pooling_Vehicles where car_group=s.GR_INCL_GOLDUPGR and country=s.country)
	,t.[RATE_QUOTED]=s.[RATE_QUOTED]
	,t.[SUBTOTAL_2]=s.[SUBTOTAL_2]
	,t.[MOP]=s.[MOP]
	,t.[PREPAID]=case when s.[PREPAID]='P' then 'Y' else 'N' end
	,t.[NEVERLOST]=case when s.[NEVERLOST]='NAVSYSREQ' then 'Y' else 'N' end
	,t.[PREDELIVERY]=s.[PREDELIVERY]
	,t.[CUST_NAME]=s.[CUST_NAME]
	,t.[PHONE]=s.[PHONE]
	,t.[CDPID_NBR]=s.[CDPID_NBR]
	,t.[CNTID_NBR]=s.[CNTID_NBR]
	,t.[NO1_CLUB_GOLD]=right(s.[NO1_CLUB_GOLD],10)
	,t.[TACO]=s.[TACO]
	,t.[FLIGHT_NBR]=s.[FLIGHT_NBR]
	,t.[GS]=s.[GS]
	,t.[N1TYPE]=s.[N1TYPE]
	,t.[DATE_SOLD]=s.[DATE_SOLD_DT]+s.[DATE_SOLD_TM]
	,t.[CO_HOURS]=DateDiff(hour,GETDATE(),(s.[RS_ARRIVAL_DATE]+Convert(time,s.[RS_ARRIVAL_TIME])))
	,t.[CO_DAYS]=DateDiff(day,GETDATE(),(s.[RS_ARRIVAL_DATE]+Convert(time,s.[RS_ARRIVAL_TIME])))
	,t.[CI_HOURS]=DateDiff(hour,GETDATE(),(s.[RTRN_DATE]+Convert(time,s.[RTRN_TIME])))
	,t.[CI_HOURS_OFFSET]=DateDiff(hour,GETDATE(),(s.[RTRN_DATE]+Convert(time,s.[RTRN_TIME])))
	,t.[CI_DAYS]=DateDiff(day,GETDATE(),(s.[RTRN_DATE]+Convert(time,s.[RTRN_TIME])))
when not matched by SOURCE and getDate()> t.[RTRN_DATE]
	then delete;
drop table #tmp

END