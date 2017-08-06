


CREATE FUNCTION [dbo].[fnGetFleetHertzOnDemandForOwnarea]
(
	@ownarea varchar(10)	
)
RETURNS bit
AS
BEGIN
	DECLARE @isHertzOnDemand bit
	IF 
		(
			((SELECT HertzOnDemand FROM AREACODES WHERE ownarea=@ownarea) = 1)
		)
		BEGIN
			SET @isHertzOnDemand = 1
		END
	ELSE
		BEGIN
			SET @isHertzOnDemand = 0
		END
	
	RETURN	@isHertzOnDemand		

END