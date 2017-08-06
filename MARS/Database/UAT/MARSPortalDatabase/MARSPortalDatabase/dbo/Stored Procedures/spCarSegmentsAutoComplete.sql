-- =============================================
-- Author:		Damien Connaghan
-- Create date: 26.09.2014
-- Description:	AutoComplete for Car Segments
-- =============================================
CREATE PROCEDURE  [dbo].[spCarSegmentsAutoComplete] @SearchTerm  VARCHAR(200)=NULL,
                                       @ExtraValue1 VARCHAR(50)=NULL,
                                       @ExtraValue2 VARCHAR(50)=NULL
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

      SELECT TOP 5 [car_segment_id]                                                AS 'ItemId',
                   [car_segment]                                                 AS 'ItemValue',
                   '<strong>Car Segment :</strong> '
                   + COALESCE('<br/><strong>Id:</strong> ' + CONVERT(VARCHAR(8), [car_segment_id]), '')
                   + COALESCE('<br/><strong>Name:</strong> ' + [car_segment], '') AS 'ItemLabel'
      FROM   [dbo].CAR_SEGMENTS WITH (NOLOCK)
      WHERE  [car_segment] LIKE '%' + @searchTerm + '%'
             AND [country] LIKE '%' + @ExtraValue1 + '%'
  END