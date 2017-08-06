

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetResCIHoursForImporttimeAndRtrndateAndRtrntime]
(
@importtime datetime,
@RtrnDate datetime,
@RtrnTime datetime
)
RETURNS int
AS
BEGIN
				DECLARE @startdatetime datetime
				SELECT @startdatetime = @importtime

				DECLARE @enddatetime datetime
				SELECT @enddatetime = @RtrnDate + @RtrnTime

				RETURN
				(
					SELECT
CONVERT(int, (CONVERT(int, @enddatetime - @startdatetime)*24) + (24*(CONVERT(float, @enddatetime - @startdatetime) - CONVERT(int, @enddatetime - @startdatetime))))
				)	
END