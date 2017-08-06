create PROCEDURE [dbo].[ResRmksImport]

AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO [dbo].[ResRemarks]
		  ([ResIdNbr],[SeqNbr],[ResRmkType],[Remark])
	select [ResIdNbr],[SeqNbr],[ResRmkType],[Remark] from ResRmkStaging
	where ResIdNbr in(select RES_ID_NBR from dbo.Reservations)

END