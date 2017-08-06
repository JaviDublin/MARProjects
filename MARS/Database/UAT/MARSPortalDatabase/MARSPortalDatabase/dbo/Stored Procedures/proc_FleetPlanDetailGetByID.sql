CREATE procedure [dbo].[proc_FleetPlanDetailGetByID]
	@fleetPlanDetailID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

			SELECT PD.PKID AS fleetPlanDetailID, PD.fleetPlanEntryID, 
			PD.targetDate, CG.car_group, PD.car_class_id,
            SLG.cms_location_group_id AS locationGroupID, 
            SLG.CMS_LOCATION_GROUP AS locationGroupName, 
	        PD.addition, PD.deletion, PD.amount
	        FROM mars_cms_fleetplandetails PD 
	        INNER JOIN CMS_Fleet_Plans FP ON FP.planID = PD.fleetPlanEntryID
	        INNER JOIN CAR_GROUPS CG ON CG.car_group_id = PD.car_class_id
	        INNER JOIN CMS_LOCATION_GROUPS SLG ON SLG.CMS_LOCATION_GROUP_ID = PD.cms_Location_Group_ID
  WHERE PD.[PKID] = @fleetPlanDetailID
END