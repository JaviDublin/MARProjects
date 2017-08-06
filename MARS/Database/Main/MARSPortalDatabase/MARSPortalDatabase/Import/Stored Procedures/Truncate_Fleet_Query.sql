-- =============================================
-- Author:		Javier
-- Create date: June 2012
-- Description:	
-- =============================================
CREATE PROCEDURE [Import].[Truncate_Fleet_Query]
@Type varchar(20)
AS
BEGIN

	SET NOCOUNT ON;

	if @Type = 'Daily'
	   TRUNCATE TABLE FLEET_EUROPE_ACTUAL_QUERY_TABLE
   
END