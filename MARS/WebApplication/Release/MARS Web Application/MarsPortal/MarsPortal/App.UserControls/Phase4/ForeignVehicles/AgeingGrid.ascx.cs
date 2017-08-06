using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll;
using Mars.App.Classes.Phase4Dal.ForeignVehicles.Entities;

namespace Mars.App.UserControls.Phase4.ForeignVehicles
{
    public partial class AgeingGrid : UserControl
    {
        private const string ForeignVehicleAgeingGridData = "ForeignVehicleAgeingGridData";

        private const string ForeignVehicleAgeingGridDataSortDirection = "ForeignVehicleAgeingGridDataSortDirection";
        private const string ForeignVehicleAgeingGridSortColumn = "ForeignVehicleAgeingGridSortColumn";

        public List<AgeingRow> GridData
        {
            private get { return Session[ForeignVehicleAgeingGridData] as List<AgeingRow>; }
            set { Session[ForeignVehicleAgeingGridData] = value; }
        }

        private SortDirection SortDirection
        {
            get { return (SortDirection)Session[ForeignVehicleAgeingGridDataSortDirection]; }
            set { Session[ForeignVehicleAgeingGridDataSortDirection] = value; }
        }

        private PropertyInfo SortColumn
        {
            get { return Session[ForeignVehicleAgeingGridSortColumn] as PropertyInfo; }
            set { Session[ForeignVehicleAgeingGridSortColumn] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SortColumn = null;
                SortDirection = SortDirection.Descending;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            gvAgeing.DataSource = GridData;
            gvAgeing.DataBind();

            if(!IsPostBack)
            {
                ucNoData.Visible = false;
                pnlGrid.Visible = false;
                ucExportToExcel.Visible = false;
                return;
            }
            var noData = gvAgeing.Rows.Count == 0;

            ucNoData.Visible = noData;
            pnlGrid.Visible = !noData;
            ucExportToExcel.Visible = !noData;
        }

        public void LoadGrid(List<AgeingRow> comparisonData)
        {
            GridData = comparisonData;


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

        protected void gvAgeing_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortColumn = typeof(AgeingRow).GetProperty(e.SortExpression);

            SortDirection = SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            SortGrid(SortDirection, SortColumn);
        }

        private void SortGrid(SortDirection direction, PropertyInfo sortByColumnName)
        {
            var isInt = sortByColumnName.PropertyType == typeof(int);
            var isDateTime = sortByColumnName.PropertyType == typeof(DateTime?);

            if (direction == SortDirection.Ascending)
            {
                GridData.Sort((x, y) => isDateTime ? (int)(((DateTime)(sortByColumnName.GetValue(x, null) ?? DateTime.MinValue)).Date
                                                - ((DateTime)(sortByColumnName.GetValue(y, null) ?? DateTime.MinValue)).Date
                                        ).TotalDays
                                    : isInt ? (int)sortByColumnName.GetValue(x, null) - (int)sortByColumnName.GetValue(y, null)
                                    : String.CompareOrdinal(sortByColumnName.GetValue(x, null).ToString()
                                    , sortByColumnName.GetValue(y, null).ToString())
                            );
            }
            else
            {
                GridData.Sort(
                    (x, y) =>
                        isDateTime
                            ? (int)
                                (((DateTime)(sortByColumnName.GetValue(y, null) ?? DateTime.MinValue)).Date -
                                 ((DateTime)(sortByColumnName.GetValue(x, null) ?? DateTime.MinValue)).Date).TotalDays
                            : isInt
                                ? (int)sortByColumnName.GetValue(y, null) - (int)sortByColumnName.GetValue(x, null)
                                : String.CompareOrdinal(sortByColumnName.GetValue(y, null).ToString()
                                    , sortByColumnName.GetValue(x, null).ToString()));

            }
        }


        protected void ExportToExcel()
        {
            var sb = new StringBuilder();
            sb.AppendLine(
                "Key, Fleet, 0-1 Days, 2-3 Days, 4-5 Days, 6-7 Days, 8-10 Days, 10-15 Days, 16-30 Days, 31+ Days");
            
            var body = HtmlTableGenerator.GenerateCsvFromGridview(gvAgeing, false);
            sb.Append(body);
            

            Session["ExportData"] = sb.ToString();
            Session["ExportFileType"] = "csv";
            Session["ExportFileName"] = string.Format("Ageing Export {0}", DateTime.Now);
        }
    }
}