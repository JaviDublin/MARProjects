CREATE PROCEDURE [dbo].[proc_AdjustmentReconcile]
@carSegmentID INT, @carClassGroupID INT, @carclassID INT, @cmsPoolID INT, @locationGroupID INT, @adjustmentToUpdateFrom INT, @date DATETIME
AS EXTERNAL NAME [SqlServerProject1].[StoredProcedures].[proc_AdjustmentReconcile]


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFileLine', @value = N'604', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'proc_AdjustmentReconcile';


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFile', @value = N'Management.cs', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'proc_AdjustmentReconcile';

