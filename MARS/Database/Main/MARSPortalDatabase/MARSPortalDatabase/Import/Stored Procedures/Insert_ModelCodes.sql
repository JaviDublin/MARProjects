-- =============================================
-- Author:		Javier
-- Create date: June 2012
-- Description:	
-- =============================================
CREATE PROCEDURE [Import].[Insert_ModelCodes]
@Type varchar(20)
	
AS
BEGIN
	
	SET NOCOUNT ON;
if @Type = 'Daily'
begin	
	INSERT INTO MODELCODES (COUNTRY, MODEL, ACTIVE)
	(
		SELECT DISTINCT 
			FEA.COUNTRY, MODEL , 1
		FROM 
			FLEET_EUROPE_ACTUAL FEA
		INNER JOIN 
			COUNTRIES C ON FEA.COUNTRY = C.COUNTRY
		WHERE 
			FEA.COUNTRY IS NOT NULL AND MODEL IS NOT NULL
	)
end
    
END