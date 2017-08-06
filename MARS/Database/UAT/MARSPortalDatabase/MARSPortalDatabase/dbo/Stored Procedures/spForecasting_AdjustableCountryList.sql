-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spForecasting_AdjustableCountryList] 

	@racfId varchar(10),
	@isReconciliation bit
		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	-- select all countries where the current user has forecasting access to
	IF @isReconciliation = 0
	BEGIN
		select distinct country 
		from MARS_USERSINROLES 
		where userId = @racfId and roleId in (2,3,4) 
		order by country
	END
	ELSE IF @isReconciliation = 1
	BEGIN
		select distinct country 
		from MARS_USERSINROLES 
		where userId = @racfId and roleId in (2) 
		order by country
	END
END