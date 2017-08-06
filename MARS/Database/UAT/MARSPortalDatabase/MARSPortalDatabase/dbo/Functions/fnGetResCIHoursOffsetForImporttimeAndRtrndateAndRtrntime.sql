

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetResCIHoursOffsetForImporttimeAndRtrndateAndRtrntime]
(
@importtime datetime,
@RtrnDate datetime,
@RtrnTime datetime
,@duewwd varchar(50)
)
RETURNS int
AS
BEGIN
				DECLARE @startdatetime datetime
				SELECT @startdatetime = @importtime

				DECLARE @enddatetime datetime
				SELECT @enddatetime = @RtrnDate + @RtrnTime

DECLARE @rh int	-- regular RETURN_HOURS WITHOUT OFFSET
				SELECT @rh = 
				(
					SELECT
CONVERT(int, (CONVERT(int, @enddatetime - @startdatetime)*24) + (24*(CONVERT(float, @enddatetime - @startdatetime) - CONVERT(int, @enddatetime - @startdatetime))))
				)
				RETURN
				(
				CASE
					WHEN
(SELECT @rh + (SELECT turnaround_hours FROM dbo.LOCATIONS WHERE location = @duewwd)) IS NULL
					THEN
					0
					ELSE
(SELECT @rh + (SELECT turnaround_hours FROM dbo.LOCATIONS WHERE location = @duewwd))
				END
				)	
END