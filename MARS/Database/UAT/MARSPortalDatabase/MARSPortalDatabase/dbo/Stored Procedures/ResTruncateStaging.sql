﻿
CREATE PROCEDURE dbo.ResTruncateStaging

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    truncate table dbo.ReservationStaging
END