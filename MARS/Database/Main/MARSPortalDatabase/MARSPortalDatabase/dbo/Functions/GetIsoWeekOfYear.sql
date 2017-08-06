-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION GetIsoWeekOfYear
(
	@DateEntered Date
)
RETURNS int
AS
BEGIN
	
	
	RETURN DatePart(ISO_WEEK, @DateEntered);

END