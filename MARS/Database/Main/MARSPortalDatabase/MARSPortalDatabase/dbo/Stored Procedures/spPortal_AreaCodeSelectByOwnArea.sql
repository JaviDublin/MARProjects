-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_AreaCodeSelectByOwnArea] 
(
	@ownarea			VARCHAR(50) =NULL
)		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT     ownarea, country, area_name, opco, fleetco, carsales, licensee
FROM         AREACODES
WHERE ownarea =@ownarea
	

END