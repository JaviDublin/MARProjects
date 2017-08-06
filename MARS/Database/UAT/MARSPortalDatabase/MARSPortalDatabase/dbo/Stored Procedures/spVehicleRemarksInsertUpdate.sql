


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spVehicleRemarksInsertUpdate] 
	-- Add the parameters for the stored procedure here
	@serial VARCHAR(25),
	@remark VARCHAR(500) = NULL,
	@next_onrent_date DATETIME = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF ((SELECT SERIAL FROM VEHICLE_REMARKS WHERE SERIAL = @serial) IS NULL)
	BEGIN
		INSERT INTO VEHICLE_REMARKS(SERIAL, REMARK, EXPECTEDRESOLUTIONDATE)
		VALUES (@serial, @remark, @next_onrent_date)
	END
	ELSE
	BEGIN
		UPDATE VEHICLE_REMARKS SET REMARK = @remark, EXPECTEDRESOLUTIONDATE = @next_onrent_date WHERE SERIAL = @serial
	END
	
END