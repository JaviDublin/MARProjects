

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetResOnewayFromRentLocAndRtrnLoc]
(
@rent_loc varchar(10),
@rtrn_loc varchar(10)
)
RETURNS varchar(2)
AS
BEGIN
				RETURN
					(
					SELECT
CASE WHEN @rent_loc <> @rtrn_loc THEN 'Y' ELSE 'N' END
					)
END