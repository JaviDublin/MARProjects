CREATE PROCEDURE [dbo].[ReservationUpdate]

AS
BEGIN

	SET NOCOUNT ON;

	update dbo.Reservations 
	set 	[CO_HOURS]=DateDiff(hour,GETDATE(),([RS_ARRIVAL_DATE]+Convert(time,[RS_ARRIVAL_TIME])))
	,[CO_DAYS]=DateDiff(day,GETDATE(),([RS_ARRIVAL_DATE]+Convert(time,[RS_ARRIVAL_TIME])))
	,[CI_HOURS]=DateDiff(hour,GETDATE(),([RTRN_DATE]+Convert(time,[RTRN_TIME])))
	,[CI_HOURS_OFFSET]=DateDiff(hour,GETDATE(),([RTRN_DATE]+Convert(time,[RTRN_TIME])))
	,[CI_DAYS]=DateDiff(day,GETDATE(),([RTRN_DATE]+Convert(time,[RTRN_TIME])))
END