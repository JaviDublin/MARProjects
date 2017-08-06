
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spTrackStarFleetEuropeActual]
	
	 --@country varchar(2)=NULL,
	 @unit varchar(25)=NULL,
	 @license varchar(25)=NULL
AS
BEGIN

	select *
	from FLEET_EUROPE_ACTUAL
	where 
	unit = @unit and 
	license = @license
END