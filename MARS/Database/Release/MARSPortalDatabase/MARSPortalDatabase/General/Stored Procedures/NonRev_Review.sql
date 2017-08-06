-- =============================================
-- Author:		Javier

-- Create date: September 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[NonRev_Review]
	
AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @today DATETIME
	
	SET @today = GETDATE()
	
	
	--if @Type = 'Daily'
	--begin
	
	
	--declare @mydate datetime = DATEADD(D,0,DATEDIFF(D,0,GETDATE()))
	
	--if exists (
	--select * from bcs.BatchControl where Data3 = 'Daily'  and FileProcess_id in (
	--	select FileProcess_id from bcs.FileProcess where 
	--	DATEADD(D,0,DATEDIFF(D,0,z_inserted)) = @today and Entity = 'Mars_Daily') and status = 'Complete')
	--begin
	

-- 1 - Fleet Maintenance
--==========================================================================================

	DELETE FROM FLEET_EUROPE_ACTUAL WHERE SERIAL IS NULL or SERIAL = ''
	
	UPDATE [General].[Dim_Fleet] SET IsActive = NULL
	
	MERGE [General].[Dim_Fleet] AS TARGET
	USING
	(
		SELECT 
			LTRIM(RTRIM(SERIAL))					AS [SERIAL], 
			LTRIM(RTRIM(LICENSE))					AS [LICENSE], 
			LTRIM(RTRIM(UNIT))						AS [UNIT],  
			MSODATE									AS [MSODATE], 
			LTRIM(RTRIM(COUNTRY))					AS [COUNTRY], 
			LTRIM(RTRIM(MODEL))						AS [MODEL], 
			LTRIM(RTRIM(MODDESC))					AS [MODDESC], 
			LTRIM(RTRIM(COLOR))						AS [COLOR], 
			LTRIM(RTRIM(VC))						AS [VC],
			LTRIM(RTRIM(OWNAREA))					AS [OWNAREA],
			DATEADD(D,0,DATEDIFF(d,0,IMPORTTIME))	AS [IMPORTTIME],
			TOTAL_FLEET , OPERATIONAL_FLEET , AVAILABLE_FLEET ,
			FLEET_RAC_OPS , FLEET_RAC_TTL , FLEET_LICENSEE , 
			FLEET_CARSALES , FLEET_ADV , FLEET_HOD
		FROM FLEET_EUROPE_ACTUAL
		WHERE SERIAL IS NOT NULL AND SERIAL NOT IN ('')
	) AS SOURCE
	ON
	(
		TARGET.Serial	= SOURCE.SERIAL		AND
		TARGET.Country	= SOURCE.COUNTRY	
		
	)
	WHEN MATCHED THEN
	UPDATE SET 
		TARGET.LastDailyFileDate	= SOURCE.IMPORTTIME ,
		TARGET.IsActive				= 1,
		TARGET.Total_Fleet			= SOURCE.TOTAL_FLEET		,
		TARGET.Operational_Fleet	= SOURCE.OPERATIONAL_FLEET	,
		TARGET.Available_Fleet		= SOURCE.AVAILABLE_FLEET	,
		TARGET.Fleet_rac_ops		= SOURCE.FLEET_RAC_OPS		,
		TARGET.Fleet_rac_ttl		= SOURCE.FLEET_RAC_TTL		,
		TARGET.Fleet_licensee		= SOURCE.FLEET_LICENSEE		,
		TARGET.Fleet_carsales		= SOURCE.FLEET_CARSALES		,
		TARGET.Fleet_adv			= SOURCE.FLEET_ADV			,
		TARGET.Fleet_hod			= SOURCE.FLEET_HOD,
		Target.SERIAL = Source.Serial,
		Target.Plate = Source.License,
		Target.Unit = Source.Unit,
		Target.MsoDate = Source.MsoDate,
		Target.Country = Source.Country,
		Target.Model = Source.Model,
		Target.ModelDesc = Source.ModDesc,
		Target.Color = Source.Color,
		Target.CarGroup = Source.Vc,
		Target.OwnArea = Source.OwnArea
		
	WHEN NOT MATCHED THEN
	INSERT
	(
		Serial , Plate , Unit , MSODate , Country , Model , ModelDesc , Color , OwnArea , IsActive ,
		LastDailyFileDate , FirstDailyFileDate ,  CarGroup , Total_Fleet , Operational_Fleet , 	
		Available_Fleet , Fleet_rac_ops , Fleet_rac_ttl , Fleet_licensee , Fleet_carsales, Fleet_adv , 
		Fleet_hod
	)
	VALUES
	(
		SOURCE.SERIAL , SOURCE.LICENSE , SOURCE.UNIT , SOURCE.MSODATE,  SOURCE.COUNTRY , SOURCE.MODEL , 
		SOURCE.MODDESC ,  SOURCE.COLOR , SOURCE.OWNAREA , 1 ,SOURCE.IMPORTTIME ,  SOURCE.IMPORTTIME ,
		SOURCE.VC , SOURCE.TOTAL_FLEET , SOURCE.OPERATIONAL_FLEET , SOURCE.AVAILABLE_FLEET ,
		SOURCE.FLEET_RAC_OPS , SOURCE.FLEET_RAC_TTL , SOURCE.FLEET_LICENSEE , 
		SOURCE.FLEET_CARSALES , SOURCE.FLEET_ADV , SOURCE.FLEET_HOD
	);
	
	UPDATE [General].[Dim_Fleet] SET IsActive = 0 WHERE IsActive IS NULL
	
	
