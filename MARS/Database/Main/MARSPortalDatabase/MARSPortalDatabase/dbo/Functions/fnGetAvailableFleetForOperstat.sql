
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetAvailableFleetForOperstat]
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
						(@operstat IN ('RT', 'SU'))
					THEN
						1
					ELSE
						0
				END
		)
END