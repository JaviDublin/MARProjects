
CREATE FUNCTION [dbo].[fnGetOverdueForOperstatAndMovetypeAndDuedate]
(
	@operstat varchar(10),
	@movetype varchar(10),
	@duedate datetime		
)
RETURNS int
AS
BEGIN	
	DECLARE @act datetime	
	SELECT @act = CONVERT(datetime, DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())) + ' 12:00:00 AM')		

	RETURN
		(
			SELECT
				CASE
					WHEN
						(@operstat = 'RT')
						AND
						(@movetype = 'R-O')
						AND						
						(@duedate < @act)
					THEN
						1
					ELSE
						0
				END
		)
END