-- 2 - Non Revenue
--==========================================================================================
		
	TRUNCATE TABLE FLEET_EUROPE_ACTUAL_NON_REV
	
	INSERT INTO FLEET_EUROPE_ACTUAL_NON_REV
	SELECT 
		IMPORTTIME, BASEOP, BDDAYS, CAPCOST, CAPDATE, CARHOLD1, COLOR, DAYSAREA, DAYSCTRY, DAYSMOVE, DAYSREV, DEPAMT, DEPSTAT, DRVNAME, DUEAREA, DUEDATE, DUETIME, DUEWWD, IDATE, LICENSE, LSTAREA, LSTDATE, LSTMLG, LSTNO, LSTOORC, LSTTIME, LSTTYPE, LSTWWD, MMDAYS, MODDESC, MODEL, MODGROUP, MOVETYPE, MSODATE, OPERDAYS, OPERSTAT, OWNAREA, OWNWWD, PONO, PREVWWD, RADATE, RALOC, RC, SDATE, SERIAL, SPROCDAT, TERMDAYS, UNIT, VC, VEHCLAS, VEHTYPE, VENDNBR, VISMODEL, FEA.COUNTRY, POOL, LOC_GROUP, CARVAN, CAR_CLASS, FLEET_RAC_TTL, FLEET_RAC_OPS, FLEET_CARSALES, FLEET_LICENSEE, TOTAL_FLEET, CARSALES, CARHOLD_H, CARHOLD_L, CU, HA, HL, LL, NC, PL, TC, SV, WS, WS_NONRAC, OPERATIONAL_FLEET, BD, MM, TW, TB, WS_RAC, AVAILABLE_TB, FS, RL, RP, TN, AVAILABLE_FLEET, RT, SU, GOLD, PREDELIVERY, OVERDUE, ON_RENT, CI_HOURS, CI_HOURS_OFFSET, CI_DAYS, FLEET_ADV, FLEET_HOD
	FROM 
		[dbo].FLEET_EUROPE_ACTUAL FEA
	INNER JOIN [Settings].vw_NonRevStatus NRS ON 
		FEA.OPERSTAT = NRS.OperationalStatusCode 
	AND 
		FEA.MOVETYPE = NRS.MovementTypeCode
	INNER JOIN COUNTRIES C ON FEA.COUNTRY = C.country AND C.active = 1
	
	-- New addition requested by Uwe Ahrendt
	WHERE
		FEA.TOTAL_FLEET = 1
	
	
	-- Temporary: Delete Invalid Data
	----------------------------------------------------------------------------------------
	DELETE FROM FLEET_EUROPE_ACTUAL_NON_REV WHERE SERIAL IS NULL or SERIAL = ''
		

