-- =============================================
-- Author:		Damien Connaghan
-- Create date: 26.09.2014
-- Description:	AutoComplete for Car Classes
-- =============================================
CREATE PROCEDURE [dbo].[spCarClassesAutoComplete] @SearchTerm  VARCHAR(200)=NULL,
                                       @ExtraValue1 VARCHAR(50)=NULL,
                                       @ExtraValue2 VARCHAR(50)=NULL
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

      SELECT TOP 5 car_class_id                                                AS 'ItemId',
                   car_class                                                 AS 'ItemValue',
                   '<strong>Car Class:</strong> '
                   + COALESCE('<br/><strong>Id:</strong> ' + CONVERT(VARCHAR(8), [car_segment_id]), '')
                   + COALESCE('<br/><strong>Name:</strong> ' + car_class, '') AS 'ItemLabel'
      FROM   [dbo].CAR_CLASSES WITH (NOLOCK)
      WHERE  car_class LIKE '%' + @searchTerm + '%'
             AND car_segment_id LIKE '%' + @ExtraValue1 + '%'
  END