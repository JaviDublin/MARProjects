using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FastMember;
using Mars.App.Classes.Phase4Bll;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;

namespace Mars.App.UserControls.Phase4.NonRev
{
    public partial class HistoricalTrendGrid : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ucExportToExcel.Visible = gvHistoricalTrend.Rows.Count != 0;
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
                var dataRow = new List<string> {day.ToShortDateString()};
                foreach (var code in keys)
                {
                    if (dataForDay.Count(d => d.ColumnCode == code) > 0)
                    {
                        var codeCount = dataForDay.Single(d => d.ColumnCode == code).CodeCount;
                        dataRow.Add(string.Format("{0: ##,##0}", codeCount));
                    }
                    else
                    {
                        dataRow.Add("0");
                    }
                    
                }
                //dataRow = dataForDay.Select(d => string.Format("{0:##,##0}", d.CodeCount)).ToList();
                //if (dataRow.Count == 0) continue;
                //dataRow.Insert(0, day.ToShortDateString());
                dt.Rows.Add(dataRow.ToArray());
            }

            gvHistoricalTrend.DataSource = dt;
            gvHistoricalTrend.DataBind();
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
            Session["ExportFileName"] = string.Format("HistoricalTrend Export {0}", DateTime.Now.ToShortDateString());
        }

    }
}