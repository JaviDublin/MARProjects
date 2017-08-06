CREATE procedure [dbo].[proc_CMSFleetPlanGetAll]
		
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
		planID, planDescription 
	FROM 
		[dbo].[CMS_Fleet_Plans]
		
END