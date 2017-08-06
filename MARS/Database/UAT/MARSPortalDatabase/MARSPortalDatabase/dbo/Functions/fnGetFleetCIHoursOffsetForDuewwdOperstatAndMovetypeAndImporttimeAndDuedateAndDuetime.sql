

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetFleetCIHoursOffsetForDuewwdOperstatAndMovetypeAndImporttimeAndDuedateAndDuetime]
(
@duewwd varchar(50),
@operstat varchar(10),
@movetype varchar(10),
@importtime datetime,
@duedate datetime,
@duetime datetime,
@lstOrc varchar(10)
)
RETURNS int
AS
BEGIN
				DECLARE @duedatetime datetime
				SELECT @duedatetime = @duedate + @duetime

DECLARE @rh int	-- regular RETURN_HOURS WITHOUT OFFSET
				SELECT @rh = 
				
				dbo.[fnGetFleetCIHoursForOperstatAndMovetypeAndImporttimeAndDuedateAndDuetime]
						(@operstat, @movetype, @importtime, @duedate, @duetime, @lstOrc, @duewwd)
				
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