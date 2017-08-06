-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetTotalFleetForLicenseAndOwnwwd]
(
	@license varchar(25),
	@ownwwd varchar(10)
)
RETURNS int
AS
BEGIN
	DECLARE @isTtlFleet bit

	IF 
		(
			((LEFT(@ownwwd,(2)) = 'GE') AND (LEFT(@license, 2) = 'XX'))				-- Exception for Germany; 'XX' - license plates are 'dummy', vehicles don't belong to the fleet yet (prior to in-fleet)
		)
	BEGIN
		SET @isTtlFleet = 0
	END
	ELSE
	BEGIN
		SET @isTtlFleet = 1
	END

	RETURN @isTtlFleet

END