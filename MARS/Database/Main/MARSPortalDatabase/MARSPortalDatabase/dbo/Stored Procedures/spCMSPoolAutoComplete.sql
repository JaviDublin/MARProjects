-- =============================================
-- Author:		Damien Connaghan
-- Create date: 25.09.2014
-- Description:	Autocomplete Textbox SPROC for CMS Pool
-- =============================================
CREATE PROCEDURE [dbo].[spCMSPoolAutoComplete] @SearchTerm  Varchar(200)=NULL,
                                     @ExtraValue1 Varchar(50)=NULL,
                                     @ExtraValue2 Varchar(50)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT TOP 5 [cms_pool_id]                                           AS 'ItemId',
                  [cms_pool]                                               AS 'ItemValue',
                   '<strong>CMS Pool :</strong> '              
                   + COALESCE('<br/><strong>Id:</strong> ' + CONVERT(Varchar(8), [cms_pool_id]), '')
                   + COALESCE('<br/><strong>Name:</strong> ' + [cms_pool], '')
                AS 'ItemLabel'
      FROM   [dbo].[CMS_POOLS] WITH (NOLOCK)
      WHERE  [cms_pool] LIKE '%' + @searchTerm + '%'
              AND [country] LIKE '%' + @ExtraValue1 + '%'       
END