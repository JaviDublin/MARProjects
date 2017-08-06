CREATE procedure [dbo].[proc_FleetPlanMovementUpdate]

		@fleetPlanDetailID		INT			,
		@fleetPlanEntryID		INT			, 
		@targetDate				DATETIME	,
		@car_class				VARCHAR(3)	,
		@cms_Location_Group_ID	INT			,
		@addition				SMALLINT	,
		@deletion				SMALLINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

declare @tmp table(fleetPlanId int,targetDate datetime,locGrp int,carGrp int,addition int,deletion int)
insert into @tmp
(fleetPlanId,targetDate,locGrp,carGrp,addition,deletion)
values
(@fleetPlanEntryID,@targetDate,@cms_Location_Group_ID,@car_class,@addition,@deletion)
merge [dbo].[MARS_CMS_FleetPlanDetails] as t
using @tmp as s
on (t.[fleetPlanEntryID]=s.fleetPlanId and t.[targetDate]=s.targetDate
	 and t.[cms_Location_Group_ID]=s.locGrp and t.[car_class_id]=s.carGrp)
when not matched by target
	then insert ([fleetPlanEntryID],[targetDate],[cms_Location_Group_ID],[car_class_id],[addition],[deletion])
     VALUES (s.fleetPlanId,s.targetDate,s.locGrp,s.carGrp,s.addition,s.deletion)
when matched
	then update set t.addition=s.addition,t.deletion=s.deletion
	;
	
	exec [dbo].[FleetSizeForecastUpdate] 
	@fleetPlanEntryID 
	,@targetDate 
	,@cms_Location_Group_ID
	,@car_class
	,@addition
	,@deletion
END