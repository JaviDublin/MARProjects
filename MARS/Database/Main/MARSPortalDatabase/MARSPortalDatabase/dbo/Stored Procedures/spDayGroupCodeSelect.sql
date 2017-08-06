-- =============================================
-- Author:		Javier
-- Create date: October 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spDayGroupCodeSelect]
	
	@filtered BIT = NULL
	
AS
BEGIN
	
	SET NOCOUNT ON;
	IF @filtered IS NULL SET @filtered = 0
	
	IF @filtered = 1
	BEGIN
		SELECT 
			DayGroupCode 
		FROM 
			[Settings].NonRev_Day_Groups
		WHERE
			NonRevDayGroupId > 3
	END
	ELSE
	BEGIN
		SELECT 
			DayGroupCode 
		FROM 
			[Settings].NonRev_Day_Groups
	END
	

   
END