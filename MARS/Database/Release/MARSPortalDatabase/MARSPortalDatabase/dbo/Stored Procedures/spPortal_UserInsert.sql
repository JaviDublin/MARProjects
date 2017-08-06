-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_UserInsert] 
	@racfId varchar(10),		
	@name varchar(50)
	--,@result int output

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF NOT EXISTS(select * from MARS_Users where racfId = @racfId)	
		BEGIN
			INSERT INTO MARS_Users VALUES( @racfId, @name)
	--		RETURN 1
		END
	--ELSE
	--	RETURN -1

END