-- 3 - Non Revenue : Staging
--==========================================================================================		
		
	TRUNCATE TABLE [General].[Staging_Fleet]
		
	INSERT INTO [General].[Staging_Fleet]
	(
		Serial , Plate , Unit , Country , MSODate , Model , ModelDesc , Color ,
		CarGroup , OwnArea,  LastDailyFileDate , NRdays , Lstwwd , Duewwd , LstMlg , DrvName ,
		OperStat , MoveType , Fleet_rac_ops , Fleet_rac_ttl , Fleet_carsales , 
		Fleet_licensee , Fleet_adv , Fleet_hod , TotalFleet , AvailableFleet , OperationalFleet ,
		IDate , BDDays , MMDays , LstDate , DueDate , DepStat , CarHold , Prevwwd , LstNo , SDate
	)
	SELECT 
		SERIAL , LICENSE , UNIT , COUNTRY , MSODATE , MODEL , MODDESC , COLOR , 
		VC , OWNAREA , DATEADD(D,0,DATEDIFF(d,0,IMPORTTIME)) , DAYSREV , LSTWWD , DUEWWD , LSTMLG , DRVNAME ,
		OPERSTAT , MOVETYPE ,FLEET_RAC_OPS , FLEET_RAC_TTL , FLEET_CARSALES ,  
		FLEET_LICENSEE , FLEET_ADV , FLEET_HOD , TOTAL_FLEET , AVAILABLE_FLEET , 
		OPERATIONAL_FLEET ,
		IDATE , BDDAYS , MMDAYS , LSTDATE , DUEDATE , DEPSTAT , CARHOLD1 , PREVWWD , LSTNO , SDATE
	FROM 
		FLEET_EUROPE_ACTUAL_NON_REV
		
	UPDATE [General].[Staging_Fleet] SET
		VehicleId = DF.VehicleId,
		IsActive  = DF.IsActive
	FROM 
		[General].[Staging_Fleet] SF
	INNER JOIN [General].[Dim_Fleet] DF ON 
		SF.Serial	= DF.Serial
	AND
		SF.Country	= DF.Country
	WHERE
		SF.VehicleId IS NULL	
	
	UPDATE [General].Staging_Fleet SET
		DayGroupCode = DG.DayGroupCode
	FROM 
		[General].Staging_Fleet AS ST
	INNER JOIN [Settings].NonRev_Day_Groups AS DG ON 
		ST.NRdays >= DG.DayMin and ST.NRdays <= DG.DayMax	
		
