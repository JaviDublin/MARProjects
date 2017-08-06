-- =============================================
-- Author:		Gavin Williams
-- Create date: 21-8-2012
-- Description:	Temp file export of the Forecast and Forecast adjustment for Germany
-- =============================================

create PROCEDURE [Export].[GEForecast]

AS
BEGIN

	SET NOCOUNT ON;

/****** Script for SelectTopNRows command from SSMS  ******/
SELECT [Repdate]
      ,[Country]
      ,[CmsLocationGroupId]
      ,[CmsLocationGroup]
      ,[CmsPool]
      ,[CarClassId]
      ,[CarClass]
      ,[Constrained]
      ,[Unconstrained]
      ,[FleetForecast]
      ,[ReservationsBooked]
      ,[CurrentOnRent]
      ,[OnRentLy]
      ,[AvailableFleet]
      ,[AdjustmentBu1]
      ,[AdjustmentBu2]
      ,[AdjustmentRc]
      ,[AdjustmentTd]
      ,[TotalFleet]
      ,[Additions]
      ,[Deletions]
      ,[Amount]
      ,[NonRev7]
      ,[OnRentMax]
      ,[OnRentAvg]
      ,[OnRent]
      ,[Maintenance]
      ,[MaintenanceMax]
      ,[Unavailable]
      ,[UnavailableMax]
      ,[ServiceUnits]
      ,[ServiceUnitsMax]
      ,[Overdue]
      ,[OverdueMax]
      ,[CarSales]
      ,[CarSalesMax]
      ,[Turnback]
      ,[TurnbackMax]
      ,[RtMax]
      ,[Rt]
      ,[OpFleet]
      ,[OpFleetMax]
      ,[OpFleetCalc]
  FROM [Export].[GermanyForecast]
  
  end