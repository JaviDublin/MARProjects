

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetResRentLocFromResLoc]
(
			@res_loc varchar(10)
)
RETURNS varchar(10)
AS
BEGIN
				RETURN
				(
					SELECT
						served_by_locn
					FROM
						dbo.LOCATIONS
					WHERE
						(location = @res_loc)
				)
END