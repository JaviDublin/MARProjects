
CREATE PROCEDURE [Fao].[DuplicateMaxFleet]
(@newScenarioId as int, @oldScenarioId as int)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Fao].[MaxFleetFactor]( MaxFleetFactorScenarioId, LocationId, CarGroupId, DayOfWeekId, NonRevPercentage, UtilizationPercentage, UpdatedBy, UpdatedOn)
	select @newScenarioId, LocationId, CarGroupId, DayOfWeekId, NonRevPercentage, UtilizationPercentage, UpdatedBy, UpdatedOn
	from [Fao].[MaxFleetFactor]
	where MaxFleetFactorScenarioId = @oldScenarioId

END