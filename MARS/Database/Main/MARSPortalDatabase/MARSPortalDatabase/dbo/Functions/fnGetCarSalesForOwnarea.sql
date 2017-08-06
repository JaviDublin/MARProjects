



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetCarSalesForOwnarea]
(
	@ownarea varchar(10)		
)
RETURNS int
AS
BEGIN
	DECLARE @isCarsales AS bit
	IF 
		(SELECT carsales FROM AREACODES WHERE (ownarea=@ownarea)) = 1
		BEGIN
			SET @isCarsales = 1
		END
	ELSE
		BEGIN
			SET @isCarsales = 0
		END	
	RETURN @isCarsales
END