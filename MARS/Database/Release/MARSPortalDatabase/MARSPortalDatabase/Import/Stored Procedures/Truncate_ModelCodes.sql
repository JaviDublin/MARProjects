-- =============================================
-- Author:		Javier
-- Create date: June 2012
-- Description:	
-- =============================================
CREATE PROCEDURE [Import].[Truncate_ModelCodes]
@Type varchar(20)
	
AS
BEGIN
	
	SET NOCOUNT ON;

	if @Type = 'Daily'
	    TRUNCATE TABLE MODELCODES
    
END