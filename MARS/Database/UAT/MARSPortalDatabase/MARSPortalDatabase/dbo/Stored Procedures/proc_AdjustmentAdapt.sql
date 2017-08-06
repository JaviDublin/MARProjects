CREATE PROCEDURE [dbo].[proc_AdjustmentAdapt]
@carSegmentID INT, @carClassGroupID INT, @carclassID INT, @cmsPoolID INT, @locationGroupID INT, @forecastToUpdateFrom INT, @adjustmentToUpdate INT, @date DATETIME
AS EXTERNAL NAME [SqlServerProject1].[StoredProcedures].[proc_AdjustmentAdapt]


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFileLine', @value = N'463', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'proc_AdjustmentAdapt';


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFile', @value = N'Management.cs', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'proc_AdjustmentAdapt';

