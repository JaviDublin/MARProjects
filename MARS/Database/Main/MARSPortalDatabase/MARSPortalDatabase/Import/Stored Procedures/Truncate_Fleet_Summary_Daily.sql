create PROCEDURE [Import].[Truncate_Fleet_Summary_Daily]
@Type varchar(20)
AS
BEGIN
	
	SET NOCOUNT ON;

	if @Type = 'Daily'
	    TRUNCATE TABLE FLEET_EUROPE_SUMMARY
END