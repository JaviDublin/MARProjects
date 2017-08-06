-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CountryInsert] 
(
	@country varchar(10)=NULL,
	@country_dw varchar(50)=NULL,
	@country_description varchar(50)=NULL,
	@active bit
)		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO COUNTRIES 
	(country, country_dw, country_description, active)
	VALUES
	(@country, @country_dw, @country_description, @active)

	-- Return Success
	SELECT 0


END