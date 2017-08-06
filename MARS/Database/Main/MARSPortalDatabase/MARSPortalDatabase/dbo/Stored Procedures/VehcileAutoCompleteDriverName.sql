CREATE PROC [dbo].[VehcileAutoCompleteDriverName] 
    @SearchTerm	NVARCHAR(200)=NULL
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

		SELECT distinct TOP 10
		--ID
		v.LastDriverName	AS 'ItemId'
		--Value To Type
		, v.LastDriverName  AS 'ItemValue'
		--Display
		, v.LastDriverName
		AS 'ItemLabel'
		FROM dbo.Vehicle v
		WHERE (
				v.LastDriverName LIKE '%' + dbo.splitstringcomma(@SearchTerm, 0) + '%' 
				)
				and 
				(
					dbo.splitstringcomma(@SearchTerm, 1) = 'all'
					or (v.IsFleet = 1 and v.IsNonRev = 1)
				)

	COMMIT