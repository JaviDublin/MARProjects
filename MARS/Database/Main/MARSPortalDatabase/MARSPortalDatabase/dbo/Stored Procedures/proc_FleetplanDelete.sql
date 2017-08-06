CREATE PROCEDURE [dbo].[proc_FleetplanDelete] 

	@fleetPlanDetailID INT

AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @targetDate  DateTime
	DECLARE @cms_Location_Group_ID int, @car_class int 
	DECLARE @fleetPlanEntryID int
	
	SELECT top 1 
	@targetDate=targetDate
	,@cms_Location_Group_ID = FD.cms_Location_Group_ID
	,@car_class=car_class_id
	,@fleetPlanEntryID = FD.fleetPlanEntryID
	FROM 
		[dbo].MARS_CMS_FleetPlanDetails FD
	WHERE 
		PKID = @fleetPlanDetailID
	
	DELETE FROM [dbo].mars_cms_fleetplandetails
	WHERE [dbo].mars_cms_fleetplandetails.PKID = @fleetPlanDetailID	
	
	exec [dbo].[FleetSizeForecastUpdate] 
	@fleetPlanEntryID 
	,@targetDate 
	,@cms_Location_Group_ID
	,@car_class
	,0
	,0
END