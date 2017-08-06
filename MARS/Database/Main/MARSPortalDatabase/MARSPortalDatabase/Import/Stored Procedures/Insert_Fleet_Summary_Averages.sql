﻿-- =============================================
-- Author:		Javier
-- Create date: June 2012
-- Description:	5) FLEET_EUROPE_STATS -> FLEET_EUROPE_SUMMARY
-- =============================================
CREATE PROCEDURE [Import].[Insert_Fleet_Summary_Averages]
@Type varchar(20)
AS
BEGIN TRY
	
	SET NOCOUNT ON;

if @Type = 'Daily'
begin
	DECLARE 
		@rowcount	INT			, 
		@inserted	INT			, 
		@updated	INT			, 
		@entity		VARCHAR(100), 
		@key1		VARCHAR(100), 
		@data2		VARCHAR(MAX)
	    
	SELECT @entity = OBJECT_NAME(@@PROCID)
	SELECT @entity = COALESCE(@entity,'test')
	DECLARE @dte2 DATETIME
	select @dte2 = MIN(REP_DATE) FROM dbo.FLEET_EUROPE_STATS

	select @key1 = convert(varchar(23),GETDATE(),126)
	select @data2 = 'procdate=' + convert(varchar(23),@dte2,126) + ' repdate=' + convert(varchar(23),@dte2,126)

	insert trace (entity, key1, key2, data2)
	select @entity, @key1, 'start', @data2

    INSERT INTO dbo.FLEET_EUROPE_SUMMARY
	SELECT 
		
		AV.REP_YEAR, AV.REP_MONTH, AV.REP_WEEK_OF_YEAR, AV.REP_DAY_OF_WEEK, AV.REP_DATE
		, AV.COUNTRY, AV.LOCATION, AV.CAR_GROUP, AV.FLEET_RAC_TTL, 
		AV.FLEET_RAC_OPS, AV.FLEET_CARSALES,	AV.FLEET_LICENSEE ,
		AV.TOTAL_FLEET, AV.CARSALES, AV.CARHOLD_H, AV.CARHOLD_L, AV.CU, AV.HA, AV.HL, AV.LL, AV.NC, AV.PL, AV.TC
		, AV.SV, AV.WS, AV.WS_NONRAC, AV.OPERATIONAL_FLEET, AV.BD, AV.MM, AV.TW, AV.TB, AV.WS_RAC, AV.AVAILABLE_TB
		, AV.FS, AV.RL, AV.RP, AV.TN, AV.AVAILABLE_FLEET, AV.RT, AV.SU, AV.GOLD, 
		AV.PREDELIVERY, AV.OVERDUE, AV.ON_RENT
		
		, MI.TOTAL_FLEET AS [TOTAL_FLEET_MIN], MI.CARSALES AS [CARSALES_MIN], MI.CARHOLD_H AS [CARHOLD_H_MIN]
		, MI.CARHOLD_L AS [CARHOLD_L_MIN], MI.CU AS [CU_MIN], MI.HA AS [HA_MIN], MI.HL AS [HL_MIN]
		, MI.LL AS [LL_MIN], MI.NC AS [NC_MIN], MI.PL AS [PL_MIN], MI.TC AS [TC_MIN], MI.SV AS [SV_MIN]
		, MI.WS AS [WS_MIN], MI.WS_NONRAC AS [WS_NONRAC_MIN], MI.OPERATIONAL_FLEET AS [OPERATIONAL_FLEET_MIN]
		, MI.BD AS [BD_MIN], MI.MM AS [MM_MIN], MI.TW AS [TW_MIN], MI.TB AS [TB_MIN], MI.WS_RAC AS [WS_RAC_MIN]
		, MI.AVAILABLE_TB AS [AVAILABLE_TB_MIN], MI.FS AS[FS_MIN], MI.RL AS [RL_MIN], MI.RP AS [RP_MIN], 
		MI.TN AS [TN_MIN]
		, MI.AVAILABLE_FLEET AS [AVAILABLE_FLEET_MIN], MI.RT AS [RT_MIN], MI.SU AS [SU_MIN], MI.GOLD AS [GOLD_MIN]
		, MI.PREDELIVERY AS [PREDELIVERY_MIN], MI.OVERDUE AS [OVERDUE_MIN], MI.ON_RENT AS [ON_RENT_MIN]
		
		, MX.TOTAL_FLEET AS [TOTAL_FLEET_MAX], MX.CARSALES AS [CARSALES_MAX], MX.CARHOLD_H AS [CARHOLD_H_MAX]
		, MX.CARHOLD_L AS [CARHOLD_L_MAX], MX.CU AS [CU_MAX], MX.HA AS [HA_MAX], MX.HL AS [HL_MAX]
		, MX.LL AS [LL_MAX], MX.NC AS [NC_MAX], MX.PL AS [PL_MAX], MX.TC AS [TC_MAX], MX.SV AS [SV_MAX]
		, MX.WS AS [WS_MAX], MX.WS_NONRAC AS [WS_NONRAC_MAX], MX.OPERATIONAL_FLEET AS [OPERATIONAL_FLEET_MAX]
		, MX.BD AS [BD_MAX], MX.MM AS [MM_MAX], MX.TW AS [TW_MAX], MX.TB AS [TB_MAX], MX.WS_RAC AS [WS_RAC_MAX]
		, MX.AVAILABLE_TB AS [AVAILABLE_TB_MAX], MX.FS AS[FS_MAX], MX.RL AS [RL_MAX], MX.RP AS [RP_MAX], 
		MX.TN AS [TN_MAX]
		, MX.AVAILABLE_FLEET AS [AVAILABLE_FLEET_MAX], MX.RT AS [RT_MAX], MX.SU AS [SU_MAX], MX.GOLD AS [GOLD_MAX]
		, MX.PREDELIVERY AS [PREDELIVERY_MAX], MX.OVERDUE AS [OVERDUE_MAX], MX.ON_RENT AS [ON_RENT_MAX]
		, AV.TOTAL_FLEET_MAX_ABSOLUTE, AV.OPERATIONAL_FLEET_MAX_ABSOLUTE
		, AV.OVERDUE_MAX_ABSOLUTE, AV.ON_RENT_MAX_ABSOLUTE ,
		 
		 AV.FLEET_ADV , AV.FLEET_HOD
					
	FROM
	(
		-- 1. Average
		SELECT
			(SELECT MIN(REP_YEAR) FROM dbo.FLEET_EUROPE_STATS) AS REP_YEAR,
			(SELECT MIN(REP_MONTH) FROM dbo.FLEET_EUROPE_STATS) AS REP_MONTH,
			(SELECT MIN(REP_WEEK_OF_YEAR) FROM dbo.FLEET_EUROPE_STATS) AS REP_WEEK_OF_YEAR,
			(SELECT MIN(REP_DAY_OF_WEEK) FROM dbo.FLEET_EUROPE_STATS) AS REP_DAY_OF_WEEK,
			(SELECT MIN(REP_DATE) FROM dbo.FLEET_EUROPE_STATS) AS REP_DATE,
			COUNTRY, LOCATION, CAR_GROUP, 
			FLEET_RAC_TTL, FLEET_RAC_OPS,	FLEET_CARSALES,	FLEET_LICENSEE, FLEET_ADV , FLEET_HOD,
			AVG(TOTAL_FLEET) AS TOTAL_FLEET, AVG(CARSALES) AS CARSALES, AVG(CARHOLD_H) AS CARHOLD_H, 
			AVG(CARHOLD_L) AS CARHOLD_L, AVG(CU) AS CU, AVG(HA) AS HA, AVG(HL) AS HL, AVG(LL) AS LL, 
			AVG(NC) AS NC, AVG(PL) AS PL, AVG(TC) AS TC, AVG(SV) AS SV, AVG(WS) AS WS, AVG(WS_NONRAC) AS WS_NONRAC, 
			AVG(OPERATIONAL_FLEET) AS OPERATIONAL_FLEET, AVG(BD) AS BD, AVG(MM) AS MM, 
			AVG(TW) AS TW, AVG(TB) AS TB, AVG(WS_RAC) AS WS_RAC, 	AVG(AVAILABLE_TB) AS AVAILABLE_TB,
			AVG(FS) AS FS, AVG(RL) AS RL, AVG(RP) AS RP, AVG(TN) AS TN, AVG(AVAILABLE_FLEET) AS AVAILABLE_FLEET, 
			AVG(RT) AS RT, AVG(SU) AS SU, AVG(GOLD) AS GOLD, AVG(PREDELIVERY) AS PREDELIVERY, 
			AVG(OVERDUE) AS OVERDUE, AVG(ON_RENT) AS ON_RENT
			, MAX(TOTAL_FLEET) AS TOTAL_FLEET_MAX_ABSOLUTE
			, MAX(OPERATIONAL_FLEET) AS OPERATIONAL_FLEET_MAX_ABSOLUTE
			, MAX(OVERDUE) AS OVERDUE_MAX_ABSOLUTE
			, MAX(ON_RENT) AS ON_RENT_MAX_ABSOLUTE
		FROM 
			dbo.FLEET_EUROPE_STATS
		GROUP BY 
			COUNTRY, LOCATION, CAR_GROUP, FLEET_RAC_TTL, FLEET_RAC_OPS, FLEET_CARSALES, FLEET_LICENSEE ,
			FLEET_ADV , FLEET_HOD
	) AV INNER JOIN
	(
		-- 2. Maximum
		SELECT 
			COUNTRY, LOCATION, CAR_GROUP, FLEET_RAC_TTL, FLEET_RAC_OPS,	FLEET_CARSALES,	FLEET_LICENSEE,
			 FLEET_ADV , FLEET_HOD, 
			TOTAL_FLEET, CARSALES, CARHOLD_H, CARHOLD_L, CU, HA, HL, LL, NC, PL, TC, SV, WS, WS_NONRAC, 
			OPERATIONAL_FLEET, BD, MM, TW, TB, WS_RAC, AVAILABLE_TB, FS, RL, RP, TN, 
			AVAILABLE_FLEET, RT, SU, GOLD, PREDELIVERY, OVERDUE, ON_RENT
		FROM ( 	
			SELECT --ISNULL(ON_RENT / NULLIF(OPERATIONAL_FLEET, 0),0) AS [Utilization], IMPORTDATE as [idate]
					Rank() OVER (PARTITION BY 
										COUNTRY,
										LOCATION,
										CAR_GROUP,
										FLEET_RAC_TTL,
										FLEET_RAC_OPS,
										FLEET_CARSALES,
										FLEET_LICENSEE, 
										FLEET_ADV , 
										FLEET_HOD 
									ORDER BY 
										ISNULL(ON_RENT / NULLIF(OPERATIONAL_FLEET, 0),0) DESC, 
										IMPORTDATE ASC) AS [RANK_UTIL_MAX]					
					, *
			FROM 
				dbo.FLEET_EUROPE_STATS		
		) AS FEST
		WHERE FEST.RANK_UTIL_MAX = 1
	) AS MX ON 
		AV.COUNTRY			= MX.COUNTRY 
	AND AV.LOCATION			= MX.LOCATION 
	AND AV.CAR_GROUP		= MX.CAR_GROUP 
	AND AV.FLEET_RAC_TTL	= MX.FLEET_RAC_TTL
	AND AV.FLEET_RAC_OPS	= MX.FLEET_RAC_OPS 
	AND AV.FLEET_CARSALES	= MX.FLEET_CARSALES 
	AND AV.FLEET_LICENSEE	= MX.FLEET_LICENSEE
	AND AV.FLEET_ADV		= MX.FLEET_ADV 
	AND AV.FLEET_HOD		= MX.FLEET_HOD
	 INNER JOIN
	(
		-- 3. Minimum
		SELECT 
			COUNTRY, LOCATION, CAR_GROUP, FLEET_RAC_TTL, FLEET_RAC_OPS,	FLEET_CARSALES,	FLEET_LICENSEE,
			FLEET_ADV , FLEET_HOD, 
			TOTAL_FLEET, CARSALES, CARHOLD_H, CARHOLD_L, CU, HA, HL, LL, NC, PL, TC, SV, WS, 
			WS_NONRAC, OPERATIONAL_FLEET, BD, MM, TW, TB, WS_RAC, AVAILABLE_TB, FS, RL, RP, TN, 
			AVAILABLE_FLEET, 
			RT, SU, GOLD, PREDELIVERY, OVERDUE, ON_RENT
		FROM ( 	
			SELECT --ISNULL(ON_RENT / NULLIF(OPERATIONAL_FLEET, 0),0) AS [Utilization], IMPORTDATE as [idate]					
					Rank() OVER (PARTITION BY 
									COUNTRY,
									LOCATION,
									CAR_GROUP,
									FLEET_RAC_TTL,
									FLEET_RAC_OPS,
									FLEET_CARSALES,
									FLEET_LICENSEE , 
									FLEET_ADV , 
									FLEET_HOD
								ORDER BY 
								ISNULL(ON_RENT / NULLIF(OPERATIONAL_FLEET, 0),0) ASC, 
								IMPORTDATE ASC) AS [RANK_UTIL_MIN]
					, *
			FROM 
				dbo.FLEET_EUROPE_STATS		
		) AS FEST
		WHERE FEST.RANK_UTIL_MIN = 1
	) MI ON 
		AV.COUNTRY			= MI.COUNTRY 
	AND AV.LOCATION			= MI.LOCATION 
	AND AV.CAR_GROUP		= MI.CAR_GROUP 
	AND AV.FLEET_RAC_TTL	= MI.FLEET_RAC_TTL
	AND AV.FLEET_RAC_OPS	= MI.FLEET_RAC_OPS 
	AND AV.FLEET_CARSALES	= MI.FLEET_CARSALES 
	AND AV.FLEET_LICENSEE	= MI.FLEET_LICENSEE
	AND AV.FLEET_ADV		= MI.FLEET_ADV 
	AND AV.FLEET_HOD		= MI.FLEET_HOD
	
	select @rowcount = @@rowcount	

	insert trace (entity, key1, key2, data1, data2)
	select @entity, @key1, 'end','instered=' + coalesce(convert(varchar(20),@rowcount),'null'), @data2	
	
end	

END TRY
begin catch
declare @ErrDesc varchar(max)
declare @ErrProc varchar(128)
declare @ErrLine varchar(20)

	select 	@ErrProc = ERROR_PROCEDURE() ,
		@ErrLine = ERROR_LINE() ,
		@ErrDesc = ERROR_MESSAGE()
	select	@ErrProc = coalesce(@ErrProc,'') ,
		@ErrLine = coalesce(@ErrLine,'') ,
		@ErrDesc = coalesce(@ErrDesc,'')
		
	insert	Trace (Entity, key1, data1, data2)
	select	Entity = @entity, key1 = 'Failure',
		data1 = '<ErrProc=' + @ErrProc + '>'
			+ '<ErrLine=' + @ErrLine + '>'
			+ '<ErrDesc=' + @ErrDesc + '>',
		data2 = @key1
	raiserror('Failed %s', 16, -1, @ErrDesc)
end catch