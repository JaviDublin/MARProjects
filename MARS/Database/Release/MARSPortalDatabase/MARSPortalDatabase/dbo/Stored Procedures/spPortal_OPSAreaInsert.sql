-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_OPSAreaInsert] 
(	
	@ops_area varchar(50)=NULL, 
	@ops_region_id int=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO OPS_AREAS 
	(ops_area, ops_region_id)
	VALUES( @ops_area, @ops_region_id )
	
	-- Return Success
	SELECT 0

END