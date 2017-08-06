-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Import].[Insert_NonRevFile]
	
	@rowText VARCHAR(4000)
AS
BEGIN
	
	SET NOCOUNT ON;

   INSERT INTO [General].[Import_NonRevFile]
   VALUES (@rowText)
   
   
END