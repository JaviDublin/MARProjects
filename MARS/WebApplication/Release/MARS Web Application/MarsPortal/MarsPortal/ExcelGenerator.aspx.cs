using System;
using System.Text;
using App.BLL.ExcelExport;
using App.BLL.Utilities;

namespace App
{
    public partial class ExcelGenerator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        
        {

            if (Session["ExportData"] == null) return;

            var fileType = Session["ExportFileType"] != null ? Session["ExportFileType"].ToString() : "csv";

            Session["ExportFileType"] = null;

            var excelData = Session["ExportData"].ToString();
            Session["ExportData"] = null;
            var fileName = Session["ExportFileName"].ToString();


            Response.Clear();
            Response.AddHeader("content-disposition",
            string.Format("attachment;filename={0}.{1}", fileName, fileType));
            Response.Charset = "";
            Response.ContentType = "application/text";

            Response.Write(excelData + "\n");
            Response.Flush();
            Response.End();
        }

        
    }
}