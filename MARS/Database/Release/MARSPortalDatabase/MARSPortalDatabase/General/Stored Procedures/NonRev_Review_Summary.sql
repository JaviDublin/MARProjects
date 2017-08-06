-- =============================================
-- Author:		Javier
-- Create date: January 2013
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[NonRev_Review_Summary] 

AS
BEGIN
	
	SET NOCOUNT ON;
	
	TRUNCATE TABLE FLEET_EUROPE_ACTUAL_STAGING
	
	INSERT INTO FLEET_EUROPE_ACTUAL_STAGING
	(
		IMPORTTIME,BASEOP,BDDAYS,CAPCOST,CAPDATE,CARHOLD1,COLOR,DAYSAREA,DAYSCTRY,DAYSMOVE,
		DAYSREV,DEPAMT,DEPSTAT,DRVNAME,DUEAREA,DUEDATE,DUETIME,DUEWWD,IDATE,LICENSE,LSTAREA,LSTDATE,LSTMLG,
		LSTNO,LSTOORC,LSTTIME,LSTTYPE,LSTWWD,MMDAYS,MODDESC,MODEL,MODGROUP,MOVETYPE,MSODATE,OPERDAYS,
		OPERSTAT,OWNAREA,OWNWWD,PONO,PREVWWD,RADATE,RALOC,RC,SDATE,SERIAL,SPROCDAT,TERMDAYS,UNIT,VC,VEHCLAS,
		VEHTYPE,VENDNBR,VISMODEL,COUNTRY,[POOL],LOC_GROUP,CARVAN,CAR_CLASS,FLEET_RAC_TTL,FLEET_RAC_OPS,
		FLEET_CARSALES,FLEET_LICENSEE,TOTAL_FLEET,CARSALES,CARHOLD_H,CARHOLD_L,CU,HA,HL,LL,NC,PL,TC,SV,WS,
		WS_NONRAC,OPERATIONAL_FLEET,BD,MM,TW,TB,WS_RAC,AVAILABLE_TB,FS,RL,RP,TN,AVAILABLE_FLEET,RT,
		SU,GOLD,PREDELIVERY,OVERDUE,ON_RENT,CI_HOURS,CI_HOURS_OFFSET,CI_DAYS,FLEET_ADV,FLEET_HOD
	)
	
	SELECT 
		IMPORTTIME,BASEOP,BDDAYS,CAPCOST,CAPDATE,CARHOLD1,COLOR,DAYSAREA,DAYSCTRY,DAYSMOVE,
		DAYSREV,DEPAMT,DEPSTAT,DRVNAME,DUEAREA,DUEDATE,DUETIME,DUEWWD,IDATE,LICENSE,LSTAREA,LSTDATE,LSTMLG,
		LSTNO,LSTOORC,LSTTIME,LSTTYPE,LSTWWD,MMDAYS,MODDESC,MODEL,MODGROUP,MOVETYPE,MSODATE,OPERDAYS,
		OPERSTAT,OWNAREA,OWNWWD,PONO,PREVWWD,RADATE,RALOC,RC,SDATE,SERIAL,SPROCDAT,TERMDAYS,UNIT,VC,VEHCLAS,
		VEHTYPE,VENDNBR,VISMODEL,COUNTRY,[POOL],LOC_GROUP,CARVAN,CAR_CLASS,FLEET_RAC_TTL,FLEET_RAC_OPS,
		FLEET_CARSALES,FLEET_LICENSEE,TOTAL_FLEET,CARSALES,CARHOLD_H,CARHOLD_L,CU,HA,HL,LL,NC,PL,TC,SV,WS,
		WS_NONRAC,OPERATIONAL_FLEET,BD,MM,TW,TB,WS_RAC,AVAILABLE_TB,FS,RL,RP,TN,AVAILABLE_FLEET,RT,
		SU,GOLD,PREDELIVERY,OVERDUE,ON_RENT,CI_HOURS,CI_HOURS_OFFSET,CI_DAYS,FLEET_ADV,FLEET_HOD
	FROM 
		FLEET_EUROPE_ACTUAL
				
	UPDATE FLEET_EUROPE_ACTUAL_STAGING SET CountryCar = COUNTRY
	
	UPDATE FLEET_EUROPE_ACTUAL_STAGING SET CountryLoc = SUBSTRING(LSTWWD,1,2)
	
	UPDATE FLEET_EUROPE_ACTUAL_STAGING SET IsNonRev = 0 WHERE OPERSTAT = 'RT' and MOVETYPE = 'R-O' 
	
	UPDATE FLEET_EUROPE_ACTUAL_STAGING SET IsNonRev = 1 WHERE IsNonRev is null

	UPDATE FLEET_EUROPE_ACTUAL_STAGING SET
		location_pool		= cms.cms_pool,
		location_pool_id	= cms.cms_pool_id,
		location_group		= cms.cms_location_group,
		location_group_id	= cms.cms_location_group_id
	FROM FLEET_EUROPE_ACTUAL_STAGING feas
	INNER JOIN Settings.vw_Locations_CMS cms ON feas.LSTWWD = cms.location 

	UPDATE FLEET_EUROPE_ACTUAL_STAGING SET
		location_ops_area		= ops.ops_area,
		location_ops_area_id	= ops.ops_area_id,
		location_ops_region		= ops.ops_region,
		location_ops_region_id	= ops.ops_region_id
	FROM FLEET_EUROPE_ACTUAL_STAGING feas
	INNER JOIN Settings.vw_Locations_OPS ops ON feas.LSTWWD = ops.location 

	UPDATE FLEET_EUROPE_ACTUAL_STAGING SET
		OperationalStatusCode	= os.OperationalStatusCode,
		KCICode					=  os.KCICode
	FROM FLEET_EUROPE_ACTUAL_STAGING feas
	inner join Settings.Operational_Status os ON feas.OPERSTAT = os.OperationalStatusCode

	UPDATE FLEET_EUROPE_ACTUAL_STAGING SET 
		DayGroupCode = dg.DayGroupCode,
		DayGroupName = dg.DayGroupName
	FROM FLEET_EUROPE_ACTUAL_STAGING feas
	INNER JOIN Settings.NonRev_Day_Groups dg ON 
		feas.DAYSREV >= dg.DayMin and feas.DAYSREV <= dg.DayMax	
	WHERE feas.IsNonRev = 1

	UPDATE FLEET_EUROPE_ACTUAL_STAGING SET
		car_group		= cg.car_group ,
		car_group_id	= cg.car_group_id,
		car_class_name	= cg.car_class,
		car_class_id	= cg.car_class_id,
		car_segment		= cg.car_segment,
		car_segment_id	= cg.car_segment_id
	FROM FLEET_EUROPE_ACTUAL_STAGING feas
	INNER JOIN Settings.vw_CarGroups cg ON feas.VC = cg.car_group and feas.CountryCar = cg.country

	DECLARE @calendar_id INT
	
	SET @calendar_id = (SELECT TOP 1 CONVERT(INT,CONVERT(VARCHAR(8),IMPORTTIME,112)) FROM FLEET_EUROPE_ACTUAL_STAGING)

	IF NOT EXISTS 
		(	SELECT	TOP 1 dim_calendar_id 
			FROM	[General].[Fact_NonRevLog_Summary]
			WHERE	dim_calendar_id = @calendar_id )
	BEGIN
		
		INSERT INTO [General].[Fact_NonRevLog_Summary]
		(
			dim_calendar_id,country_car,country_loc,
			location_group,location_group_id,location_pool,location_pool_id,location_ops_area,location_ops_area_id,
			location_ops_region,location_ops_region_id,car_group,car_group_id,car_class,car_class_id,
			car_segment,car_segment_id,location ,
			total_records,total_fleet,operational_fleet,available_fleet,rev_fleet, non_rev_fleet,
			daygroup1,daygroup2,daygroup3,daygroup4,daygroup5,daygroup6,daygroup7,daygroup8,daygroup9,
			total_fleet_cs,operational_fleet_cs,available_fleet_cs,rev_fleet_cs,non_rev_fleet_cs,daygroup1_cs,
			daygroup2_cs,daygroup3_cs,daygroup4_cs,daygroup5_cs,daygroup6_cs,daygroup7_cs,daygroup8_cs,
			daygroup9_cs,total_fleet_lc,operational_fleet_lc,available_fleet_lc,rev_fleet_lc,non_rev_fleet_lc,daygroup1_lc,
			daygroup2_lc,daygroup3_lc,daygroup4_lc,daygroup5_lc,daygroup6_lc,daygroup7_lc,daygroup8_lc,
			daygroup9_lc,total_fleet_ad,operational_fleet_ad,available_fleet_ad,rev_fleet_ad,non_rev_fleet_ad,
			daygroup1_ad,daygroup2_ad,daygroup3_ad,daygroup4_ad,daygroup5_ad,daygroup6_ad,daygroup7_ad,daygroup8_ad,
			daygroup9_ad,total_fleet_hd,operational_fleet_hd,available_fleet_hd,rev_fleet_hd,non_rev_fleet_hd,
			daygroup1_hd,daygroup2_hd,daygroup3_hd,daygroup4_hd,daygroup5_hd,daygroup6_hd,daygroup7_hd,daygroup8_hd,
			daygroup9_hd,total_fleet_op,operational_fleet_op,available_fleet_op,rev_fleet_op,non_rev_fleet_op,daygroup1_op,
			daygroup2_op,daygroup3_op,daygroup4_op,daygroup5_op,daygroup6_op,daygroup7_op,daygroup8_op,daygroup9_op,
			total_fleet_tt,operational_fleet_tt,available_fleet_tt,rev_fleet_tt,non_rev_fleet_tt,
			daygroup1_tt,daygroup2_tt,daygroup3_tt,daygroup4_tt,daygroup5_tt,daygroup6_tt,daygroup7_tt,daygroup8_tt,
			daygroup9_tt , OperationalStatusCode
		)
		
	
		SELECT 
			CONVERT(INT,CONVERT(VARCHAR(8),IMPORTTIME,112)) AS [dim_calendar_id],
			CountryCar ,  CountryLoc , location_group , location_group_id , location_pool ,  location_pool_id ,
			location_ops_area ,  location_ops_area_id ,  location_ops_region ,  location_ops_region_id , 
			car_group ,  car_group_id ,  car_class_name ,  car_class_id ,  car_segment ,  car_segment_id ,
			LSTWWD , COUNT(SERIAL) as [Total Records],
