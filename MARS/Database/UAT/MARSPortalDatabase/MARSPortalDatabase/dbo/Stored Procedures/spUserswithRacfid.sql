-- =============================================
-- Author:		Damien Connaghan
-- Create date: 02.10.2014
-- Description:	AutoComplete for Users who match racfid 
-- =============================================
CREATE PROCEDURE [dbo].[spUserswithRacfid] @SearchTerm  VARCHAR(200)=NULL,
                                           @ExtraValue1 VARCHAR(50)=NULL,
                                           @ExtraValue2 VARCHAR(50)=NULL
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

      SELECT TOP 5 [racfId]                                                AS 'ItemId',
                   [racfId]                                                  AS 'ItemValue',
                   '<strong>Location :</strong> '
                   + COALESCE('<br/><strong>Id:</strong> ' + [racfId], '')
                   + COALESCE('<br/><strong>Name:</strong> ' + [name], '') AS 'ItemLabel'
      FROM   [dbo].[MARS_Users] WITH (NOLOCK)
      WHERE  [racfId] LIKE '%' + @searchTerm + '%'
  END