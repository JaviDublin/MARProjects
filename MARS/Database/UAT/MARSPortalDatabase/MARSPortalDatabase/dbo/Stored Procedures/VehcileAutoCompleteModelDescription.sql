CREATE PROC [dbo].[VehcileAutoCompleteModelDescription] 
    @SearchTerm	NVARCHAR(200)=NULL
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

		SELECT distinct TOP 10
		--ID
		v.ModelDescription	AS 'ItemId'
		--Value To Type
		, v.ModelDescription  AS 'ItemValue'
		--Display
		, v.ModelDescription
		AS 'ItemLabel'
		FROM dbo.Vehicle v
		WHERE (
				v.ModelDescription LIKE '%' + dbo.splitstringcomma(@SearchTerm, 0) + '%' 
				)
								and 
				(
					dbo.splitstringcomma(@SearchTerm, 1) = 'all'
					or (v.IsFleet = 1 and v.IsNonRev = 1)
				)

	COMMIT
	
	


  --exec [VehcileAutoCompleteModelDescription] 'Ford'