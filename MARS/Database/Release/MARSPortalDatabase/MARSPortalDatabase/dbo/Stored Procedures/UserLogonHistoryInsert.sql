CREATE PROC [dbo].[UserLogonHistoryInsert] 
    @UserName varchar(100),
    @UserId varchar(100),
    @TimeStamp datetime
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN
	
	INSERT INTO [dbo].[UserLogonHistory] ([UserName], [UserId], [TimeStamp])
	SELECT @UserName, @UserId, @TimeStamp
	
	/*-- Begin Return Select <- do not remove
	SELECT [UserLogonHistoryId], [UserName], [UserId], [TimeStamp]
	FROM   [dbo].[UserLogonHistory]
	WHERE  [UserLogonHistoryId] = SCOPE_IDENTITY()
	-- End Return Select <- do not remove*/
               
	COMMIT