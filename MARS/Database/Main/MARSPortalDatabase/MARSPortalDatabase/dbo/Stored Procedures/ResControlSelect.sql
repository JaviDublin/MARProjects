CREATE PROCEDURE [dbo].[ResControlSelect]
	-- Add the parameters for the stored procedure here
	@ConditionId int output

AS
BEGIN

	SET NOCOUNT ON; 
	
	select top 1 @ConditionId=ConditionId
	from dbo.ResControl
	order by Id desc

END