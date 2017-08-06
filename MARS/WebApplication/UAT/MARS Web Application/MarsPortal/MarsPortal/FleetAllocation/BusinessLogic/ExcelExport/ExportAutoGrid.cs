using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.Phase4Bll;


namespace Mars.FleetAllocation.BusinessLogic.ExcelExport
{
    public static class ExportAutoGrid
    {
        public static void ExportToExcel(List<object> dataToExport, string fileName, string[] headerNames)
        {
            var dataTable = HtmlTableGenerator.GenerateHtmlTableFromList(dataToExport, headerNames);

            HttpContext.Current.Session["ExportData"] = dataTable;
            HttpContext.Current.Session["ExportFileType"] = "xls";
            HttpContext.Current.Session["ExportFileName"] = string.Format("{0} {1}",fileName, DateTime.Now.ToLongTimeString());
        }
    }
}