-- =============================================
-- Author:		Javier
-- Create date: June 2012
-- Description:	
-- =============================================
CREATE PROCEDURE [Import].[Truncate_Fleet_Stats]
@Type varchar(20)

AS
BEGIN
	
	SET NOCOUNT ON;

	if @Type = 'Daily'
	    TRUNCATE TABLE dbo.FLEET_EUROPE_STATS
END