create PROC [dbo].[AutoCompleteLocationWwdCode] 
    @SearchTerm	NVARCHAR(200)=NULL
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

		SELECT distinct TOP 10
		--ID
		l.dim_Location_id	AS 'ItemId'
		--Value To Type
		, l.location  AS 'ItemValue'
		--Display
		, l.location + '(' + l.location_name + ')'
		AS 'ItemLabel'
		FROM dbo.LOCATIONS l
		WHERE (
				l.location LIKE @searchTerm + '%' 
				)

	COMMIT