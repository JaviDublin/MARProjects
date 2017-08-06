CREATE procedure [dbo].[proc_MarsLog_AddCategory]
	-- Add the parameters for the function here
	@CategoryName nvarchar(64),
	@LogID int
AS
BEGIN
	SET NOCOUNT ON;
    DECLARE @CatID INT
	SELECT @CatID = CategoryID FROM [Mars_Logging_Category] WHERE CategoryName = @CategoryName
	IF @CatID IS NULL
	BEGIN
		INSERT INTO [Mars_Logging_Category] (CategoryName) VALUES(@CategoryName)
		SELECT @CatID = @@IDENTITY
	END

	EXEC proc_MarsLog_InsertCategoryLog @CatID, @LogID 

	RETURN @CatID
END