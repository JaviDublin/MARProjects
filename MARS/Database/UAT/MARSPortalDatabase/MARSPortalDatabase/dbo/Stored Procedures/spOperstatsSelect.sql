CREATE procedure [dbo].[spOperstatsSelect] 
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT operstat_name, operstat_desc + ' (' + operstat_name + ')' as operstat_desc
	 FROM OPERSTATS ORDER BY sort_operstats
END