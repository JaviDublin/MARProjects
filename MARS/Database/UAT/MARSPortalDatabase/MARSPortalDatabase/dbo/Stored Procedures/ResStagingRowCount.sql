create PROCEDURE [dbo].[ResStagingRowCount]
	@RowCount int output
AS
BEGIN

	SET NOCOUNT ON;

    select @RowCount=COUNT(*) from dbo.ReservationStaging
    
END