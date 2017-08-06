-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CarGroupUpdate] 
(	
	@car_group_id int=NULL,
	@car_group varchar(3)=NULL, 
	@car_group_gold varchar(3)=NULL, 
	@car_group_Fivestar varchar(3)=NULL, 
	@car_group_PresidentCircle varchar(3)=NULL, 
	@car_group_Platinum varchar(3)=NULL, 
	@car_class_id int=NULL,
	@sort_car_group int=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CAR_GROUPS SET  
	car_group = @car_group, 
	car_group_gold = @car_group_gold, 
	car_group_fivestar = @car_group_Fivestar,
	car_group_presidentCircle = @car_group_PresidentCircle,
	car_group_platinum = @car_group_Platinum,
	car_class_id = @car_class_id, 
	sort_car_group = @sort_car_group

	WHERE car_group_id = @car_group_id

END