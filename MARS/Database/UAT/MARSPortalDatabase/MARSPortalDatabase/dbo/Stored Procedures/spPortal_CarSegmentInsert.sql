-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CarSegmentInsert] 
(	
	@car_segment varchar(50)=NULL, 
	@country varchar(10)=NULL,
	@sort_car_segment int=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO CAR_SEGMENTS 
	(car_segment, country, sort_car_segment)
	VALUES( @car_segment, @country, @sort_car_segment )

	-- Return Success
	SELECT 0

END