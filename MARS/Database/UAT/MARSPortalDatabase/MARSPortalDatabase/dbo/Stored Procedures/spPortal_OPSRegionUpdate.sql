-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_OPSRegionUpdate] 
	
	@ops_region_id int,
	@ops_region varchar(50), 
	@country varchar(10)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE OPS_Regions SET  ops_region = @ops_region, country = @country

	WHERE ops_region_id = @ops_region_id

END