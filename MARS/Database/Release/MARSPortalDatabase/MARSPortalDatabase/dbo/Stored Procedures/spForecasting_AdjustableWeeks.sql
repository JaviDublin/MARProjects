-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spForecasting_AdjustableWeeks]

	@country varchar(10),
	@racfId varchar(10),
	@year int,
	@curWeek int,
	@eWeek int
		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @weeks table (wk int)	
	declare @index integer
	set @index = 1
	
	while @index <= @eWeek
	begin
		insert into @weeks values(@index)
		set @index = @index + 1
	end

	-- if country coordinator or next year, then return all weeks
	IF EXISTS(select * from MARS_USERSINROLES where userId = @racfId and country = @country and roleId = 2)
		OR @year > YEAR(getdate())
	BEGIN
		select * from @weeks
	END

	ELSE
		select * from @weeks where wk > @curWeek
	


END