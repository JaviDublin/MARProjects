CREATE PROCEDURE [dbo].[proc_GetAdjustment]
@country NVARCHAR (4000), @carSegmentID INT, @carClassGroupID INT, @carclassID INT, @cmsPoolID INT, @locationGroupID INT, @date DATETIME
AS EXTERNAL NAME [SqlServerProject1].[StoredProcedures].[proc_GetAdjustment]


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFileLine', @value = N'94', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'proc_GetAdjustment';


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFile', @value = N'Management.cs', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'proc_GetAdjustment';

