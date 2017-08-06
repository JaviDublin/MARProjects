-- =============================================
-- Author:		Nigel Rivett
-- Create date: June 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Import].[Insert_Fleet_Summary_History_Fact_Daily]
@Type varchar(20)
	
	
	
AS
BEGIN
	
	SET NOCOUNT ON;

	BEGIN TRY
	
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

		DECLARE @REP_DATE INT
		SELECT TOP 1 @REP_DATE = CONVERT(VARCHAR(8),REP_DATE,112)
		FROM FLEET_EUROPE_SUMMARY

		SELECT @key1 = CONVERT(VARCHAR(8),@REP_DATE)
		INSERT trace (entity, key1, key2)
		SELECT @entity, @key1, 'start'

		DELETE Inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY
		WHERE dim_Calendar_id = @REP_DATE	
		
		SELECT @rowcount = @@rowcount
		
		INSERT Trace (entity, key1, key2, data1)
		SELECT @entity, @key1, 'end', 'deleted=' + COALESCE(CONVERT(VARCHAR(20),@rowcount),'null')
			
			
		INSERT [Inp].Fact_FLEET_EUROPE_SUMMARY_HISTORY
		(
			[dim_Calendar_id] ,
			[COUNTRY] ,
			[dim_Location_id] ,
			car_group ,
			[FLEET_CARSALES] ,
			[FLEET_RAC_TTL] ,
			[FLEET_RAC_OPS] ,
			FLEET_LICENSEE ,
			
			TOTAL_FLEET, 
			CARSALES, 
			CARHOLD_H ,
			CARHOLD_L ,
			CU, 
			HA, 
			HL, 
			LL, 
			NC, 
			PL, 
			TC, 
			SV, 
			WS, 
			WS_NONRAC ,
			OPERATIONAL_FLEET, 
			BD, 
			MM, 
			TW, 
			TB, 
			WS_RAC ,
			AVAILABLE_TB ,
			FS, 
			RL, 
			RP, 
			TN, 
			AVAILABLE_FLEET, 
			RT, 
			SU, 
			GOLD, 
			PREDELIVERY, 
			OVERDUE, 
			ON_RENT	,
			TOTAL_FLEET_MIN ,
			CARSALES_MIN ,
			CARHOLD_H_MIN ,
			CARHOLD_L_MIN ,
			CU_MIN ,
			HA_MIN ,
			HL_MIN ,
			LL_MIN ,
			NC_MIN ,
			PL_MIN ,
			TC_MIN ,
			SV_MIN ,
			WS_MIN ,
			WS_NONRAC_MIN ,
			OPERATIONAL_FLEET_MIN ,
			BD_MIN ,
			MM_MIN ,
			TW_MIN ,
			TB_MIN ,
			WS_RAC_MIN ,
			AVAILABLE_TB_MIN ,
			FS_MIN ,
			RL_MIN ,
			RP_MIN ,
			TN_MIN ,
			AVAILABLE_FLEET_MIN ,
			RT_MIN ,
			SU_MIN ,
			GOLD_MIN ,
			PREDELIVERY_MIN ,
			OVERDUE_MIN ,
			ON_RENT_MIN ,
			TOTAL_FLEET_MAX ,
			CARSALES_MAX ,
			CARHOLD_H_MAX ,
			CARHOLD_L_MAX ,
			CU_MAX ,
			HA_MAX ,
			HL_MAX ,
			LL_MAX ,
			NC_MAX ,
			PL_MAX ,
			TC_MAX ,
			SV_MAX ,
			WS_MAX ,
			WS_NONRAC_MAX ,
			OPERATIONAL_FLEET_MAX ,
			BD_MAX ,
			MM_MAX ,
			TW_MAX ,
			TB_MAX ,
			WS_RAC_MAX ,
			AVAILABLE_TB_MAX ,
			FS_MAX ,
			RL_MAX ,
			RP_MAX ,
			TN_MAX ,
			AVAILABLE_FLEET_MAX ,
			RT_MAX ,
			SU_MAX ,
			GOLD_MAX ,
			PREDELIVERY_MAX ,
			OVERDUE_MAX ,
			ON_RENT_MAX ,
			TOTAL_FLEET_MAX_ABSOLUTE ,
			OPERATIONAL_FLEET_MAX_ABSOLUTE ,
			OVERDUE_MAX_ABSOLUTE ,
			ON_RENT_MAX_ABSOLUTE,
			FLEET_ADV ,
			FLEET_HOD
			)
	SELECT
			dim_Calendar_id ,
			h.COUNTRY ,
			dim_Location_id ,
			car_group ,
			FLEET_CARSALES ,
			FLEET_RAC_TTL ,
			FLEET_RAC_OPS ,
			FLEET_LICENSEE ,
			
			TOTAL_FLEET, 
			CARSALES, 
			CARHOLD_H ,
			CARHOLD_L ,
			CU, 
			HA, 
			HL, 
			LL, 
			NC, 
			PL, 
			TC, 
			SV, 
			WS, 
			WS_NONRAC ,
			OPERATIONAL_FLEET, 
			BD, 
			MM, 
			TW, 
			TB, 
			WS_RAC ,
			AVAILABLE_TB ,
			FS, 
			RL, 
			RP, 
			TN, 
			AVAILABLE_FLEET, 
			RT, 
			SU, 
			GOLD, 
			PREDELIVERY, 
			OVERDUE, 
			ON_RENT ,
			TOTAL_FLEET_MIN ,
			CARSALES_MIN ,
			CARHOLD_H_MIN ,
			CARHOLD_L_MIN ,
			CU_MIN ,
			HA_MIN ,
			HL_MIN ,
			LL_MIN ,
			NC_MIN ,
			PL_MIN ,
			TC_MIN ,
			SV_MIN ,
			WS_MIN ,
			WS_NONRAC_MIN ,
			OPERATIONAL_FLEET_MIN ,
			BD_MIN ,
			MM_MIN ,
			TW_MIN ,
			TB_MIN ,
			WS_RAC_MIN ,
			AVAILABLE_TB_MIN ,
			FS_MIN ,
			RL_MIN ,
			RP_MIN ,
			TN_MIN ,
			AVAILABLE_FLEET_MIN ,
			RT_MIN ,
			SU_MIN ,
			GOLD_MIN ,
			PREDELIVERY_MIN ,
			OVERDUE_MIN ,
			ON_RENT_MIN ,
			TOTAL_FLEET_MAX ,
			CARSALES_MAX ,
			CARHOLD_H_MAX ,
			CARHOLD_L_MAX ,
			CU_MAX ,
			HA_MAX ,
			HL_MAX ,
			LL_MAX ,
			NC_MAX ,
			PL_MAX ,
			TC_MAX ,
			SV_MAX ,
			WS_MAX ,
			WS_NONRAC_MAX ,
			OPERATIONAL_FLEET_MAX ,
			BD_MAX ,
			MM_MAX ,
			TW_MAX ,
			TB_MAX ,
			WS_RAC_MAX ,
			AVAILABLE_TB_MAX ,
			FS_MAX ,
			RL_MAX ,
			RP_MAX ,
			TN_MAX ,
			AVAILABLE_FLEET_MAX ,
			RT_MAX ,
			SU_MAX ,
			GOLD_MAX ,
			PREDELIVERY_MAX ,
			OVERDUE_MAX ,
			ON_RENT_MAX ,
			TOTAL_FLEET_MAX_ABSOLUTE ,
			OPERATIONAL_FLEET_MAX_ABSOLUTE ,
			OVERDUE_MAX_ABSOLUTE ,
			ON_RENT_MAX_ABSOLUTE ,
			FLEET_ADV ,
			FLEET_HOD
	
		FROM 
			FLEET_EUROPE_SUMMARY h
		LEFT JOIN inp.dim_Calendar d
				on h.REP_DATE = d.Rep_Date
		LEFT JOIN inp.dim_LOCATION l
				on l.LOCATION = h.LOCATION
			
			
		SELECT @rowcount = @@rowcount

		INSERT trace (entity, key1, key2, data1)
		SELECT @entity, @key1, 'end', 'inserted=' + COALESCE(CONVERT(VARCHAR(20),@rowcount),'null')	
	end
	END TRY
	BEGIN CATCH
		DECLARE 
			@ErrDesc VARCHAR(MAX) ,
			@ErrProc VARCHAR(128) ,
			@ErrLine VARCHAR(20)

			SELECT 	
				@ErrProc = ERROR_PROCEDURE()	,
				@ErrLine = ERROR_LINE()			,
				@ErrDesc = ERROR_MESSAGE()
			SELECT	
				@ErrProc = COALESCE(@ErrProc,'') ,
				@ErrLine = COALESCE(@ErrLine,'') ,
				@ErrDesc = COALESCE(@ErrDesc,'')
			
			INSERT	Trace (Entity, key1, data1, data2)
			SELECT	
				Entity = @entity, key1 = 'Failure',
				data1 = '<ErrProc=' + @ErrProc + '>'
					+ '<ErrLine=' + @ErrLine + '>'
					+ '<ErrDesc=' + @ErrDesc + '>',
				data2 = @key1
			RAISERROR('Failed %s', 16, -1, @ErrDesc)
	
	
	END CATCH
    

END