COUNT(case when TOTAL_FLEET = 1 then SERIAL else null end) as [Total Fleet],
COUNT(case when TOTAL_FLEET = 1 AND OPERATIONAL_FLEET = 1 then SERIAL else null end) as [Operational Fleet],
COUNT(case when TOTAL_FLEET = 1 AND AVAILABLE_FLEET = 1 then SERIAL else null end) as [Available Fleet],
COUNT(case when IsNonRev = 0 and TOTAL_FLEET = 1 then SERIAL else null end) as [Rev],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 then SERIAL else null end) as [Non Rev],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup1' then SERIAL else null end) as [DayGroup1],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup2' then SERIAL else null end) as [DayGroup2],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup3' then SERIAL else null end) as [DayGroup3],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup4' then SERIAL else null end) as [DayGroup4],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup5' then SERIAL else null end) as [DayGroup5],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup6' then SERIAL else null end) as [DayGroup6],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup7' then SERIAL else null end) as [DayGroup7],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup8' then SERIAL else null end) as [DayGroup8],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup9' then SERIAL else null end) as [DayGroup9],
COUNT(case when TOTAL_FLEET = 1 AND FLEET_CARSALES = 1 then SERIAL else null end) as [Total Fleet CS],
COUNT(case when TOTAL_FLEET = 1 AND OPERATIONAL_FLEET = 1 AND FLEET_CARSALES = 1 then SERIAL else null end) as [Operational Fleet CS],
COUNT(case when TOTAL_FLEET = 1 AND AVAILABLE_FLEET = 1 AND FLEET_CARSALES = 1 then SERIAL else null end) as [Available Fleet CS],
COUNT(case when IsNonRev = 0 and TOTAL_FLEET = 1 AND FLEET_CARSALES = 1 then SERIAL else null end) as [Rev CS],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 AND FLEET_CARSALES = 1 then SERIAL else null end) as [Non Rev CS],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup1' AND FLEET_CARSALES = 1 then SERIAL else null end) as [DayGroup1 CS],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup2' AND FLEET_CARSALES = 1 then SERIAL else null end) as [DayGroup2 CS],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup3' AND FLEET_CARSALES = 1 then SERIAL else null end) as [DayGroup3 CS],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup4' AND FLEET_CARSALES = 1 then SERIAL else null end) as [DayGroup4 CS],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup5' AND FLEET_CARSALES = 1 then SERIAL else null end) as [DayGroup5 CS],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup6' AND FLEET_CARSALES = 1 then SERIAL else null end) as [DayGroup6 CS],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup7' AND FLEET_CARSALES = 1 then SERIAL else null end) as [DayGroup7 CS],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup8' AND FLEET_CARSALES = 1 then SERIAL else null end) as [DayGroup8 CS],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup9' AND FLEET_CARSALES = 1 then SERIAL else null end) as [DayGroup9 CS],
COUNT(case when TOTAL_FLEET = 1 AND FLEET_LICENSEE = 1 then SERIAL else null end) as [Total Fleet LC],
COUNT(case when TOTAL_FLEET = 1 AND OPERATIONAL_FLEET = 1 AND FLEET_LICENSEE = 1 then SERIAL else null end) as [Operational Fleet LC],
COUNT(case when TOTAL_FLEET = 1 AND AVAILABLE_FLEET = 1 AND FLEET_LICENSEE = 1 then SERIAL else null end) as [Available Fleet LC],
COUNT(case when IsNonRev = 0 and TOTAL_FLEET = 1 AND FLEET_LICENSEE = 1 then SERIAL else null end) as [Rev LC],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 AND FLEET_LICENSEE = 1 then SERIAL else null end) as [Non Rev LC],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup1' AND FLEET_LICENSEE = 1 then SERIAL else null end) as [DayGroup1 LC],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup2' AND FLEET_LICENSEE = 1 then SERIAL else null end) as [DayGroup2 LC],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup3' AND FLEET_LICENSEE = 1 then SERIAL else null end) as [DayGroup3 LC],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup4' AND FLEET_LICENSEE = 1 then SERIAL else null end) as [DayGroup4 LC],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup5' AND FLEET_LICENSEE = 1 then SERIAL else null end) as [DayGroup5 LC],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup6' AND FLEET_LICENSEE = 1 then SERIAL else null end) as [DayGroup6 LC],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup7' AND FLEET_LICENSEE = 1 then SERIAL else null end) as [DayGroup7 LC],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup8' AND FLEET_LICENSEE = 1 then SERIAL else null end) as [DayGroup8 LC],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup9' AND FLEET_LICENSEE = 1 then SERIAL else null end) as [DayGroup9 LC],
COUNT(case when TOTAL_FLEET = 1 AND FLEET_ADV = 1 then SERIAL else null end) as [Total Fleet AD],
COUNT(case when TOTAL_FLEET = 1 AND OPERATIONAL_FLEET = 1 AND FLEET_ADV = 1 then SERIAL else null end) as [Operational Fleet AD],
COUNT(case when TOTAL_FLEET = 1 AND AVAILABLE_FLEET = 1 AND FLEET_ADV = 1 then SERIAL else null end) as [Available Fleet AD],
COUNT(case when IsNonRev = 0 and TOTAL_FLEET = 1 AND FLEET_ADV = 1 then SERIAL else null end) as [Rev AD],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 AND FLEET_ADV = 1 then SERIAL else null end) as [Non Rev AD],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup1' AND FLEET_ADV = 1 then SERIAL else null end) as [DayGroup1 AD],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup2' AND FLEET_ADV = 1 then SERIAL else null end) as [DayGroup2 AD],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup3' AND FLEET_ADV = 1 then SERIAL else null end) as [DayGroup3 AD],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup4' AND FLEET_ADV = 1 then SERIAL else null end) as [DayGroup4 AD],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup5' AND FLEET_ADV = 1 then SERIAL else null end) as [DayGroup5 AD],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup6' AND FLEET_ADV = 1 then SERIAL else null end) as [DayGroup6 AD],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup7' AND FLEET_ADV = 1 then SERIAL else null end) as [DayGroup7 AD],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup8' AND FLEET_ADV = 1 then SERIAL else null end) as [DayGroup8 AD],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup9' AND FLEET_ADV = 1 then SERIAL else null end) as [DayGroup9 AD],
COUNT(case when TOTAL_FLEET = 1 AND FLEET_HOD = 1 then SERIAL else null end) as [Total Fleet HD],
COUNT(case when TOTAL_FLEET = 1 AND OPERATIONAL_FLEET = 1 AND FLEET_HOD = 1 then SERIAL else null end) as [Operational Fleet HD],
COUNT(case when TOTAL_FLEET = 1 AND AVAILABLE_FLEET = 1 AND FLEET_HOD = 1 then SERIAL else null end) as [Available Fleet HD],
COUNT(case when IsNonRev = 0 and TOTAL_FLEET = 1 AND FLEET_HOD = 1 then SERIAL else null end) as [Rev HD],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 AND FLEET_HOD = 1 then SERIAL else null end) as [Non Rev HD],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup1' AND FLEET_HOD = 1 then SERIAL else null end) as [DayGroup1 HD],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup2' AND FLEET_HOD = 1 then SERIAL else null end) as [DayGroup2 HD],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup3' AND FLEET_HOD = 1 then SERIAL else null end) as [DayGroup3 HD],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup4' AND FLEET_HOD = 1 then SERIAL else null end) as [DayGroup4 HD],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup5' AND FLEET_HOD = 1 then SERIAL else null end) as [DayGroup5 HD],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup6' AND FLEET_HOD = 1 then SERIAL else null end) as [DayGroup6 HD],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup7' AND FLEET_HOD = 1 then SERIAL else null end) as [DayGroup7 HD],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup8' AND FLEET_HOD = 1 then SERIAL else null end) as [DayGroup8 HD],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup9' AND FLEET_HOD = 1 then SERIAL else null end) as [DayGroup9 HD],
COUNT(case when TOTAL_FLEET = 1 AND FLEET_RAC_OPS = 1 then SERIAL else null end) as [Total Fleet OP],
COUNT(case when TOTAL_FLEET = 1 AND OPERATIONAL_FLEET = 1 AND FLEET_RAC_OPS = 1 then SERIAL else null end) as [Operational Fleet OP],
COUNT(case when TOTAL_FLEET = 1 AND AVAILABLE_FLEET = 1 AND FLEET_RAC_OPS = 1 then SERIAL else null end) as [Available Fleet OP],
COUNT(case when IsNonRev = 0 and TOTAL_FLEET = 1 AND FLEET_RAC_OPS = 1 then SERIAL else null end) as [Rev OP],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 AND FLEET_RAC_OPS = 1 then SERIAL else null end) as [Non Rev OP],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup1' AND FLEET_RAC_OPS = 1 then SERIAL else null end) as [DayGroup1 OP],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup2' AND FLEET_RAC_OPS = 1 then SERIAL else null end) as [DayGroup2 OP],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup3' AND FLEET_RAC_OPS = 1 then SERIAL else null end) as [DayGroup3 OP],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup4' AND FLEET_RAC_OPS = 1 then SERIAL else null end) as [DayGroup4 OP],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup5' AND FLEET_RAC_OPS = 1 then SERIAL else null end) as [DayGroup5 OP],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup6' AND FLEET_RAC_OPS = 1 then SERIAL else null end) as [DayGroup6 OP],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup7' AND FLEET_RAC_OPS = 1 then SERIAL else null end) as [DayGroup7 OP],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup8' AND FLEET_RAC_OPS = 1 then SERIAL else null end) as [DayGroup8 OP],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup9' AND FLEET_RAC_OPS = 1 then SERIAL else null end) as [DayGroup9 OP],
COUNT(case when TOTAL_FLEET = 1 AND FLEET_RAC_TTL = 1 then SERIAL else null end) as [Total Fleet TT],
COUNT(case when TOTAL_FLEET = 1 AND OPERATIONAL_FLEET = 1 AND FLEET_RAC_TTL = 1 then SERIAL else null end) as [Operational Fleet TT],
COUNT(case when TOTAL_FLEET = 1 AND AVAILABLE_FLEET = 1 AND FLEET_RAC_TTL = 1 then SERIAL else null end) as [Available Fleet TT],
COUNT(case when IsNonRev = 0 and TOTAL_FLEET = 1 AND FLEET_RAC_TTL = 1 then SERIAL else null end) as [Rev TT],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 AND FLEET_RAC_TTL = 1 then SERIAL else null end) as [Non Rev TT],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup1' AND FLEET_RAC_TTL = 1 then SERIAL else null end) as [DayGroup1 TT],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup2' AND FLEET_RAC_TTL = 1 then SERIAL else null end) as [DayGroup2 TT],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup3' AND FLEET_RAC_TTL = 1 then SERIAL else null end) as [DayGroup3 TT],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup4' AND FLEET_RAC_TTL = 1 then SERIAL else null end) as [DayGroup4 TT],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup5' AND FLEET_RAC_TTL = 1 then SERIAL else null end) as [DayGroup5 TT],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup6' AND FLEET_RAC_TTL = 1 then SERIAL else null end) as [DayGroup6 TT],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup7' AND FLEET_RAC_TTL = 1 then SERIAL else null end) as [DayGroup7 TT],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup8' AND FLEET_RAC_TTL = 1 then SERIAL else null end) as [DayGroup8 TT],
COUNT(case when IsNonRev = 1 and TOTAL_FLEET = 1 and DayGroupName = 'DayGroup9' AND FLEET_RAC_TTL = 1 then SERIAL else null end) as [DayGroup9 TT]
		, OperationalStatusCode
		FROM 
			FLEET_EUROPE_ACTUAL_STAGING
		GROUP BY 
			IMPORTTIME , CountryCar ,  CountryLoc , location_group , location_group_id , location_pool ,  location_pool_id ,
			location_ops_area ,  location_ops_area_id ,  location_ops_region ,  location_ops_region_id , 
			car_group ,  car_group_id ,  car_class_name ,  car_class_id ,  car_segment ,  car_segment_id ,
			LSTWWD , OperationalStatusCode
		ORDER BY  
			CountryCar
			
			
			
		UPDATE [General].[Fact_NonRevLog_Summary] SET
			Rep_DayOfWeek = dc.Rep_DayOfWeek
		FROM 
			[General].[Fact_NonRevLog_Summary] fnrls
		INNER JOIN [Inp].dim_Calendar dc ON fnrls.dim_calendar_id = dc.dim_Calendar_id
		WHERE fnrls.Rep_DayOfWeek IS NULL
	
	END
	
	
  
END