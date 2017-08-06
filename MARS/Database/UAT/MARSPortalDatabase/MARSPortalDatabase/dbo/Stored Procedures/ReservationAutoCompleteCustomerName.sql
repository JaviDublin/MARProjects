CREATE PROC [dbo].[ReservationAutoCompleteCustomerName] 
    @SearchTerm	NVARCHAR(200)=NULL
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

		SELECT distinct TOP 10
		--ID
		r.ReservationId	AS 'ItemId'
		--Value To Type
		, r.CustomerName  AS 'ItemValue'
		--Display
		, r.CustomerName
		AS 'ItemLabel'
		FROM dbo.Reservation r
		WHERE (
				cast(r.CustomerName as varchar) LIKE '%' + @searchTerm + '%' 
				)

	COMMIT