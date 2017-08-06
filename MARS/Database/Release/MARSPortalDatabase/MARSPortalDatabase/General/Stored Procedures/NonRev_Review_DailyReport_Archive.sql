
-- moves to Fact_NonRevLog_DailyReport_History all rows older then 10 days
CREATE PROCEDURE [General].[NonRev_Review_DailyReport_Archive] 
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CutOffDate DATETIME

	SET @CutOffDate = (GetDate() - 11)
	
	BEGIN TRY
		BEGIN TRANSACTION

		INSERT INTO [General].[Fact_NonRevLog_DailyReport_History]
		(	Rep_Date, dim_calendar_id, Rep_Year, Rep_Month, Rep_WeekOfYear, Rep_DayOfWeek,
			CountryCar, CountryLoc, LocationCode, LocationId, CarGroup, CarGroupId, OperStat,
			DayGroupCode, TotalFleet, TotalFleet_CarSales, TotalFleet_RacOps, TotalFleet_RacTtl,
			TotalFleet_Hod, TotalFleet_Adv, TotalFleet_Licensee, OperationalFleet, 
			OperationalFleet_CarSales, OperationalFleet_RacOps, OperationalFleet_RacTtl, 
			OperationalFleet_Hod, OperationalFleet_Adv, OperationalFleet_Licensee, TotalFleetNR, 
			TotalFleetNR_CarSales, TotalFleetNR_RacOps, TotalFleetNR_RacTtl, TotalFleetNR_Hod, 
			TotalFleetNR_Adv, TotalFleetNR_Licensee, TotalFleetNR_Remark, Cms_Group_Id, Cms_Group, 
			Cms_Pool_Id, Cms_Pool, Ops_Area_Id, Ops_Area, Ops_Region_Id, Ops_Region, Car_Segment_Id, 
			Car_Segment, Car_Class_Id, Car_Class, rns
		)
		SELECT 
			Rep_Date, dim_calendar_id, Rep_Year, Rep_Month, Rep_WeekOfYear, Rep_DayOfWeek, CountryCar,
			CountryLoc, LocationCode, LocationId, CarGroup, CarGroupId, OperStat, DayGroupCode, TotalFleet, 
			TotalFleet_CarSales, TotalFleet_RacOps, TotalFleet_RacTtl, TotalFleet_Hod, TotalFleet_Adv, 
			TotalFleet_Licensee, OperationalFleet, OperationalFleet_CarSales, OperationalFleet_RacOps, 
			OperationalFleet_RacTtl, OperationalFleet_Hod, OperationalFleet_Adv, OperationalFleet_Licensee, 
			TotalFleetNR, TotalFleetNR_CarSales, TotalFleetNR_RacOps, TotalFleetNR_RacTtl, TotalFleetNR_Hod, 
			TotalFleetNR_Adv, TotalFleetNR_Licensee, TotalFleetNR_Remark, Cms_Group_Id, Cms_Group, Cms_Pool_Id, 
			Cms_Pool, Ops_Area_Id, Ops_Area, Ops_Region_Id, Ops_Region, Car_Segment_Id, Car_Segment, Car_Class_Id, 
			Car_Class, rns
		FROM [General].[Fact_NonRevLog_DailyReport]
		WHERE Rep_Date < @CutOffDate

		DELETE
		FROM General.Fact_NonRevLog_DailyReport
		WHERE Rep_Date < @CutOffDate	

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
	END CATCH
END