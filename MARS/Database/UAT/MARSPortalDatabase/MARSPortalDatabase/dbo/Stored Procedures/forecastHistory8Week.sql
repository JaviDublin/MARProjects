
-- =============================================
-- Author:		Gavin Williams
-- Create date: 23 March 12
-- Description:	loops through stored procedure for 8 weeks
-- =============================================
CREATE PROCEDURE [dbo].[forecastHistory8Week]
		@theDay datetime
AS
BEGIN
	
	SET NOCOUNT ON;

    DECLARE @count int, @current_count int, @current_date datetime
	declare @c varchar(50), @u varchar(50)
	
	set @count = 8			-- 8 weeks
	set @current_count = 0
	set @current_date = @theDay	
	
	WHILE(@current_count <= @count)
	BEGIN
		if @current_count = 0 
			begin
				set @c = 'cms_constrained'
				set @u = 'cms_unconstrained'
			end
		else
			begin
				set @c = 'cms_constrained_wk' + cast (@current_count as varchar)
				set @u = 'cms_unconstrained_wk' + cast (@current_count as varchar)
			end				
	
		EXEC [dbo].[forecastHistory_V1_0] @current_date, @c, @u
		
		SET @current_count = @current_count + 1	
		SET @current_date = DATEADD(d, 7, @current_date)
	END
END