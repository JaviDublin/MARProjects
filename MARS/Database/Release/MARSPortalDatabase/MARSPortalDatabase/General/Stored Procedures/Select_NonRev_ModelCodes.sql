-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Select_NonRev_ModelCodes]

	@country VARCHAR(10) = NULL

AS
BEGIN
	
	SET NOCOUNT ON;
	
	declare @models table (Country varchar(2) , Model varchar(10) , Total int)
	insert into @models
	select Country ,  Model , Count(distinct VehicleId)
	from [General].Dim_Fleet
	where IsActive = 1 and Country = @country
	group by Country ,  Model
	order by Count(distinct VehicleId) desc
	select Model as 'model' from @models where Total > 5

  
END