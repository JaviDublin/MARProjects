CREATE PROCEDURE [dbo].[proc_AdjustmentUpdate]
@carSegmentID INT, @carClassGroupID INT, @carclassID INT, @cmsPoolID INT, @locationGroupID INT, @adjustmentToUpdate INT, @adjustmentType INT, @isAddition BIT, @adjustmentValue NUMERIC (9, 2), @date DATETIME
AS EXTERNAL NAME [SqlServerProject1].[StoredProcedures].[proc_AdjustmentUpdate]


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFileLine', @value = N'309', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'proc_AdjustmentUpdate';


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFile', @value = N'Management.cs', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'proc_AdjustmentUpdate';

