-- =============================================
-- Author:		Damien Connaghan
-- Create date: 26.09.2014
-- Description:	AutoComplete for Ops Region
-- =============================================
CREATE PROCEDURE  [dbo].[spOpsRegionAutoComplete] @SearchTerm  VARCHAR(200)=NULL,
                                       @ExtraValue1 VARCHAR(50)=NULL,
                                       @ExtraValue2 VARCHAR(50)=NULL
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

      SELECT TOP 5 ops_region_id                                                  AS 'ItemId',
                   ops_region                                                     AS 'ItemValue',
                   '<strong>OPS Area :</strong> '
                   + COALESCE('<br/><strong>Id:</strong> ' + CONVERT(VARCHAR(8), ops_region_id), '')
                   + COALESCE('<br/><strong>Name:</strong> ' + ops_region, '') AS 'ItemLabel'
      FROM   [dbo].OPS_REGIONS WITH (NOLOCK)
      WHERE  ops_region LIKE '%' + @searchTerm + '%'
             AND [country] LIKE '%' + @ExtraValue1 + '%'
  END