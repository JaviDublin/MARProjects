-- =============================================
-- Author:		Gavin
-- Create date: 3/4/12
-- Description:	If the racfId is in the table returns noRow otherwise adds the adds the record 
-- =============================================
Create PROCEDURE [dbo].[spPortal_RolesUserCarSearch]
	@rac as Varchar(10),		-- The racfId (Primary key in table)
	@no as Numeric (18,0) = 500	-- The no of records the user is allowed to retrieve
								-- NB: If this is set to -1 then its is NOT inderted or updated
AS
BEGIN
	SET NOCOUNT ON;

	If Not Exists	-- Insert new user and number of rows
		(Select [racfId] ,[noRows]
			From [dbo].[Mars_UserCarSearch]
			Where [racfId]=@rac) and @no > -1
		Begin
			INSERT INTO [dbo].[Mars_UserCarSearch]
					([racfId], [noRows])
			VALUES  (  @rac,	 @no   )
		End
	Else If @no > -1
		Begin
			UPDATE [dbo].[Mars_UserCarSearch]
			SET [noRows] = @no
			WHERE [racfId]=@rac
		End
			
	  Select [noRows]	-- Return number of records that can be retrieved
			From [dbo].[Mars_UserCarSearch]
			Where [racfId]=@rac

END