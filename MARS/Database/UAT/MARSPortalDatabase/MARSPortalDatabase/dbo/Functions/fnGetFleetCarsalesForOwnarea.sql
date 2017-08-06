

CREATE FUNCTION [dbo].[fnGetFleetCarsalesForOwnarea]
(
	@ownarea varchar(10)	
)
RETURNS bit
AS
BEGIN
	DECLARE @isCarsales bit
	IF 
		(
			((SELECT carsales FROM AREACODES WHERE ownarea=@ownarea) = 1)
		)
		BEGIN
			SET @isCarsales = 1
		END
	ELSE
		BEGIN
			SET @isCarsales = 0
		END
	
	RETURN	@isCarsales		

END