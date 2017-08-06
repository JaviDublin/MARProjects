-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spRemarkListSelect]
	
AS
BEGIN
	
	SET NOCOUNT ON;

    DECLARE @remarks TABLE (RemarkId INT ,RemarkText VARCHAR(255))

	INSERT INTO @remarks
	SELECT 
		RemarkId , RemarkText 
	FROM 
		[Settings].NonRev_Remarks_List
	WHERE
		RemarkId = 1
	
	INSERT INTO @remarks
    SELECT 
		RemarkId , RemarkText 
	FROM 
		[Settings].NonRev_Remarks_List
	WHERE
		IsActive = 1 AND RemarkId > 1
	ORDER BY
		RemarkText
	SELECT RemarkId , RemarkText FROM @remarks
END