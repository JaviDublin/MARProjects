using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll;

using Mars.App.Classes.Phase4Dal.NonRev;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;

namespace Mars.App.UserControls.Phase4.NonRev
{
    public partial class ComparisonGrid : UserControl
    {
        public bool ShowTotalColumn
        {
            set
            {
                gvOverview.Columns[1].Visible = value;
                gvOverview.Columns[3].Visible = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ucExportToExcel.Visible = gvOverview.Rows.Count != 0;
        }

        public void LoadGrid(List<ComparisonRow> comparisonData)
        {
            gvOverview.DataSource = comparisonData;
            gvOverview.DataBind();
            if (gvOverview.Rows.Count > 0)
            {
                gvOverview.Rows[gvOverview.Rows.Count - 1].Font.Bold = true;
                gvOverview.Rows[gvOverview.Rows.Count - 1].Font.Size = FontUnit.Point(10);
            }
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

        private void ExportToExcel()
        {
            var dataTable = HtmlTableGenerator.GenerateCsvFromGridview(gvOverview);
            Session["ExportData"] = dataTable;
            Session["ExportFileType"] = "csv";
            Session["ExportFileName"] = string.Format("Comparison Export {0}", DateTime.Now.ToShortDateString());
        }
    }
}