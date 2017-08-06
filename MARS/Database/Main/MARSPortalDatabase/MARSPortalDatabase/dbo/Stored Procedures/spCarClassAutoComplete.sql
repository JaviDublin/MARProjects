-- =============================================
-- Author:		Damien Connaghan
-- Create date: 26.09.2014
-- Description:	AutoComplete for Car Group
-- =============================================
CREATE PROCEDURE [dbo].[spCarClassAutoComplete] @SearchTerm  VARCHAR(200)=NULL,
                                       @ExtraValue1 VARCHAR(50)=NULL,
                                       @ExtraValue2 VARCHAR(50)=NULL
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

      SELECT TOP 5 car_group_id                                                AS 'ItemId',
                   car_group                                                 AS 'ItemValue',
                   '<strong>Car Group:</strong> '
                   + COALESCE('<br/><strong>Id:</strong> ' + CONVERT(VARCHAR(8), car_group_id), '')
                   + COALESCE('<br/><strong>Name:</strong> ' + car_group, '') AS 'ItemLabel'
      FROM   [dbo].CAR_GROUPS WITH (NOLOCK)
      WHERE  car_group LIKE '%' + @searchTerm + '%'
             AND car_class_id LIKE '%' + @ExtraValue1 + '%'
  END