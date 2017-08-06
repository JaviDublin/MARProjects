-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [bcs].[s_FileList] 
	
AS
BEGIN
	
	SET NOCOUNT ON;

    SELECT 
		FileId , FileMask , FilePath , FilePathTemp , FilePathArchive , BatchType , IsActive , HasDate , BatchTime
	FROM 
		[bcs].FileList


END