CREATE PROC [dbo].[VehcileAutoCompleteAreaCode] 
    @SearchTerm	NVARCHAR(200)=NULL
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

		SELECT distinct TOP 10
		--ID
		v.OwningArea	AS 'ItemId'
		--Value To Type
		, v.OwningArea  AS 'ItemValue'
		--Display
		, v.OwningArea + ' - ' + ac.area_name
		AS 'ItemLabel'
		FROM dbo.AREACODES ac
		join Vehicle v on ac.ownarea = v.OwningArea
		WHERE (
				ac.ownarea LIKE '%' + @searchTerm + '%' 
				)
				or
				ac.area_name like '%' + @searchTerm + '%' 

	COMMIT