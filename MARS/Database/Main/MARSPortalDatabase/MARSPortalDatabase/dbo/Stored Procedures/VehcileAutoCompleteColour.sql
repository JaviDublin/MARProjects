CREATE PROC [dbo].[VehcileAutoCompleteColour] 
    @SearchTerm	NVARCHAR(200)=NULL
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

		SELECT distinct TOP 10
		--ID
		v.Colour	AS 'ItemId'
		--Value To Type
		, v.Colour  AS 'ItemValue'
		--Display
		, v.Colour
		AS 'ItemLabel'
		FROM dbo.Vehicle v
		WHERE (
				cast(v.Colour as varchar) LIKE '%' + @searchTerm + '%' 
				)

	COMMIT