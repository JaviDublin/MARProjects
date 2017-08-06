-- =============================================
-- Author:		Javier
-- Create date: January 2013 
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Settings].[NonRev_Remark_Edit]
	
		@edittype varchar(10),
		@remark varchar(100),
		@remarkid int = null
AS
BEGIN
	
	SET NOCOUNT ON;
	
	
	
	IF @edittype = 'Insert'
	BEGIN
		DECLARE @counter INT
		SET @counter = (SELECT COUNT(*) FROM [Settings].[NonRev_Remarks_List] WHERE RemarkText = @remark)
		IF @counter = 0
		BEGIN
			INSERT INTO [Settings].[NonRev_Remarks_List] (RemarkText , isActive) VALUES (@remark , 1)	
		END
	END
	
	IF @edittype = 'Delete'
	BEGIN
		DECLARE @id INT
		
		SET @id = (SELECT RemarkId FROM [Settings].[NonRev_Remarks_List] WHERE RemarkText = @remark)
		
		UPDATE [General].Fact_NonRevLog SET RemarkId = 1 WHERE RemarkId = @id
		
		DELETE FROM [Settings].[NonRev_Remarks_List] WHERE RemarkId = @id
			
	END
	IF @edittype = 'Update'
	BEGIN
		UPDATE [Settings].[NonRev_Remarks_List] SET RemarkText = @remark WHERE RemarkId = @remarkid
	END

  
  
  
END