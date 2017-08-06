-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_AreaCodeInsert] 
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

	INSERT INTO AreaCodes
	(ownarea, country, area_name, opco, fleetco, carsales,licensee)
	VALUES
	( @ownarea, @country, @area_name, @opco, @fleetco, @carsales, @licensee)

	-- Return Success
	SELECT 0

END