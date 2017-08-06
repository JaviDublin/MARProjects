CREATE procedure [dbo].[spPortal_Transfer_REA]
-- Add the parameters for the stored procedure here

AS
BEGIN	-- (Start of stored procedure)
	-- Set nocount on added to prevent extra result sets from
	-- interfering with select statements.
	SET NOCOUNT ON;

	-- Declare transaction and set to false
	DECLARE @TranStarted   BIT
	SET @TranStarted = 0


	--Check transaction count and Begin Transaction
	IF( @@TRANCOUNT = 0 )
		BEGIN
			BEGIN TRANSACTION -- Start transaction
			SET @TranStarted = 1
		END
	ELSE
			SET @TranStarted = 0


	--	DELETE current records from dbo.RESERVATIONS_EUROPE_ACTUAL 
	BEGIN	-- (Start of delete function)
		
		TRUNCATE TABLE dbo.RESERVATIONS_EUROPE_ACTUAL 
		
		-- Check if error occured --
		IF (@@ERROR <> 0) 
		GOTO CLEANUP	
		
	END	-- (End of delete function)


	--	TRANSFER DATA from dbo.RESERVATIONS_EUROPE_ACTUAL to dbo.RESERVATIONS_EUROPE_ACTUAL_QUERY_TABLE 
	BEGIN	-- (Start of Transfer data from REA to REA_Query Table)

		INSERT INTO dbo.RESERVATIONS_EUROPE_ACTUAL 
		(
			IMPORTTIME, 
			REP_YEAR, 
			REP_MONTH, 
			COUNTRY, 
			CMS_POOL, 
			CMS_LOC_GRP, 
			OPS_REGION, 
			OPS_AREA, 
			CAR_SEGMENT, 
			CAR_CLASS, 
			CARVAN, 
			RES_ID_NBR,
			RES_LOC, 
			RENT_LOC, 
			RTRN_LOC, 
			ICIND, 
			ONEWAY, 
			RS_ARRIVAL_DATE, 
			RS_ARRIVAL_TIME, 
			RTRN_DATE, 
			RTRN_TIME, 
			RES_DAYS, 
			RES_VEH_CLASS, 
			GR_INCL_GOLDUPGR, 
			RATE_QUOTED, 
			SUBTOTAL_2, 
			MOP, 
			PREPAID, 
			NEVERLOST, 
			PREDELIVERY, 
			CUST_NAME, 
			PHONE, 
			CDPID_NBR, 
			CNTID_NBR, 
			NO1_CLUB_GOLD, 
			TACO, 
			FLIGHT_NBR, 
			REMARKS, 
			GS, 
			N1TYPE, 
			DATE_SOLD, 
			R1, 
			R2, 
			R3, 
			TS, 
			CO_HOURS, 
			CO_DAYS, 
			CI_HOURS, 
			CI_HOURS_OFFSET, 
			CI_DAYS
		)
		SELECT
			IMPORTTIME, 
			REP_YEAR, 
			REP_MONTH, 
			COUNTRY, 
			CMS_POOL, 
			CMS_LOC_GRP, 
			OPS_REGION, 
			OPS_AREA, 
			CAR_SEGMENT, 
			CAR_CLASS, 
			CARVAN, 
			RES_ID_NBR,
			RES_LOC, 
			RENT_LOC, 
			RTRN_LOC, 
			ICIND, 
			ONEWAY, 
			RS_ARRIVAL_DATE, 
			RS_ARRIVAL_TIME, 
			RTRN_DATE, 
			RTRN_TIME, 
			RES_DAYS, 
			RES_VEH_CLASS, 
			GR_INCL_GOLDUPGR, 
			RATE_QUOTED, 
			SUBTOTAL_2, 
			MOP, 
			PREPAID, 
			NEVERLOST, 
			PREDELIVERY, 
			CUST_NAME, 
			PHONE, 
			CDPID_NBR, 
			CNTID_NBR, 
			NO1_CLUB_GOLD, 
			TACO, 
			FLIGHT_NBR, 
			REMARKS, 
			GS, 
			N1TYPE, 
			DATE_SOLD, 
			R1, 
			R2, 
			R3, 
			TS, 
			CO_HOURS, 
			CO_DAYS, 
			CI_HOURS, 
			CI_HOURS_OFFSET, 
			CI_DAYS			
		FROM
			dbo.RESERVATIONS_EUROPE_ACTUAL_QUERY_TABLE

		-- Check if error occured --
		IF (@@ERROR <> 0) 
		GOTO CLEANUP	
	END

	-- No error Commit transaction --
	IF( @TranStarted = 1 )
	BEGIN
		SET @TranStarted = 0
		COMMIT TRANSACTION
	END

	-- ROLLBACK TRANSACTION --
	CLEANUP:
	IF (@TranStarted = 1)
	BEGIN
		SET @TranStarted = 0
		ROLLBACK TRANSACTION
	END
	
END	-- (End of stored procedure)