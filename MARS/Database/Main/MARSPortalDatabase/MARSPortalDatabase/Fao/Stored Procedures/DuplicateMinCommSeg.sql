
CREATE PROCEDURE [Fao].[DuplicateMinCommSeg]
(@newScenarioId as int, @oldScenarioId as int)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO fao.MinCommercialSegment(MinCommercialSegmentScenarioId, LocationId, CarSegmentId, CommercialCarSegmentId, Percentage, UpdatedBy, UpdatedOn )
	select @newScenarioId, LocationId, CarSegmentId, CommercialCarSegmentId, Percentage, UpdatedBy, UpdatedOn
	from fao.MinCommercialSegment
	where MinCommercialSegmentScenarioId = @oldScenarioId
END