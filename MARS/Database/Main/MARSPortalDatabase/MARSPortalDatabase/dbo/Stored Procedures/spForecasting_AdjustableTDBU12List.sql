-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spForecasting_AdjustableTDBU12List]

	@racfId varchar(10), 
	@country varchar(10)
	
		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select roleId

	from MARS_USERSINROLES 

	where userId = @racfId and country = @country and roleId in (2,3,4) 

	order by roleId


END