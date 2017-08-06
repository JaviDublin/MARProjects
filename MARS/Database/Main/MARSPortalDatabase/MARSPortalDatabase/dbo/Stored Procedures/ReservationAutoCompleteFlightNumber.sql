CREATE PROC [dbo].[ReservationAutoCompleteFlightNumber] 
    @SearchTerm	NVARCHAR(200)=NULL
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

		SELECT distinct TOP 10
		--ID
		r.ReservationId	AS 'ItemId'
		--Value To Type
		, r.FlightNumber  AS 'ItemValue'
		--Display
		, r.FlightNumber
		AS 'ItemLabel'
		FROM dbo.Reservation r
		WHERE (
				cast(r.FlightNumber as varchar) LIKE '%' + @searchTerm + '%' 
				)

	COMMIT