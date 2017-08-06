using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll;
using Mars.App.Classes.Phase4Dal.ForeignVehicles;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;
using HistoricalTrendRow = Mars.App.Classes.Phase4Dal.ForeignVehicles.Entities.HistoricalTrendRow;

namespace Mars.App.UserControls.Phase4.ForeignVehicles
{
    public partial class HistoricalTrendGrid : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var noData = gvHistoricalTrend.Rows.Count == 0;

            ucNoData.Visible = noData;
            pnlGrid.Visible = !noData;
            ucExportToExcel.Visible = !noData;
  
            
        }

        public void LoadGrid(List<HistoricalTrendRow> comparisonData)
        {
            if (comparisonData.Count == 0)
            {
                gvHistoricalTrend.DataSource = new List<EmptyRow>();
                gvHistoricalTrend.DataBind();
                return;
            }

            ExtractColumnHeaders(comparisonData);
        }

        private void ExtractColumnHeaders(List<HistoricalTrendRow> comparisonData)
        {
            comparisonData = comparisonData.OrderByDescending(d => d.Date).ToList();


            var dt = new DataTable();

            dt.Columns.Add("Date");
            //Add Headers
            var keys = comparisonData.Select(d => d.ColumnCode).Distinct().ToList();

            foreach (var code in keys)
            {
                dt.Columns.Add(code);
                
            }

            foreach (var day in comparisonData.Select(d => d.Date).Distinct().ToList())
            {

                var dataForDay = comparisonData.Where(d => d.Date == day).ToList();
                var dataColumns = new List<string> { day.ToShortDateString() };
                foreach (var code in keys)
                {
                    if (dataForDay.Count(d => d.ColumnCode == code) > 0)
                    {
                        var codeCount = dataForDay.Single(d => d.ColumnCode == code).CodeCount;
                        dataColumns.Add(string.Format("{0: ##,##0}", codeCount));
                    }
                    else
                    {
                        dataColumns.Add("0");
                    }
                    

                }

                dt.Rows.Add(dataColumns.ToArray());
            }

            gvHistoricalTrend.DataSource = dt;
            gvHistoricalTrend.DataBind();
        }

        protected void gvHistoricalTrend_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int i = 0;
                foreach (DataControlFieldCell c in e.Row.Cells)
                {
                    c.HorizontalAlign = i == 0 ? HorizontalAlign.Left : HorizontalAlign.Right;
                    i++;
                }
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
                    handled = true;
                }

            }
            return handled;
        }

        protected void ExportToExcel()
        {
            var dataTable = HtmlTableGenerator.GenerateCsvFromGridview(gvHistoricalTrend);

            Session["ExportData"] = dataTable;
            Session["ExportFileType"] = "csv";
            Session["ExportFileName"] = string.Format("Historical Trend Export {0}", DateTime.Now.ToShortDateString());
        }

    }
}