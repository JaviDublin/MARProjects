using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;

namespace Mars.App.UserControls.Phase4.NonRev
{
    public partial class ReasonHistoryGrid : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ibExportToExcel.Visible = gvOverview.Rows.Count != 0;
        }

        public void LoadGrid(List<ComparisonRow> comparisonData)
        {
            gvOverview.DataSource = comparisonData;
            gvOverview.DataBind();
        }

        protected void ibExportToExcel_Click(object sender, EventArgs e)
        {
            var dataTable = HtmlTableGenerator.GenerateCsvFromGridview(gvOverview);
            Session["ExportData"] = dataTable;
            Session["ExportFileType"] = "csv";
            Session["ExportFileName"] = string.Format("Reason History Export {0}", DateTime.Now.ToShortDateString());
        }
    }
}