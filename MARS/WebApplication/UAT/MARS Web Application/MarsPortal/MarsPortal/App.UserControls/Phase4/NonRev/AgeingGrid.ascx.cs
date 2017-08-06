using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;

namespace Mars.App.UserControls.Phase4.NonRev
{
    public partial class AgeingGrid : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ucExportToExcel.Visible = gvAgeing.Rows.Count != 0;
        }

        public void LoadGrid(List<AgeingRow> comparisonData)
        {
            gvAgeing.DataSource = comparisonData;
            gvAgeing.DataBind();
            if (gvAgeing.Rows.Count <= 0) return;
            gvAgeing.Rows[gvAgeing.Rows.Count - 1].Font.Bold = true;
            gvAgeing.Rows[gvAgeing.Rows.Count - 1].Font.Size = FontUnit.Point(10);
        }

        protected override bool OnBubbleEvent(object sender, EventArgs args)
        {
            var handled = false;
            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if (commandArgs.CommandName == HelpIcons.ExportToExcel.ExportString)
                {
                    ExportToExcel();
                }
                handled = true;
            }
            return handled;
        }


        protected void ExportToExcel()
        {
            var dataTable = HtmlTableGenerator.GenerateCsvFromGridview(gvAgeing);
            Session["ExportData"] = dataTable;
            Session["ExportFileType"] = "csv";
            Session["ExportFileName"] = string.Format("Ageing Export {0}", DateTime.Now.ToShortDateString());
        }
    }
}