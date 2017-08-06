

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetResCOHoursForImporttimeAndRsarrivaldateAndRsarrivaltime]
(
@importtime datetime,
@RsArrivalDate datetime,
@RsArrivalTime datetime
)
RETURNS int
AS
BEGIN
				DECLARE @startdatetime datetime
				SELECT @startdatetime = @importtime

				DECLARE @enddatetime datetime
SELECT @enddatetime = @RsArrivalDate + @RsArrivalTime

				RETURN
				(
					SELECT
CONVERT(int, (CONVERT(int, @enddatetime - @startdatetime)*24) + (24*(CONVERT(float, @enddatetime - @startdatetime) - CONVERT(int, @enddatetime - @startdatetime))))
				)	
END