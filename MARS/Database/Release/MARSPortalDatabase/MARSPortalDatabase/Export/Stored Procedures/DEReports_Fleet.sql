-- =============================================
-- Author:		Javier
-- Create date: July 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Export].[DEReports_Fleet]
	
AS
BEGIN
	
    SET NOCOUNT ON;
	
    DECLARE @theDay DATETIME

    SET @theDay =GETDATE();


    SELECT 
	   FES.COUNTRY AS 'COUNTRY',
	   CASE WHEN FES.FLEET_ADV = 1 THEN 'FF' ELSE
		  CASE WHEN FES.FLEET_HOD = 1 THEN 'HOD' ELSE 'RAC' END END AS 'BRAND',		
	   CONVERT(varchar(4), DATEPART(year, C.Rep_Date)) AS 'REP_YEAR',
	   CASE WHEN DATEPART(month, C.Rep_Date) < 10 THEN '0' + CONVERT(varchar(2), DATEPART(month, C.Rep_Date)) ELSE CONVERT(varchar(2), DATEPART(month, C.Rep_Date)) END AS 'REP_MONTH',
	   LEFT(LTRIM(RTRIM(CONVERT(char, C.Rep_Date, 120))), 10) AS 'REP_DATE',
	   L.location AS 'LOCATION',
	   CASE WHEN CS.car_segment = 'Van' OR CS.car_segment = 'Vans' THEN 'VAN' ELSE 'CAR' END AS 'CAR_SEGMENT',
	   SUM(FES.TOTAL_FLEET) AS 'TOTAL_FLEET',
	   SUM(FES.WS) as WHOLESALE,
	   SUM(FES.SV) as SALVAGE ,
	   SUM(FES.OPERATIONAL_FLEET) AS 'OPERATIONAL_FLEET',
	   SUM(FES.BD) as BODY_DAMAGE ,
	   SUM(FES.MM) as MAINTENANCE ,
	   SUM(FES.TB) as TURNBACK,
	   SUM(FES.AVAILABLE_FLEET) AS 'AVAILABLE_FLEET',
	   SUM(FES.OVERDUE) AS 'OVERDUE',
	   SUM(FES.ON_RENT) AS 'ON_RENT',
	   SUM(FES.ON_RENT_MAX) AS 'ON_RENT_PEAK',		
	   SUM(FES.AVAILABLE_FLEET_MAX) AS 'AVAILABLE_FLEET_PEAK'	
    FROM	  Inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY AS FES
    JOIN inp.dim_Calendar cal on fes.dim_Calendar_id = cal.dim_Calendar_id
    INNER JOIN Inp.dim_Calendar C ON FES.dim_Calendar_id = C.dim_Calendar_id
    INNER JOIN dbo.CAR_GROUPS AS CG ON FES.CAR_GROUP = CG.car_group
    INNER JOIN dbo.CAR_CLASSES AS CC ON CG.car_class_id = CC.car_class_id
    INNER JOIN dbo.CAR_SEGMENTS AS CS ON CC.car_segment_id = CS.car_segment_id			
    INNER JOIN dbo.LOCATIONS L ON FES.dim_Location_id = L.dim_Location_id
    INNER JOIN dbo.CMS_LOCATION_GROUPS CMG ON L.cms_location_group_id = CMG.cms_location_group_id
    INNER JOIN dbo.CMS_POOLS CMP ON CMG.cms_pool_id = CMP.cms_pool_id
    WHERE
	   ((FES.FLEET_RAC_TTL = 1) OR (FES.FLEET_CARSALES = 1)OR (FES.FLEET_ADV = 1)
		  OR(FES.FLEET_HOD = 1))
	   AND (FES.COUNTRY = CS.country)
	   AND (FES.COUNTRY = CMP.country)
	   AND (FES.COUNTRY IN ('BE', 'FR', 'GE', 'IT', 'LU', 'NE', 'SP', 'UK'))
	   AND (cal.[Rep_Year]	= YEAR(@theDay))
	   AND(cal.Rep_Month	= MONTH(@theDay))	
    GROUP BY
	   FES.COUNTRY
	   ,CASE WHEN FES.FLEET_ADV = 1 THEN 'FF' 
	   ELSE CASE WHEN FES.FLEET_HOD = 1 THEN 'HOD' 
	   ELSE 'RAC' END END,
	   C.Rep_Date,
	   L.location,
	   CASE 
		  WHEN CS.car_segment = 'Van' OR CS.car_segment = 'Vans' THEN 'VAN' 
	   ELSE 'CAR' 
	   END;
	

    
END