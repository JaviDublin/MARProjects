
-- =============================================
-- Author:		Damien Connaghan
-- Create date: 25.09.2014
-- Description:	AutoComplete for Ops Area
-- =============================================
CREATE PROCEDURE [dbo].[spOpsAreaAutoComplete] @SearchTerm  VARCHAR(200)=NULL,
                                       @ExtraValue1 VARCHAR(50)=NULL,
                                       @ExtraValue2 VARCHAR(50)=NULL
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

      SELECT TOP 5 ops_area_id                                                  AS 'ItemId',
                   ops_area                                                     AS 'ItemValue',
                   '<strong>OPS Area :</strong> '
                   + COALESCE('<br/><strong>Id:</strong> ' + CONVERT(VARCHAR(8), ops_area_id), '')
                   + COALESCE('<br/><strong>Name:</strong> ' + ops_area, '') AS 'ItemLabel'
      FROM   [dbo].OPS_AREAS WITH (NOLOCK)
      WHERE  ops_area LIKE '%' + @searchTerm + '%'
             AND ops_region_id LIKE '%' + @ExtraValue1 + '%'
  END