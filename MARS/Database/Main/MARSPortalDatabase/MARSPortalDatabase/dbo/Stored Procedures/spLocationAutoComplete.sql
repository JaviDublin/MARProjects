-- =============================================
-- Author:		Damien Connaghan
-- Create date: 30.09.2014
-- Description:	AutoComplete for Locations
-- =============================================
CREATE PROCEDURE  [dbo].[spLocationAutoComplete] @SearchTerm  VARCHAR(200)=NULL,
                                       @ExtraValue1 VARCHAR(50)=NULL,
                                       @ExtraValue2 VARCHAR(50)=NULL
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

      SELECT TOP 5   [location]                                               AS 'ItemId',
                   location_name                                                     AS 'ItemValue',
                   '<strong>Location :</strong> '
                   + COALESCE('<br/><strong>Id:</strong> ' + [location], '')
                   + COALESCE('<br/><strong>Name:</strong> ' + location_name, '') AS 'ItemLabel'
      FROM   [dbo].LOCATIONS WITH (NOLOCK)
      WHERE  location_name LIKE '%' + @searchTerm + '%'
             AND [cms_location_group_id] = @ExtraValue1 
  END