-- 4 - Log
--==========================================================================================	

	--Close all open Cars in NonRevLog that are NOT Active in Fleet

	UPDATE [General].Fact_NonRevLog SET
		IsOpen  = 0 , 
		EndDate = @today,
		NRdays	= DATEDIFF(day,StartDate,@today)
	WHERE 
		IsOpen = 1 
	AND 
		VehicleId in 
			(SELECT distinct VehicleId FROM [General].Dim_Fleet WHERE IsActive = 0)

	
	----------------------------------------------------------------------------------------
	
	--Close Open Cars in Fleet except those in Staging
	
	UPDATE [General].Fact_NonRevLog SET
		IsOpen = 0 , 
		EndDate = @today ,
		NRdays	= DATEDIFF(day,StartDate,@today)
	WHERE 
		VehicleId in 
	(
		SELECT distinct VehicleId FROM [General].[Fact_NonRevLog] WHERE IsOpen = 1
		EXCEPT
		SELECT distinct VehicleId FROM [General].Staging_Fleet
	)
	AND IsOpen = 1
	
	UPDATE [General].[Fact_NonRevLog] SET
		DayGroupCode = DG.DayGroupCode
	FROM 
		[General].[Fact_NonRevLog] AS FNRL
	INNER JOIN [Settings].NonRev_Day_Groups AS DG ON 
		FNRL.NRdays >= DG.DayMin 
	AND 
		FNRL.NRdays <= DG.DayMax
	WHERE 
		FNRL.IsOpen = 0 
	AND	
		FNRL.EndDate = @today
	
	MERGE [General].Fact_NonRevLog AS TARGET
	USING [General].Staging_Fleet AS SOURCE
	ON 
	(
		TARGET.VehicleId = SOURCE.VehicleId AND
		TARGET.IsOpen	 = 1
	)
	WHEN MATCHED THEN
	UPDATE SET 
		TARGET.NRDays			= SOURCE.NRdays,
		TARGET.Fleet_rac_ops	= SOURCE.Fleet_rac_ops,
		TARGET.Fleet_rac_ttl	= SOURCE.Fleet_rac_ttl,
		TARGET.Fleet_carsales	= SOURCE.Fleet_carsales,
		TARGET.Fleet_licensee	= SOURCE.Fleet_licensee,
		TARGET.Fleet_adv		= SOURCE.Fleet_adv,
		TARGET.Fleet_hod		= SOURCE.Fleet_hod,
		TARGET.TotalFleet		= SOURCE.TotalFleet,
		TARGET.AvailableFleet	= SOURCE.AvailableFleet ,
		TARGET.OperationalFleet	= SOURCE.OperationalFleet,
		
		TARGET.Lstwwd			= SOURCE.Lstwwd,
		TARGET.Duewwd			= SOURCE.Duewwd,
		TARGET.LstMlg			= SOURCE.LstMlg,
		TARGET.OperStat			= SOURCE.OperStat,
		TARGET.MoveType			= SOURCE.MoveType,
		
		Target.LstDate = Source.LstDate,
		Target.LstNo = Source.LstNo,
		Target.DrvName = Source.DrvName
		
	WHEN NOT MATCHED THEN
	INSERT
	(
		VehicleId , NRdays , Lstwwd , Duewwd , LstMlg , DrvName , StartDate , EndDate , 
		ERDate , OperStat , MoveType , Remark , RemarkId , 
		IsOpen , Fleet_rac_ops , Fleet_rac_ttl ,  Fleet_carsales , Fleet_licensee , 
		Fleet_adv ,  Fleet_hod , TotalFleet ,  AvailableFleet ,  OperationalFleet ,
		IDate , BDDays , MMDays , LstDate , DueDate , DepStat , CarHold , Prevwwd , LstNo ,
		IsApproved , CarGroup , SDate , CountryCar
	)
	VALUES
	(
		SOURCE.VehicleId , SOURCE.NRdays , SOURCE.Lstwwd , SOURCE.Duewwd , 
		SOURCE.LstMlg , SOURCE.DrvName , SOURCE.LastDailyFileDate - SOURCE.NRdays ,
		null , null , SOURCE.OperStat , SOURCE.MoveType , null , null , 1 ,
		SOURCE.Fleet_rac_ops , SOURCE.Fleet_rac_ttl ,  SOURCE.Fleet_carsales , 
		SOURCE.Fleet_licensee , SOURCE.Fleet_adv ,  SOURCE.Fleet_hod , 
		SOURCE.TotalFleet ,  SOURCE.AvailableFleet ,  SOURCE.OperationalFleet ,
		SOURCE.IDate , SOURCE.BDDays , SOURCE.MMDays , SOURCE.LstDate , 
		SOURCE.DueDate , SOURCE.DepStat , SOURCE.CarHold , SOURCE.Prevwwd , SOURCE.LstNo ,
		0 , SOURCE.CarGroup , SOURCE.SDate , SOURCE.Country
	);

	UPDATE [General].[Fact_NonRevLog] SET
		DayGroupCode = DG.DayGroupCode
	FROM 
		[General].[Fact_NonRevLog] AS FNRL
	INNER JOIN [Settings].NonRev_Day_Groups AS DG ON 
		FNRL.NRdays >= DG.DayMin and FNRL.NRdays <= DG.DayMax
	WHERE 
		FNRL.IsOpen = 1 

	UPDATE [General].[Fact_NonRevLog] SET
		KCICode = OS.KCICode 
	FROM 
		[General].[Fact_NonRevLog] AS FNRL
	INNER JOIN [Settings].[Operational_Status] AS OS ON
		FNRL.OperStat = OS.OperationalStatusCode
	WHERE 
		FNRL.IsOpen = 1	

	UPDATE [General].Fact_NonRevLog  SET
		LocationId = l.dim_Location_id ,
		CountryLoc = l.country
	FROM 
		[General].Fact_NonRevLog nrl
	INNER JOIN LOCATIONS l ON nrl.Lstwwd = l.location
	WHERE 
		nrl.LocationId IS NULL	


	-- Get the Non Rev Log Id
	----------------------------------------------------------------------------------------
	UPDATE [General].Staging_Fleet SET
		NonRevLogId = NRL.NonRevLogId ,
		IsOpen		= 1
	FROM 
		[General].Staging_Fleet SF
	INNER JOIN
		(
			SELECT 
				NonRevLogId , VehicleId 
			FROM 
				General.Fact_NonRevLog
			WHERE 
				IsOpen = 1	
		) AS NRL ON
			SF.VehicleId = NRL.VehicleId
	WHERE
		SF.NonRevLogId  IS NULL
	
	-- Remark Id Null (1 = No Reason Code)
	-----------------------------------------------------------------------
	UPDATE [General].Fact_NonRevLog  SET 
		RemarkId = 1 
	WHERE 
		RemarkId is null	
		
	-- Generate Stats
	----------------------------------------------------------------------------------------	

	
	EXEC [General].[NonRev_Review_DailyReport]
  
 
	EXEC [General].[NonRev_Review_Summary] 

	--end
	--end
  
END