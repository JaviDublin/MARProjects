CREATE FUNCTION [dbo].[SplitStringComma] ( @stringToSplit VARCHAR(MAX), @selection int )
RETURNS nvarchar(500)
AS
BEGIN

 DECLARE @name NVARCHAR(255) 
 DECLARE @pos INT
 declare @counter int = 0

 WHILE CHARINDEX(',', @stringToSplit) > 0
 BEGIN
  SELECT @pos  = CHARINDEX(',', @stringToSplit)  

  if RIGHT(@stringToSplit,1) = ','
  begin
	return null
  end
  
  SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)

	if @counter = @selection
	begin
		 RETURN @name
	end
  

  SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
  select @counter = @counter + 1
 END
 
	if @counter = @selection
	begin
		return @stringToSplit
	end

return null
END