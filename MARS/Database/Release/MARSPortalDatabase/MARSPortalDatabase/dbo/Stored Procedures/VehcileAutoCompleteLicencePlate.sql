CREATE PROC [dbo].[VehcileAutoCompleteLicencePlate] 
    @SearchTerm	NVARCHAR(200)=NULL
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

		SELECT distinct TOP 10
		--ID
		v.LicensePlate	AS 'ItemId'
		--Value To Type
		, v.LicensePlate  AS 'ItemValue'
		--Display
		, v.LicensePlate
		AS 'ItemLabel'
		FROM dbo.Vehicle v
		WHERE (
				v.LicensePlate LIKE '%' + dbo.splitstringcomma(@SearchTerm, 0) + '%' 
				)
				and 
				(
					dbo.splitstringcomma(@SearchTerm, 1) = 'all'
					or (v.IsFleet = 1 and v.IsNonRev = 1)
				)


	COMMIT