-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_UserUpdateRoles] 
	@racfId VARCHAR(10)=NULL,
	@roleId	INT =NULL,		
	@country VARCHAR(50)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	INSERT INTO dbo.MARS_UsersInRoles
	(userId, roleId, country) 
	VALUES
	(@racfId, @roleId, @country)
	

END