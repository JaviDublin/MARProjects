using App.BLL;

namespace App.AvailabilityTool.KPIDownload
{
    public partial class DownloadFile : System.Web.UI.Page
    {
        #region "Page Events"
        
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                string filePath = SessionHandler.AvailabilityKPIDownloadFileName;

                if (!(filePath == null))
                {
                    // retrieve the physical path of the file to download, and create
                    // a FileInfo object to read its properties
                    //Dim FilePath As String = Server.MapPath(virtualPath)
                    System.IO.FileInfo TargetFile = new System.IO.FileInfo(filePath);

                    // clear the current output content from the buffer
                    Response.Clear();
                    // add the header that specifies the default filename for the Download/
                    // SaveAs dialog
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + TargetFile.Name);
                    // add the header that specifies the file size, so that the browser
                    // can show the download progress
                    Response.AddHeader("Content-Length", TargetFile.Length.ToString());
                    // specify that the response is a stream that cannot be read by the
                    // client and must be downloaded
                    Response.ContentType = "application/octet-stream";
                    // send the file stream to the client
                    Response.WriteFile(TargetFile.FullName);
                }

            }
            catch
            {

            }
            finally
            {
                // stop the execution of this page
                Response.End();
            }

        }
        #endregion
    }
}