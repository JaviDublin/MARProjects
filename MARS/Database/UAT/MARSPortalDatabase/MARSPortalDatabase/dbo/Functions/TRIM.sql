CREATE FUNCTION [dbo].[TRIM]
	(
	@string varchar(MAX)
	)
RETURNS varchar(MAX)
BEGIN

	RETURN LTRIM(RTRIM(@string))

END