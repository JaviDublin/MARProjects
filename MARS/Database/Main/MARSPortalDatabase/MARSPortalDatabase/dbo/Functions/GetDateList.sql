
CREATE FUNCTION [dbo].[GetDateList](@fromDate smalldatetime, @count int)
RETURNS @Results TABLE (targetDate smalldatetime)
AS

    BEGIN      
    
    DECLARE @pos int
    set @pos = 0
    
    WHILE (@pos < @count)
	BEGIN
		INSERT INTO @Results
		SELECT DATEADD(d, @pos, @fromDate)

		set @pos = @pos + 1
	END	

    RETURN
END