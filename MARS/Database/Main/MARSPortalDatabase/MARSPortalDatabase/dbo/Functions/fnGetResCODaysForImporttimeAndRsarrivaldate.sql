

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetResCODaysForImporttimeAndRsarrivaldate]
(
@importtime datetime,
@RsArrivalDate datetime
)
RETURNS int
AS
BEGIN
				DECLARE @startdate datetime
SELECT @startdate = CONVERT(datetime, CONVERT(char, @importtime, 112))

				DECLARE @enddate datetime
SELECT @enddate = @RsArrivalDate

				RETURN
				(
					SELECT
CONVERT(int, @enddate - @startdate)
				)	
END