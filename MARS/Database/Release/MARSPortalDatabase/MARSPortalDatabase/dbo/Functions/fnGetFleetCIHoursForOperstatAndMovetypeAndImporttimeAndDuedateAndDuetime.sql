

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetFleetCIHoursForOperstatAndMovetypeAndImporttimeAndDuedateAndDuetime]
(
	@operstat varchar(10),
	@movetype varchar(10),
	@importtime datetime,
	@duedate datetime,
	@duetime datetime,
	@lstOrc varchar(10),
	@dueWwd varchar(10)
)
RETURNS int
AS
BEGIN
				DECLARE @duedatetime datetime
				SELECT @duedatetime = @duedate + @duetime

				RETURN
				(
				SELECT
					CASE
						WHEN
							(@lstOrc = 'O')
						THEN
							CASE
								WHEN
									(LEFT(@duewwd, 2) IN ('UK', 'IR', 'PG'))
								THEN
									DATEDIFF(HOUR, @importtime, @duedatetime)
								ELSE
									DATEDIFF(HOUR, @importtime, DATEADD(HOUR, -1, @duedatetime))
							END
						ELSE
							NULL
					END

				)	
END