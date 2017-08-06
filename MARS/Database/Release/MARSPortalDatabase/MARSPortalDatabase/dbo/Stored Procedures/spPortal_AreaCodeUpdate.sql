-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_AreaCodeUpdate] 
(
	@ownarea varchar(5)=NULL,		
	@country varchar(10)=NULL, 
	@area_name varchar(50)=NULL, 
	@opco bit=NULL, 
	@fleetco bit=NULL, 
	@carsales bit=NULL, 
	@licensee bit=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE AreaCodes SET 
	country =  @country, 
	area_name = @area_name, 
	opco =  @opco, 
	fleetco =  @fleetco, 
	carsales = @carsales, 
	licensee = @licensee
	WHERE ownarea = @ownarea
	

END