-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_OPSRegionInsert] 
(	
	@ops_region varchar(50)=NULL, 
	@country varchar(10)=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO OPS_Regions 
	(ops_region, country)
	VALUES( @ops_region, @country )
	
	-- Return Success
	SELECT 0

END