




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetOperationalFleetForOperstat]
(
	@operstat varchar(10)	
)
RETURNS int
AS
BEGIN

	RETURN
		(
			SELECT
				CASE
					WHEN
						(@operstat IN ('BD', 'MM', 'TW', 'TB', 'FS', 'RL', 'RP', 'TN', 'RT', 'SU'))
					THEN
						1
					ELSE
						0
				END
		)
END