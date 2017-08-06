CREATE PROC [dbo].[ReservationAutoCompleteExternalId] 
    @SearchTerm	NVARCHAR(200)=NULL
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

		SELECT distinct TOP 10
		--ID
		r.ReservationId	AS 'ItemId'
		--Value To Type
		, r.ExternalId  AS 'ItemValue'
		--Display
		, r.ExternalId
		AS 'ItemLabel'
		FROM dbo.Reservation r
		WHERE (
				cast(r.ExternalId as varchar) LIKE @searchTerm + '%' 
				)

	COMMIT