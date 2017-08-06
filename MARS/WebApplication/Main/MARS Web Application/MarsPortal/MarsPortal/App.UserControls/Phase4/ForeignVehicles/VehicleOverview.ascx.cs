using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll;
using Mars.App.Classes.Phase4Bll.CustomGridViewItems;
using Mars.App.Classes.Phase4Dal.ForeignVehicles;
using Mars.App.Classes.Phase4Dal.ForeignVehicles.Entities;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;

namespace Mars.App.UserControls.Phase4.ForeignVehicles
{
    public partial class VehicleOverview : UserControl
    {

        public Dictionary<string, string> DistinctLocations
        {
            private get { return (Dictionary<string, string>)Session[DistinctLocationsSessionString]; }
            set
            {
                Session[DistinctLocationsSessionString] = value;
            }
        }

        public string ColumnOneName
        {
            private get { return hfColumnOneName.Value; }
            set { hfColumnOneName.Value = value; }
        }

        public string OtherColumnsName
        {
            private get { return hfOtherColumnsName.Value; }
            set { hfOtherColumnsName.Value = value; }
        }

        private const string GridDataSessionString = "FvVehicleOverviewGridDataSessionString";
        private const string DistinctLocationsSessionString = "FvDistinctLocationsSessionString";
        private const string OwningCountryList = "FvOwningCountryList";

        public List<OverviewGridItemHolder> OverviewData
        {
            private get { return (List<OverviewGridItemHolder>)Session[GridDataSessionString]; }
            set { Session[GridDataSessionString] = value; }   
        }

        public List<CountryHolder> OwningCountries
        {
            private get { return (List<CountryHolder>)Session[OwningCountryList]; }
            set { Session[OwningCountryList] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.LoadComplete += Page_LoadComplete;

            if (OverviewData != null)
            {
                BindGridView();
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (OverviewData != null)
            {
                BindGridView();
            }
        }

        private void BindGridView()
        {            
            var dataTable = new DataTable();

            var dc = new DataColumn(GridViewTemplate.LocationString, typeof(string));
            dataTable.Columns.Add(dc);


            var hiddenColumn = new DataColumn(GridViewTemplate.HiddenId, typeof(string));
            dataTable.Columns.Add(hiddenColumn);

            foreach (var oc in OwningCountries)
            {
                dataTable.Columns.Add(oc.CountryDescription, typeof(string));
            }
            //foreach (var od in OverviewData)
            //{
            //    dataTable.Columns.Add(od.CountryName, typeof(string));
            //}

            if (DistinctLocations.ContainsKey(VehicleOverviewDataAccess.TotalString))
            {
                DistinctLocations.Remove(VehicleOverviewDataAccess.TotalString);
            }
            

            //Ensure total is always at the end
            DistinctLocations.Add(VehicleOverviewDataAccess.TotalString, VehicleOverviewDataAccess.TotalString);

            dc = new DataColumn(VehicleOverviewDataAccess.TotalString, typeof(string));
            dataTable.Columns.Add(dc);

            var totalFleetCounter = new Dictionary<string, int>();
            
            foreach (var loc in DistinctLocations)
            {
                totalFleetCounter.Add(loc.Key, 0);
                var rowList = new List<string> {loc.Value, loc.Key};

                int totalCount = 0;
                foreach (var od in OverviewData)
                {
                    int foreignVehicleCount = 0;

                    var fvh = od.ForeignVehiclesHolder.FirstOrDefault(d => d.LocationId == loc.Key);

                    if (fvh != null)
                    {
                        foreignVehicleCount = fvh.VehicleCount;
                        totalCount += foreignVehicleCount;
                    }
                    
                    var lb = foreignVehicleCount.ToString(CultureInfo.InvariantCulture);
                    rowList.Add(lb);
                }
                rowList.Add(totalCount.ToString(CultureInfo.InvariantCulture));

                var dr = dataTable.NewRow();
                
                dr.ItemArray = rowList.ToArray();
                dataTable.Rows.Add(dr);
            }

            //Add Columns to Gridview

            gvOverview.Columns.Clear();
            var templateField = new TemplateField
                                {
                                    HeaderTemplate =
                                        new GridViewTemplate(ListItemType.Header, string.Empty),
                                    ItemTemplate =
                                        new GridViewTemplate(ListItemType.Item, GridViewTemplate.LocationString)
                                };
            gvOverview.Columns.Add(templateField);

            foreach (var od in OwningCountries)
            {
                templateField = new TemplateField
                                {
                                    HeaderTemplate =
                                        new GridViewTemplate(ListItemType.Header, od.CountryDescription),
                                    ItemTemplate =
                                        new GridViewTemplate(ListItemType.Item, od.CountryDescription, od)
                                };

                gvOverview.Columns.Add(templateField);
            }

            templateField = new TemplateField
            {
                HeaderTemplate =
                    new GridViewTemplate(ListItemType.Header, VehicleOverviewDataAccess.TotalString),
                ItemTemplate =
                    new GridViewTemplate(ListItemType.Item, VehicleOverviewDataAccess.TotalString)
            };

            gvOverview.Columns.Add(templateField);

            gvOverview.DataSource = dataTable;
            gvOverview.DataBind();

            
            var newHeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            var headerCell = new TableCell { Text = ColumnOneName };
            newHeaderGridRow.Cells.Add(headerCell);

            headerCell = new TableCell { Text = OtherColumnsName, ColumnSpan = OverviewData.Count};

            newHeaderGridRow.Cells.Add(headerCell);
            newHeaderGridRow.Font.Bold = true;

            gvOverview.Controls[0].Controls.AddAt(0, newHeaderGridRow);

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
            var sb = new StringBuilder();
            sb.Append(",");
            foreach(var oc in OwningCountries)
            {
                sb.Append(oc.CountryDescription + ",");
            }
            sb.Append(VehicleOverviewDataAccess.TotalString);
            sb.AppendLine();

            foreach (var loc in DistinctLocations)
            {
                sb.Append(loc.Value + ",");
                int totalCount = 0;
                foreach (var od in OverviewData)
                {
                    var fvh = od.ForeignVehiclesHolder.FirstOrDefault(d => d.LocationId == loc.Key);
                    sb.Append((fvh == null ? "0" : fvh.VehicleCount.ToString()) + ",");
                    totalCount += fvh == null ? 0 : fvh.VehicleCount;
                }
                sb.Append(totalCount.ToString());

                sb.AppendLine();
            }

            var reportName = ColumnOneName.Contains("Check") ? "Reservation" : "Fleet";

            Session["ExportData"] = sb.ToString();
            Session["ExportFileType"] = "csv";
            Session["ExportFileName"] = string.Format("{0} Overview Export {1}", reportName, DateTime.Now);
        }



        

    }
}