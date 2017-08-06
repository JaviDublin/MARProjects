
-- =============================================
-- Author:		Damien Connaghan
-- Create date: 25.09.2014
-- Description:	 Auto complete for CMS Loc Groups in cms pool id
-- =============================================
CREATE PROCEDURE [dbo].[spCmsLocGroupAutoComplete] @SearchTerm  VARCHAR(200)=NULL,
                                                 @ExtraValue1 VARCHAR(50)=NULL,
                                                 @ExtraValue2 VARCHAR(50)=NULL
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

      SELECT TOP 5 cms_location_group_id                                                  AS 'ItemId',
                   cms_location_group                                                     AS 'ItemValue',
                   '<strong>CMS Loc Group :</strong> '
                   + COALESCE('<br/><strong>Id:</strong> ' + CONVERT(VARCHAR(8), cms_location_group_id), '')
                   + COALESCE('<br/><strong>Name:</strong> ' + cms_location_group, '') AS 'ItemLabel'
      FROM   [dbo].CMS_LOCATION_GROUPS WITH (NOLOCK)
      WHERE  cms_location_group LIKE '%' + @searchTerm + '%'
             AND CONVERT(VARCHAR(8), cms_pool_id) LIKE '%' + @ExtraValue1 + '%'
  END