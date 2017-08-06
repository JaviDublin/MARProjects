CREATE PROC [dbo].[VehcileAutoCompleteUnitNumber] 
    @SearchTerm	NVARCHAR(200)=NULL
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

		SELECT distinct TOP 10
		--ID
		v.UnitNumber	AS 'ItemId'
		--Value To Type
		, v.UnitNumber  AS 'ItemValue'
		--Display
		, v.UnitNumber
		AS 'ItemLabel'
		FROM dbo.Vehicle v
		WHERE (
				cast(v.UnitNumber as varchar) LIKE '%' + dbo.splitstringcomma(@SearchTerm, 0)  + '%' 
				)
				and 
				(
					dbo.splitstringcomma(@SearchTerm, 1) = 'all'
					or (v.IsFleet = 1 and v.IsNonRev = 1)
				)

	COMMIT