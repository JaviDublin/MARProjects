-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Import].[Truncate_NonRevFile]
	
AS
BEGIN

	SET NOCOUNT ON;

    TRUNCATE TABLE [General].[Import_NonRevFile]

END