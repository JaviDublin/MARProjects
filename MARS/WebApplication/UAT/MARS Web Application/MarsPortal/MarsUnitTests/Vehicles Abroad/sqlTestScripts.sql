/*
This test proves that reservations which are present in dbo.Reservations table are not present in dbo.[RESERVATIONS_EUROPE_ACTUAL]
*/

DECLARE @p0 INT=0
DECLARE @p1 INT =2
DECLARE @p2 DATETIME='2014-07-01 00:00:00'
DECLARE @p3 DATETIME='2014-07-03 00:00:00'

DECLARE @p4 DATETIME='2014-07-01 00:00:00'
DECLARE @p5 DATETIME='2014-07-03 00:00:00'



	   -- 1. Find missing reservations
SELECT DISTINCT [t0].[RES_ID_NBR]                     
        FROM   [dbo].[Reservations] AS [t0]
               INNER JOIN [dbo].[LOCATIONS] AS [t1]
                       ON [t0].[RENT_LOC] = ( [t1].[dim_Location_id] )
               INNER JOIN [dbo].[CMS_LOCATION_GROUPS] AS [t2]
                       ON [t1].[cms_location_group_id] = ( [t2].[cms_location_group_id] )
               INNER JOIN [dbo].[CMS_POOLS] AS [t3]
                       ON [t2].[cms_pool_id] = [t3].[cms_pool_id]
               INNER JOIN [dbo].[COUNTRIES] AS [t4]
                       ON [t3].[country] = [t4].[country]
               INNER JOIN [dbo].[LOCATIONS] AS [t5]
                       ON [t0].[RTRN_LOC] = ( [t5].[dim_Location_id] )
               INNER JOIN [dbo].[CMS_LOCATION_GROUPS] AS [t6]
                       ON [t5].[cms_location_group_id] = ( [t6].[cms_location_group_id] )
               INNER JOIN [dbo].[CMS_POOLS] AS [t7]
                       ON [t6].[cms_pool_id] = [t7].[cms_pool_id]
               INNER JOIN [dbo].[COUNTRIES] AS [t8]
                       ON [t7].[country] = [t8].[country]
               INNER JOIN [dbo].[CAR_GROUPS] AS [t9]
                       ON [t0].[GR_INCL_GOLDUPGR] = ( [t9].[car_group_id] )
               INNER JOIN [dbo].[CAR_CLASSES] AS [t10]
                       ON [t9].[car_class_id] = [t10].[car_class_id]
               INNER JOIN [dbo].[CAR_SEGMENTS] AS [t11]
                       ON [t10].[car_segment_id] = [t11].[car_segment_id]
        WHERE  ( [t0].[RS_ARRIVAL_DATE] >= @p4 )
               AND ( [t0].[RS_ARRIVAL_DATE] <= @p5 )
               AND ( [t0].[COUNTRY] <> [t7].[country] )
			   and t0.COUNTRY = 'SP' and   [t8].[country_dw]     = 'FR'

except
SELECT DISTINCT [t0].[RES_ID_NBR]            
FROM   [dbo].[RESERVATIONS_EUROPE_ACTUAL] AS [t0]
       INNER JOIN [dbo].[CMS_POOLS] AS [t1]
               ON [t0].[CMS_POOL] = ( CONVERT(NVARCHAR, [t1].[cms_pool_id]) )
       INNER JOIN [dbo].[CMS_LOCATION_GROUPS] AS [t2]
               ON [t0].[CMS_LOC_GRP] = ( CONVERT(NVARCHAR, [t2].[cms_location_group_id]) )
       INNER JOIN [dbo].[COUNTRIES] AS [t3]
               ON [t0].[COUNTRY] = [t3].[country]
       INNER JOIN [dbo].[COUNTRIES] AS [t4]
               ON Substring([t0].[RTRN_LOC], @p0 + 1, @p1) = [t4].[country]
       INNER JOIN [dbo].[LOCATIONS] AS [t5]
               ON [t0].[RTRN_LOC] = [t5].[location]
       INNER JOIN [dbo].[CMS_LOCATION_GROUPS] AS [t6]
               ON [t5].[cms_location_group_id] = ( [t6].[cms_location_group_id] )
       INNER JOIN [dbo].[CMS_POOLS] AS [t7]
               ON [t6].[cms_pool_id] = [t7].[cms_pool_id]
       INNER JOIN [dbo].[CAR_SEGMENTS] AS [t8]
               ON [t0].[CAR_SEGMENT] = ( CONVERT(NVARCHAR, [t8].[car_segment_id]) )
       INNER JOIN [dbo].[CAR_CLASSES] AS [t9]
               ON [t0].[CAR_CLASS] = ( CONVERT(NVARCHAR, [t9].[car_class_id]) )
WHERE  ( [t0].[RS_ARRIVAL_DATE] >= @p2 )
       AND ( [t0].[RS_ARRIVAL_DATE] <= @p3 )
       AND ( [t0].[COUNTRY] <> [t7].[country] )
	   and t3.country = 'SP' and   [t4].[country_dw] = 'FR'


	   --2.  take a look at missing res ids
	   SELECT *
	   FROM RESERVATIONS_EUROPE_ACTUAL        
	   WHERE RES_ID_NBR in ('G1260925439',
'G12613777B4',
'G1340196394',
'G1340201892',
'G13433518F1')


