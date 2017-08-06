CREATE PROC [dbo].[VehcileAutoCompleteVin] 
    @SearchTerm	NVARCHAR(200)=NULL
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

		

		SELECT distinct TOP 10
		--ID
		v.Vin	AS 'ItemId'
		--Value To Type
		, v.Vin  AS 'ItemValue'
		--Display
		, v.Vin
		AS 'ItemLabel'
		FROM dbo.Vehicle v
		WHERE (
				v.Vin LIKE '%' + dbo.splitstringcomma(@SearchTerm, 0) + '%' 
				)
				and 
				(
					dbo.splitstringcomma(@SearchTerm, 1) = 'all'
					or (v.IsFleet = 1 and v.IsNonRev = 1)
				)

	COMMIT
	
	
--select dbo.splitstringcomma('this is text', 1)