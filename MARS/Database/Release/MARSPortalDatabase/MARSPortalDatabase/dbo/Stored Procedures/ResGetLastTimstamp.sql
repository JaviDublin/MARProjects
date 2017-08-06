-- =============================================
CREATE PROCEDURE [dbo].[ResGetLastTimstamp]
	@Timestamp datetime output 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT top 1 @Timestamp=TeradataTimeStamp
	from dbo.ReservationTeradataControlLog
	where processed = 1
	order by ReservationTeradataControlLogId desc
	
END