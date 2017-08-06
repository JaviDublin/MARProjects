-- =============================================
-- Author:		Javier
-- Create date: October 2010
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnAddCommaToInt]
(
	@value INT
)
RETURNS VARCHAR(20)
AS
BEGIN
	
	DECLARE @valueString VARCHAR(20)
	SET @valueString = CONVERT(VARCHAR(20),@value)
	
	DECLARE @valueLen INT
	SET @valueLen = LEN(@valueString)
	
	DECLARE @result VARCHAR(20)
	
	
	IF @valueLen <= 3
	BEGIN
		SET @result = @valueString
	END
	ELSE IF @valueLen > 3 AND @valueLen < 7
	BEGIN
		DECLARE @rightPart VARCHAR(3) , @leftPart VARCHAR(3)
		SET 	@rightPart = SUBSTRING(@valueString , 1 , @valueLen - 3)
		SET		@leftPart  = SUBSTRING(@valueString , @valueLen - 2 , @valueLen)
		
		SET @result = @rightPart + ',' + @leftPart
		
	END
	ELSE
	BEGIN
		SET @result = @valueString
	END
		
	RETURN @result
	

END