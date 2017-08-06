CREATE PROCEDURE [dbo].[proc_GetAdjustmentCount]
@carSegmentID INT, @carClassGroupID INT, @carclassID INT, @cmsPoolID INT, @locationGroupID INT, @date DATETIME, @count INT OUTPUT
AS EXTERNAL NAME [SqlServerProject1].[StoredProcedures].[proc_GetAdjustmentCount]


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFileLine', @value = N'208', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'proc_GetAdjustmentCount';


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFile', @value = N'Management.cs', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'proc_GetAdjustmentCount';

