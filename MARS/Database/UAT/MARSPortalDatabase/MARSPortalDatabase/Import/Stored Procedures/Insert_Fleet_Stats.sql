-- =============================================
-- Author:		Javier
-- Create date: June 2012
-- Description:	4) FLEET_EUROPE_SUMMARY -> FLEET_EUROPE_STATS
-- =============================================
CREATE PROCEDURE [Import].[Insert_Fleet_Stats] 
	
AS
	SET NOCOUNT ON;
	
BEGIN TRY
DECLARE @rowcount int, @inserted int, @updated int
DECLARE @entity varchar(100)
DECLARE @key1 varchar(100)
DECLARE @data2 varchar(max)

	select @entity = OBJECT_NAME(@@PROCID)
	select @entity = coalesce(@entity,'test')

	insert trace (entity, key1, key2, data2)
	select @entity, @key1, 'start', @data2
	
	INSERT INTO dbo.FLEET_EUROPE_STATS
	(
	IMPORTDATE,
	REP_YEAR,
		REP_MONTH,
		REP_WEEK_OF_YEAR,
		REP_DAY_OF_WEEK,
		REP_DATE,
		COUNTRY,
		LOCATION, 
		CAR_GROUP, 
		FLEET_RAC_TTL,
		FLEET_RAC_OPS,
		FLEET_CARSALES,
		FLEET_LICENSEE,
		FLEET_ADV,
		FLEET_HOD,
		TOTAL_FLEET, 
		CARSALES, 
		CARHOLD_H, 
		CARHOLD_L, 
		CU, 
		HA, 
		HL, 
		LL, 
		NC, 
		PL, 
		TC, 
		SV,
		WS,
		WS_NONRAC, 
		OPERATIONAL_FLEET, 
		BD, 
		MM, 
		TW, 
		TB, 
		WS_RAC, 
		AVAILABLE_TB,
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
		ON_RENT
	)
	SELECT 
		GETDATE() AS IMPORTDATE, 
		REP_YEAR,
		REP_MONTH,
		REP_WEEK_OF_YEAR,
		REP_DAY_OF_WEEK,
		REP_DATE,
		COUNTRY,
		LOCATION, 
		CAR_GROUP, 
		FLEET_RAC_TTL,
		FLEET_RAC_OPS,
		FLEET_CARSALES,
		FLEET_LICENSEE,
		FLEET_ADV,
		FLEET_HOD,
		TOTAL_FLEET, 
		CARSALES, 
		CARHOLD_H, 
		CARHOLD_L, 
		CU, 
		HA, 
		HL, 
		LL, 
		NC, 
		PL, 
		TC, 
		SV,
		WS,
		WS_NONRAC, 
		OPERATIONAL_FLEET, 
		BD, 
		MM, 
		TW, 
		TB, 
		WS_RAC, 
		AVAILABLE_TB,
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
		ON_RENT
	FROM 
		dbo.FLEET_EUROPE_SUMMARY
	WHERE 
		DATEADD(dd, 0, DATEDIFF(dd, 0, dbo.FLEET_EUROPE_SUMMARY.REP_DATE)) = 
		DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))
    
	select @rowcount = @@rowcount

	insert trace (entity, key1, key2, data1, data2)
	select @entity, @key1, 'end', 'updated=' + coalesce(convert(varchar(20),@rowcount),'null')
						, @data2

end try
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