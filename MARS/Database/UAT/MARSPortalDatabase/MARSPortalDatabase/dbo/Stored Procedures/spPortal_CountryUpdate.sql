-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CountryUpdate] 
(
	@country varchar(10)=NULL,
	@country_dw varchar(50)=NULL,
	@country_description varchar(50)=NULL,
	@active bit=NULL
)		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	    -- Insert statements for procedure here
	UPDATE Countries SET 
	country_dw = @country_dw, 
	country_description = @country_description, 
	active = @active

	WHERE country = @country

END