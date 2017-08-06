

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetFleetCIDaysForOperstatAndMovetypeAndImporttimeAndDuedate]
(
@operstat varchar(10),
@movetype varchar(10),
@importtime datetime,
@duedate datetime
)
RETURNS int
AS
BEGIN
				RETURN
				(
				SELECT
					CASE 
						WHEN 
						(@operstat = 'RT')
						AND
						(@movetype = 'R-O')
						THEN
							CONVERT(int, @duedate - CONVERT(datetime, CONVERT(char, @importtime, 112)))		
					ELSE
						CASE 
							WHEN 
							(@movetype = 'T-O')
							THEN
								CONVERT(int, @duedate - CONVERT(datetime, CONVERT(char, @importtime, 112)))			
						ELSE
							0
						END
					END
				)	
END